package com.example.cyrkapi.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import com.example.cyrkapi.R;
import com.example.cyrkapi.models.Product;
import com.example.cyrkapi.services.ApiService;
import com.example.cyrkapi.utils.CartManager;

public class ProductDetailActivity extends AppCompatActivity {

    private TextView tvProductName, tvProductDescription, tvProductPrice, tvProductStock, tvCategoryId;
    private ImageView ivProductImage;
    private Button btnAddToCart, btnBuyNow;
    private Toolbar toolbar;

    private Product product;
    private ApiService apiService;
    private CartManager cartManager;

    public static final String EXTRA_PRODUCT_ID = "product_id";
    public static final String EXTRA_PRODUCT = "product";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_product_detail);

        initViews();
        setupToolbar();
        initServices();
        loadProductData();
        setupClickListeners();
    }

    private void initViews() {
        toolbar = findViewById(R.id.toolbar);
        tvProductName = findViewById(R.id.tvProductName);
        tvProductDescription = findViewById(R.id.tvProductDescription);
        tvProductPrice = findViewById(R.id.tvProductPrice);
        tvProductStock = findViewById(R.id.tvProductStock);
        tvCategoryId = findViewById(R.id.tvCategoryId);
        ivProductImage = findViewById(R.id.ivProductImage);
        btnAddToCart = findViewById(R.id.btnAddToCart);
        btnBuyNow = findViewById(R.id.btnBuyNow);
    }

    private void setupToolbar() {
        setSupportActionBar(toolbar);
        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setTitle("Szczegóły produktu");
        }

        toolbar.setNavigationOnClickListener(v -> onBackPressed());
    }

    private void initServices() {
        apiService = ApiService.getInstance(this);
        cartManager = CartManager.getInstance(this);
    }

    private void loadProductData() {
        // Sprawdź czy produkt został przekazany bezpośrednio
        if (getIntent().hasExtra(EXTRA_PRODUCT)) {
            product = (Product) getIntent().getSerializableExtra(EXTRA_PRODUCT);
            displayProductData();
        }
        // Jeśli nie, pobierz po ID
        else if (getIntent().hasExtra(EXTRA_PRODUCT_ID)) {
            int productId = getIntent().getIntExtra(EXTRA_PRODUCT_ID, -1);
            if (productId != -1) {
                loadProductById(productId);
            } else {
                showError("Nieprawidłowe ID produktu");
            }
        } else {
            showError("Brak danych produktu");
        }
    }

    private void loadProductById(int productId) {
        apiService.getProduct(productId, new ApiService.ApiCallback<Product>() {
            @Override
            public void onSuccess(Product result) {
                runOnUiThread(() -> {
                    product = result;
                    displayProductData();
                });
            }

            @Override
            public void onError(String error) {
                runOnUiThread(() -> showError("Błąd ładowania produktu: " + error));
            }
        });
    }

    private void displayProductData() {
        if (product == null) return;

        tvProductName.setText(product.name);
        tvProductDescription.setText(product.description != null ? product.description : "Brak opisu");
        tvProductPrice.setText(String.format("%.2f zł", product.price));
        tvProductStock.setText("Dostępność: " + product.stock + " szt.");
        tvCategoryId.setText("Kategoria ID: " + (product.category_id != null ? product.category_id : "Brak"));

        // Ustaw kolor dostępności
        if (product.stock == 0) {
            tvProductStock.setTextColor(getColor(android.R.color.holo_red_dark));
            btnAddToCart.setEnabled(false);
            btnBuyNow.setEnabled(false);
            btnAddToCart.setText("Brak w magazynie");
        } else if (product.stock < 10) {
            tvProductStock.setTextColor(getColor(android.R.color.holo_orange_dark));
        } else {
            tvProductStock.setTextColor(getColor(android.R.color.holo_green_dark));
        }

        // Placeholder dla obrazka - w przyszłości można dodać ładowanie z URL
        ivProductImage.setImageResource(R.drawable.product_placeholder);
    }

    private void setupClickListeners() {
        btnAddToCart.setOnClickListener(v -> addToCart());
        btnBuyNow.setOnClickListener(v -> buyNow());
    }

    private void addToCart() {
        if (product != null && product.stock > 0) {
            cartManager.addProduct(product, 1);
            Toast.makeText(this, "Dodano do koszyka: " + product.name, Toast.LENGTH_SHORT).show();

            // Opcjonalnie: pokaż animację lub zaktualizuj UI
            btnAddToCart.setText("Dodano do koszyka ✓");
            btnAddToCart.postDelayed(() -> btnAddToCart.setText("Dodaj do koszyka"), 2000);
        }
    }

    private void buyNow() {
        if (product != null && product.stock > 0) {
            // Dodaj do koszyka i przejdź od razu do koszyka
            cartManager.addProduct(product, 1);

            Intent intent = new Intent(this, CartActivity.class);
            startActivity(intent);
        }
    }

    private void showError(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
        finish();
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }
}