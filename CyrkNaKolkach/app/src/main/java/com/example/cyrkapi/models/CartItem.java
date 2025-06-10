package com.example.cyrkapi.models;

import java.io.Serializable;
import java.math.BigDecimal;

public class CartItem implements Serializable {
    private Product product;
    private int quantity;

    public CartItem() {
        // Konstruktor bezparametrowy dla Gson
    }

    public CartItem(Product product, int quantity) {
        this.product = product;
        this.quantity = quantity;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public int getQuantity() {
        return quantity;
    }

    public void setQuantity(int quantity) {
        this.quantity = Math.max(0, quantity); // Zapobiega ujemnym wartościom
    }

    public BigDecimal getTotalPrice() {
        if (product != null && product.price != null) {
            return product.price.multiply(new BigDecimal(quantity));
        }
        return BigDecimal.ZERO;
    }

    public boolean isAvailable() {
        return product != null && product.stock >= quantity;
    }

    public int getMaxAvailableQuantity() {
        return product != null ? product.stock : 0;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;

        CartItem cartItem = (CartItem) obj;
        return product != null && product.id == cartItem.product.id;
    }

    @Override
    public int hashCode() {
        return product != null ? Integer.hashCode(product.id) : 0;
    }

    @Override
    public String toString() {
        return "CartItem{" +
                "product=" + (product != null ? product.name : "null") +
                ", quantity=" + quantity +
                ", totalPrice=" + getTotalPrice() +
                '}';
    }
}