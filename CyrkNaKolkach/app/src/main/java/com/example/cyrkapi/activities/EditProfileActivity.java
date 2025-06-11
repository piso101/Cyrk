package com.example.cyrkapi.activities;

import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import com.example.cyrkapi.R;
import com.example.cyrkapi.dtos.UserReadDto;
import com.example.cyrkapi.dtos.UserUpdateDto;
import com.example.cyrkapi.services.ApiService;

public class EditProfileActivity extends AppCompatActivity {

    private static final String TAG = "EditProfileActivity";

    private Toolbar toolbar;
    private EditText etEditName, etEditSurname, etEditEmail, etEditPhone, etEditAddress;
    private Button btnCancel, btnSave;
    private ProgressBar progressBar;

    private ApiService apiService;
    private int userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit_profile);

        initViews();
        setupToolbar();
        initApiService();
        loadUserData();
        setupClickListeners();
    }


    public void onError(String error) {
        runOnUiThread(() -> {
            showProgress(false);

            String errorMessage;
            if (error.contains("500")) {
                errorMessage = "Nie udało się zapisać profilu!\n\n Najprawdopodobniej:\n• Email już istnieje w systemie\n• Spróbuj użyć innego adresu email\n\n Wskazówka: Jeśli nie zmieniałeś emaila, to może być inny problem z danymi.";
            } else if (error.contains("400")) {
                errorMessage = "Nieprawidłowe dane!\nSprawdź czy wszystkie pola są wypełnione poprawnie.";
            } else {
                errorMessage = "Błąd zapisu: " + error;
            }

            Toast.makeText(EditProfileActivity.this, errorMessage, Toast.LENGTH_LONG).show();
        });
    }

    private void initViews() {
        toolbar = findViewById(R.id.toolbar);
        etEditName = findViewById(R.id.etEditName);
        etEditSurname = findViewById(R.id.etEditSurname);
        etEditEmail = findViewById(R.id.etEditEmail);
        etEditPhone = findViewById(R.id.etEditPhone);
        etEditAddress = findViewById(R.id.etEditAddress);
        btnCancel = findViewById(R.id.btnCancel);
        btnSave = findViewById(R.id.btnSave);
        progressBar = findViewById(R.id.progressBar);
    }

    private void setupToolbar() {
        setSupportActionBar(toolbar);
        if (getSupportActionBar() != null) {
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setTitle("Edytuj profil");
        }
        toolbar.setNavigationOnClickListener(v -> finish());
    }

    private void initApiService() {
        apiService = ApiService.getInstance(this);
    }

    private void loadUserData() {
        // Pobierz dane z intent
        String userName = getIntent().getStringExtra("user_name");
        String userSurname = getIntent().getStringExtra("user_surname");
        String userEmail = getIntent().getStringExtra("user_email");
        String userPhone = getIntent().getStringExtra("user_phone");
        String userAddress = getIntent().getStringExtra("user_address");
        userId = getIntent().getIntExtra("user_id", -1);

        Log.d(TAG, "Loading user data - ID: " + userId);
        Log.d(TAG, "Name: " + userName + ", Email: " + userEmail);

        // Sprawdź czy ID jest prawidłowe
        if (userId == -1) {
            Log.e(TAG, "Invalid user ID received!");
            Toast.makeText(this, "Błąd: Nieprawidłowe ID użytkownika", Toast.LENGTH_LONG).show();
            finish();
            return;
        }

        // Wypełnij pola
        etEditName.setText(userName != null ? userName : "");
        etEditSurname.setText(userSurname != null ? userSurname : "");
        etEditEmail.setText(userEmail != null ? userEmail : "");
        etEditPhone.setText(userPhone != null ? userPhone : "");
        etEditAddress.setText(userAddress != null ? userAddress : "");
    }

    private void setupClickListeners() {
        btnCancel.setOnClickListener(v -> finish());
        btnSave.setOnClickListener(v -> saveProfile());
    }

    private void saveProfile() {
        if (!validateInput()) {
            return;
        }

        showProgress(true);

        // Przygotuj dane - sprawdź czy wartości nie są null
        UserUpdateDto updateDto = new UserUpdateDto();
        updateDto.name = etEditName.getText().toString().trim();
        updateDto.surname = etEditSurname.getText().toString().trim();
        updateDto.email = etEditEmail.getText().toString().trim();

        // Dla phone i address - jeśli puste, wyślij null zamiast pustego stringa
        String phone = etEditPhone.getText().toString().trim();
        String address = etEditAddress.getText().toString().trim();

        updateDto.phone = phone.isEmpty() ? null : phone;
        updateDto.address = address.isEmpty() ? null : address;

        Log.d(TAG, "Saving profile for user ID: " + userId);
        Log.d(TAG, "Update data: " + updateDto.name + " " + updateDto.surname + " " + updateDto.email);

        apiService.updateUser(userId, updateDto, new ApiService.ApiCallback<UserReadDto>() {
            @Override
            public void onSuccess(UserReadDto result) {
                Log.d(TAG, "Profile update successful");
                runOnUiThread(() -> {
                    showProgress(false);
                    Toast.makeText(EditProfileActivity.this, "Profil został zaktualizowany!", Toast.LENGTH_SHORT).show();
                    finish();
                });
            }

            @Override
            public void onError(String error) {
                Log.e(TAG, "Profile update failed: " + error);
                runOnUiThread(() -> {
                    showProgress(false);

                    // Bardziej szczegółowe info o błędzie
                    String errorMessage = "Błąd aktualizacji profilu";
                    if (error.contains("500")) {
                        errorMessage += "\n\nBłąd serwera (500). Sprawdź:\n• Czy wszystkie pola są prawidłowe\n• Czy email nie jest już używany przez innego użytkownika";
                    }
                    errorMessage += "\n\nSzczegóły: " + error;

                    Toast.makeText(EditProfileActivity.this, errorMessage, Toast.LENGTH_LONG).show();
                });
            }
        });
    }

    private boolean validateInput() {
        // Walidacja imienia
        if (TextUtils.isEmpty(etEditName.getText().toString().trim())) {
            etEditName.setError("Imię jest wymagane");
            etEditName.requestFocus();
            return false;
        }

        // Walidacja nazwiska
        if (TextUtils.isEmpty(etEditSurname.getText().toString().trim())) {
            etEditSurname.setError("Nazwisko jest wymagane");
            etEditSurname.requestFocus();
            return false;
        }

        // Walidacja emaila
        String email = etEditEmail.getText().toString().trim();
        if (TextUtils.isEmpty(email)) {
            etEditEmail.setError("Email jest wymagany");
            etEditEmail.requestFocus();
            return false;
        }

        if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            etEditEmail.setError("Podaj prawidłowy adres email");
            etEditEmail.requestFocus();
            return false;
        }

        Log.d(TAG, "Input validation passed");
        return true;
    }

    private void showProgress(boolean show) {
        progressBar.setVisibility(show ? View.VISIBLE : View.GONE);
        btnSave.setEnabled(!show);
        btnCancel.setEnabled(!show);
        btnSave.setText(show ? "Zapisywanie..." : "Zapisz");
    }
}