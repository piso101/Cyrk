package com.example.cyrkapi.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.cyrkapi.R;
import com.example.cyrkapi.adapters.CartAdapter;
import com.example.cyrkapi.models.CartItem;
import com.example.cyrkapi.utils.CartManager;

import java.math.BigDecimal;
import java.util.List;

public class CartActivity extends AppCompatActivity implements CartAdapter.OnCartItemListener {

    private RecyclerView recyclerViewCart;
    private TextView tvEmptyCart, tvTotalPrice, tvTotalItems;
    private Button btnProceedToCheckout, btnClearCart;
    private Toolbar toolbar;

    private CartAdapter cartAdapter;
    private CartManager cartManager;
    private List<CartItem> cartItems;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_cart);

        initViews();
        setupToolbar();
        initCartManager();
        setupRecyclerView();
        loadCartData();
        setupClickListeners();
    }

    private void initViews() {
        toolbar = findViewById(R.id.toolbar);
        recyclerViewCart = findViewById(R.id.recyclerViewCart);
        tvEmptyCart = findViewById(R.id.tvEmptyCart); // To musi być TextView, nie LinearLayout!
        tvTotalPrice = findViewById(R.id.tvTotalPrice);
        tvTotalItems = findViewById(R.id.tvTotalItems);
        btnProceedToCheckout = findViewById(R.id.btnProceedToCheckout);
        btnClearCart = findViewById(R.id.btnClearCart);
    }

    private void setupToolbar() {
        setSupportActionBar(toolbar);
        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setTitle("Koszyk");
        }

        toolbar.setNavigationOnClickListener(v -> onBackPressed());
    }

    private void initCartManager() {
        cartManager = CartManager.getInstance(this);
    }

    private void setupRecyclerView() {
        cartAdapter = new CartAdapter(this);
        recyclerViewCart.setLayoutManager(new LinearLayoutManager(this));
        recyclerViewCart.setAdapter(cartAdapter);
    }

    private void loadCartData() {
        cartItems = cartManager.getCartItems();
        cartAdapter.setCartItems(cartItems);
        updateCartSummary();
        updateEmptyState();
    }

    private void setupClickListeners() {
        btnProceedToCheckout.setOnClickListener(v -> proceedToCheckout());
        btnClearCart.setOnClickListener(v -> clearCart());
    }

    private void updateCartSummary() {
        BigDecimal totalPrice = cartManager.getTotalPrice();
        int totalItems = cartManager.getTotalItemsCount();

        tvTotalPrice.setText(String.format("%.2f zł", totalPrice));
        tvTotalItems.setText(totalItems + " pozycji");

        btnProceedToCheckout.setEnabled(totalItems > 0);
    }

    private void updateEmptyState() {
        boolean isEmpty = cartItems.isEmpty();

        // Pokaż/ukryj odpowiednie elementy
        tvEmptyCart.setVisibility(isEmpty ? View.VISIBLE : View.GONE);
        recyclerViewCart.setVisibility(isEmpty ? View.GONE : View.VISIBLE);

        // POPRAWIONE: Pokaż przyciski i podsumowanie gdy koszyk NIE jest pusty
        findViewById(R.id.layoutCartSummary).setVisibility(isEmpty ? View.GONE : View.VISIBLE);
        findViewById(R.id.layoutCartActions).setVisibility(isEmpty ? View.GONE : View.VISIBLE);

        // Ustaw stan przycisków
        btnProceedToCheckout.setEnabled(!isEmpty);
        btnClearCart.setEnabled(!isEmpty);
    }

    private void proceedToCheckout() {
        if (cartItems.isEmpty()) {
            Toast.makeText(this, "Koszyk jest pusty", Toast.LENGTH_SHORT).show();
            return;
        }

        Intent intent = new Intent(this, CheckoutActivity.class);
        startActivity(intent);
    }

    private void clearCart() {
        cartManager.clearCart();
        loadCartData(); // Odśwież widok
        Toast.makeText(this, "Koszyk został wyczyszczony", Toast.LENGTH_SHORT).show();
    }

    @Override
    protected void onResume() {
        super.onResume();
        // Odśwież koszyk po powrocie z innych aktywności
        loadCartData();
    }

    // Implementacja OnCartItemListener
    @Override
    public void onQuantityChanged(CartItem item, int newQuantity) {
        if (newQuantity <= 0) {
            cartManager.removeProduct(item.getProduct().id);
        } else {
            cartManager.updateQuantity(item.getProduct().id, newQuantity);
        }
        loadCartData(); // Odśwież widok
    }

    @Override
    public void onItemRemoved(CartItem item) {
        cartManager.removeProduct(item.getProduct().id);
        loadCartData(); // Odśwież widok
        Toast.makeText(this, "Usunięto: " + item.getProduct().name, Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onItemClicked(CartItem item) {
        // Przejdź do szczegółów produktu
        Intent intent = new Intent(this, ProductDetailActivity.class);
        intent.putExtra(ProductDetailActivity.EXTRA_PRODUCT, item.getProduct());
        startActivity(intent);
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }
}