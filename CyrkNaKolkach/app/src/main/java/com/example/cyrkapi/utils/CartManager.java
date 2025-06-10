package com.example.cyrkapi.utils;

import android.content.Context;
import android.content.SharedPreferences;

import com.example.cyrkapi.models.CartItem;
import com.example.cyrkapi.models.Product;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class CartManager {
    private static final String PREFS_NAME = "cart_prefs";
    private static final String CART_KEY = "cart_items";

    private static CartManager instance;
    private SharedPreferences sharedPreferences;
    private Gson gson;
    private Map<Integer, CartItem> cartItems;

    private CartManager(Context context) {
        sharedPreferences = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        gson = new Gson();
        cartItems = new HashMap<>();
        loadCartFromPreferences();
    }

    public static synchronized CartManager getInstance(Context context) {
        if (instance == null) {
            instance = new CartManager(context.getApplicationContext());
        }
        return instance;
    }

    public void addProduct(Product product, int quantity) {
        if (product == null || quantity <= 0) return;

        int productId = product.id;

        if (cartItems.containsKey(productId)) {
            // Jeśli produkt już istnieje, zwiększ ilość
            CartItem existingItem = cartItems.get(productId);
            int newQuantity = existingItem.getQuantity() + quantity;

            // Sprawdź czy nie przekracza dostępnego stanu
            if (newQuantity <= product.stock) {
                existingItem.setQuantity(newQuantity);
            } else {
                existingItem.setQuantity(product.stock);
            }
        } else {
            // Dodaj nowy produkt
            int finalQuantity = Math.min(quantity, product.stock);
            CartItem newItem = new CartItem(product, finalQuantity);
            cartItems.put(productId, newItem);
        }

        saveCartToPreferences();
    }

    public void removeProduct(int productId) {
        cartItems.remove(productId);
        saveCartToPreferences();
    }

    public void updateQuantity(int productId, int quantity) {
        if (quantity <= 0) {
            removeProduct(productId);
            return;
        }

        CartItem item = cartItems.get(productId);
        if (item != null) {
            // Sprawdź czy nie przekracza dostępnego stanu
            int finalQuantity = Math.min(quantity, item.getProduct().stock);
            item.setQuantity(finalQuantity);
            saveCartToPreferences();
        }
    }

    public List<CartItem> getCartItems() {
        return new ArrayList<>(cartItems.values());
    }

    public CartItem getCartItem(int productId) {
        return cartItems.get(productId);
    }

    public boolean isProductInCart(int productId) {
        return cartItems.containsKey(productId);
    }

    public int getProductQuantityInCart(int productId) {
        CartItem item = cartItems.get(productId);
        return item != null ? item.getQuantity() : 0;
    }

    public int getTotalItemsCount() {
        int total = 0;
        for (CartItem item : cartItems.values()) {
            total += item.getQuantity();
        }
        return total;
    }

    public int getUniqueItemsCount() {
        return cartItems.size();
    }

    public BigDecimal getTotalPrice() {
        BigDecimal total = BigDecimal.ZERO;
        for (CartItem item : cartItems.values()) {
            BigDecimal itemTotal = item.getProduct().price.multiply(new BigDecimal(item.getQuantity()));
            total = total.add(itemTotal);
        }
        return total;
    }

    public void clearCart() {
        cartItems.clear();
        saveCartToPreferences();
    }

    public boolean isEmpty() {
        return cartItems.isEmpty();
    }

    private void saveCartToPreferences() {
        String cartJson = gson.toJson(new ArrayList<>(cartItems.values()));
        sharedPreferences.edit().putString(CART_KEY, cartJson).apply();
    }

    private void loadCartFromPreferences() {
        String cartJson = sharedPreferences.getString(CART_KEY, null);
        if (cartJson != null && !cartJson.isEmpty()) {
            try {
                Type type = new TypeToken<List<CartItem>>(){}.getType();
                List<CartItem> items = gson.fromJson(cartJson, type);

                cartItems.clear();
                if (items != null) {
                    for (CartItem item : items) {
                        if (item.getProduct() != null) {
                            cartItems.put(item.getProduct().id, item);
                        }
                    }
                }
            } catch (Exception e) {
                // Jeśli jest błąd parsowania, wyczyść koszyk
                cartItems.clear();
                saveCartToPreferences();
            }
        }
    }

    // Metody pomocnicze dla walidacji
    public boolean validateCartItems() {
        List<Integer> itemsToRemove = new ArrayList<>();

        for (Map.Entry<Integer, CartItem> entry : cartItems.entrySet()) {
            CartItem item = entry.getValue();
            Product product = item.getProduct();

            // Sprawdź czy produkt jest jeszcze dostępny
            if (product.stock <= 0) {
                itemsToRemove.add(entry.getKey());
            }
            // Sprawdź czy ilość w koszyku nie przekracza dostępnego stanu
            else if (item.getQuantity() > product.stock) {
                item.setQuantity(product.stock);
            }
        }

        // Usuń niedostępne produkty
        for (Integer productId : itemsToRemove) {
            cartItems.remove(productId);
        }

        if (!itemsToRemove.isEmpty()) {
            saveCartToPreferences();
            return false; // Były zmiany
        }

        return true; // Wszystko OK
    }
}