package com.example.cyrkapi.activities;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.cyrkapi.R;
import com.example.cyrkapi.adapters.CheckoutAdapter;
import com.example.cyrkapi.dtos.OrderCreateDto;
import com.example.cyrkapi.models.CartItem;
import com.example.cyrkapi.models.Order;
import com.example.cyrkapi.services.ApiService;
import com.example.cyrkapi.utils.CartManager;

import java.math.BigDecimal;
import java.util.List;

public class CheckoutActivity extends AppCompatActivity {

    private Toolbar toolbar;
    private RecyclerView recyclerViewOrderItems;
    private TextView tvOrderTotal, tvShippingCost, tvFinalTotal;
    private EditText etShippingName, etShippingAddress, etShippingPhone, etShippingEmail;
    private RadioGroup rgPaymentMethod, rgShippingMethod;
    private Button btnPlaceOrder;
    private ProgressBar progressBar;

    private CheckoutAdapter checkoutAdapter;
    private CartManager cartManager;
    private ApiService apiService;

    private BigDecimal orderTotal = BigDecimal.ZERO;
    private BigDecimal shippingCost = BigDecimal.ZERO;
    private BigDecimal finalTotal = BigDecimal.ZERO;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_checkout);

        initViews();
        setupToolbar();
        initServices();
        setupRecyclerView();
        loadOrderData();
        setupClickListeners();
        calculateShipping();
    }

    private void initViews() {
        toolbar = findViewById(R.id.toolbar);
        recyclerViewOrderItems = findViewById(R.id.recyclerViewOrderItems);
        tvOrderTotal = findViewById(R.id.tvOrderTotal);
        tvShippingCost = findViewById(R.id.tvShippingCost);
        tvFinalTotal = findViewById(R.id.tvFinalTotal);

        etShippingName = findViewById(R.id.etShippingName);
        etShippingAddress = findViewById(R.id.etShippingAddress);
        etShippingPhone = findViewById(R.id.etShippingPhone);
        etShippingEmail = findViewById(R.id.etShippingEmail);

        rgPaymentMethod = findViewById(R.id.rgPaymentMethod);
        rgShippingMethod = findViewById(R.id.rgShippingMethod);

        btnPlaceOrder = findViewById(R.id.btnPlaceOrder);
        progressBar = findViewById(R.id.progressBar);
    }

    private void setupToolbar() {
        setSupportActionBar(toolbar);
        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setTitle("Podsumowanie zamówienia");
        }

        toolbar.setNavigationOnClickListener(v -> onBackPressed());
    }

    private void initServices() {
        cartManager = CartManager.getInstance(this);
        apiService = ApiService.getInstance(this);
    }

    private void setupRecyclerView() {
        checkoutAdapter = new CheckoutAdapter();
        recyclerViewOrderItems.setLayoutManager(new LinearLayoutManager(this));
        recyclerViewOrderItems.setAdapter(checkoutAdapter);
    }

    private void loadOrderData() {
        List<CartItem> cartItems = cartManager.getCartItems();

        if (cartItems.isEmpty()) {
            Toast.makeText(this, "Koszyk jest pusty", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        checkoutAdapter.setOrderItems(cartItems);
        orderTotal = cartManager.getTotalPrice();
        updatePriceSummary();
    }

    private void setupClickListeners() {
        btnPlaceOrder.setOnClickListener(v -> placeOrder());

        rgShippingMethod.setOnCheckedChangeListener((group, checkedId) -> {
            calculateShipping();
        });
    }

    private void calculateShipping() {
        int selectedShippingId = rgShippingMethod.getCheckedRadioButtonId();

        if (selectedShippingId == R.id.rbStandardShipping) {
            shippingCost = new BigDecimal("15.00");
        } else if (selectedShippingId == R.id.rbExpressShipping) {
            shippingCost = new BigDecimal("25.00");
        } else if (selectedShippingId == R.id.rbFreeShipping) {
            shippingCost = BigDecimal.ZERO;
        } else {
            shippingCost = new BigDecimal("15.00"); // domyślnie
        }

        updatePriceSummary();
    }

    private void updatePriceSummary() {
        finalTotal = orderTotal.add(shippingCost);

        tvOrderTotal.setText(String.format("%.2f zł", orderTotal));
        tvShippingCost.setText(String.format("%.2f zł", shippingCost));
        tvFinalTotal.setText(String.format("%.2f zł", finalTotal));
    }

    private void placeOrder() {
        if (!validateInput()) {
            return;
        }

        showProgress(true);

        // Tutaj będzie tworzenie zamówienia przez API
        createOrder();
    }

    private boolean validateInput() {
        // Walidacja danych dostawy
        if (TextUtils.isEmpty(etShippingName.getText().toString().trim())) {
            etShippingName.setError("Podaj imię i nazwisko");
            etShippingName.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(etShippingAddress.getText().toString().trim())) {
            etShippingAddress.setError("Podaj adres dostawy");
            etShippingAddress.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(etShippingPhone.getText().toString().trim())) {
            etShippingPhone.setError("Podaj numer telefonu");
            etShippingPhone.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(etShippingEmail.getText().toString().trim())) {
            etShippingEmail.setError("Podaj adres email");
            etShippingEmail.requestFocus();
            return false;
        }

        // Walidacja metody płatności
        if (rgPaymentMethod.getCheckedRadioButtonId() == -1) {
            Toast.makeText(this, "Wybierz metodę płatności", Toast.LENGTH_SHORT).show();
            return false;
        }

        // Walidacja metody dostawy
        if (rgShippingMethod.getCheckedRadioButtonId() == -1) {
            Toast.makeText(this, "Wybierz metodę dostawy", Toast.LENGTH_SHORT).show();
            return false;
        }

        return true;
    }

    private void createOrder() {
        showProgress(true);

        // Przygotuj dane zamówienia
        OrderCreateDto orderDto = new OrderCreateDto();
        orderDto.user_id = 1; // TODO: Pobierz z sesji użytkownika
        orderDto.total_price = finalTotal;
        orderDto.status = "Pending";

        // Pobierz aktualną zawartość koszyka
        List<CartItem> cartItems = cartManager.getCartItems();

        // Najpierw sprawdź dostępność produktów
        apiService.validateCartAvailability(cartItems, new ApiService.ApiCallback<Boolean>() {
            @Override
            public void onSuccess(Boolean allAvailable) {
                if (!allAvailable) {
                    showProgress(false);
                    new androidx.appcompat.app.AlertDialog.Builder(CheckoutActivity.this)
                            .setTitle("Uwaga!")
                            .setMessage("Niektóre produkty w koszyku nie są już dostępne w wybranej ilości. Sprawdź koszyk i spróbuj ponownie.")
                            .setPositiveButton("OK", null)
                            .show();
                    return;
                }

                // Jeśli wszystko dostępne, utwórz zamówienie z aktualizacją stanu
                apiService.createOrderWithStockUpdate(orderDto, cartItems, new ApiService.ApiCallback<Order>() {
                    @Override
                    public void onSuccess(Order result) {
                        runOnUiThread(() -> {
                            showProgress(false);
                            onOrderCreated(result);
                        });
                    }

                    @Override
                    public void onError(String error) {
                        runOnUiThread(() -> {
                            showProgress(false);
                            Toast.makeText(CheckoutActivity.this, "Błąd tworzenia zamówienia: " + error, Toast.LENGTH_LONG).show();
                        });
                    }
                });
            }

            @Override
            public void onError(String error) {
                showProgress(false);
                Toast.makeText(CheckoutActivity.this, "Błąd sprawdzania dostępności: " + error, Toast.LENGTH_LONG).show();
            }
        });
    }

    private void onOrderCreated(Order order) {
        cartManager.clearCart();

        // Prosta wersja - pokaż alert dialog z sukcesem
        new androidx.appcompat.app.AlertDialog.Builder(this)
                .setTitle("Sukces! 🎉")
                .setMessage("Zakup się udał!\n\nNumer zamówienia: #" + order.id + "\n\nDziękujemy za zakup!")
                .setPositiveButton("OK", (dialog, which) -> {
                    // Wróć do głównej aktywności
                    Intent intent = new Intent(this, MainActivity.class);
                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
                    startActivity(intent);
                    finish();
                })
                .setCancelable(false)
                .show();
    }

    private void showProgress(boolean show) {
        progressBar.setVisibility(show ? View.VISIBLE : View.GONE);
        btnPlaceOrder.setEnabled(!show);
        btnPlaceOrder.setText(show ? "Składanie zamówienia..." : "Złóż zamówienie");
    }

    private String getSelectedPaymentMethod() {
        int selectedId = rgPaymentMethod.getCheckedRadioButtonId();
        if (selectedId == R.id.rbCreditCard) {
            return "Credit Card";
        } else if (selectedId == R.id.rbPayPal) {
            return "PayPal";
        } else if (selectedId == R.id.rbBankTransfer) {
            return "Bank Transfer";
        }
        return "Credit Card";
    }

    private String getSelectedShippingMethod() {
        int selectedId = rgShippingMethod.getCheckedRadioButtonId();
        if (selectedId == R.id.rbStandardShipping) {
            return "Standard";
        } else if (selectedId == R.id.rbExpressShipping) {
            return "Express";
        } else if (selectedId == R.id.rbFreeShipping) {
            return "Free";
        }
        return "Standard";
    }

    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }
}