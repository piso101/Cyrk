package com.example.cyrkapi.activities;

import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.cyrkapi.R;
import com.example.cyrkapi.dtos.UserRegisterDto;
import com.example.cyrkapi.services.ApiService;

public class RegisterActivity extends AppCompatActivity {

    private EditText etName, etSurname, etEmail, etPassword, etConfirmPassword, etAddress, etPhone;
    private Button btnRegister, btnBackToLogin;
    private ProgressBar progressBar;
    private ApiService apiService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        initViews();
        initApiService();
        setClickListeners();
    }

    private void initViews() {
        etName = findViewById(R.id.etName);
        etSurname = findViewById(R.id.etSurname);
        etEmail = findViewById(R.id.etEmail);
        etPassword = findViewById(R.id.etPassword);
        etConfirmPassword = findViewById(R.id.etConfirmPassword);
        etAddress = findViewById(R.id.etAddress);
        etPhone = findViewById(R.id.etPhone);
        btnRegister = findViewById(R.id.btnRegister);
        btnBackToLogin = findViewById(R.id.btnBackToLogin);
        progressBar = findViewById(R.id.progressBar);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(this);
    }

    private void setClickListeners() {
        btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                register();
            }
        });

        btnBackToLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }

    private void register() {
        String name = etName.getText().toString().trim();
        String surname = etSurname.getText().toString().trim();
        String email = etEmail.getText().toString().trim();
        String password = etPassword.getText().toString().trim();
        String confirmPassword = etConfirmPassword.getText().toString().trim();
        String address = etAddress.getText().toString().trim();
        String phone = etPhone.getText().toString().trim();

        if (!validateInput(name, surname, email, password, confirmPassword)) {
            return;
        }

        showProgress(true);

        UserRegisterDto registerDto = new UserRegisterDto();
        registerDto.name = name;
        registerDto.surname = surname;
        registerDto.email = email;
        registerDto.password = password;
        registerDto.confirm_password = confirmPassword;
        registerDto.address = address.isEmpty() ? null : address;
        registerDto.phone = phone.isEmpty() ? null : phone;

        apiService.register(registerDto, new ApiService.ApiCallback<String>() {
            @Override
            public void onSuccess(String result) {
                showProgress(false);
                Toast.makeText(RegisterActivity.this, "Rejestracja zakończona pomyślnie!", Toast.LENGTH_LONG).show();
                finish();
            }

            @Override
            public void onError(String error) {
                showProgress(false);
                Toast.makeText(RegisterActivity.this, "Błąd rejestracji: " + error, Toast.LENGTH_LONG).show();
            }
        });
    }

    private boolean validateInput(String name, String surname, String email, String password, String confirmPassword) {
        if (TextUtils.isEmpty(name)) {
            etName.setError("Imię jest wymagane");
            etName.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(surname)) {
            etSurname.setError("Nazwisko jest wymagane");
            etSurname.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(email)) {
            etEmail.setError("Email jest wymagany");
            etEmail.requestFocus();
            return false;
        }

        if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            etEmail.setError("Podaj prawidłowy adres email");
            etEmail.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(password)) {
            etPassword.setError("Hasło jest wymagane");
            etPassword.requestFocus();
            return false;
        }

        if (password.length() < 6) {
            etPassword.setError("Hasło musi mieć minimum 6 znaków");
            etPassword.requestFocus();
            return false;
        }

        if (TextUtils.isEmpty(confirmPassword)) {
            etConfirmPassword.setError("Potwierdzenie hasła jest wymagane");
            etConfirmPassword.requestFocus();
            return false;
        }

        if (!password.equals(confirmPassword)) {
            etConfirmPassword.setError("Hasła nie są identyczne");
            etConfirmPassword.requestFocus();
            return false;
        }

        return true;
    }

    private void showProgress(boolean show) {
        progressBar.setVisibility(show ? View.VISIBLE : View.GONE);
        btnRegister.setEnabled(!show);
        btnBackToLogin.setEnabled(!show);
    }
}