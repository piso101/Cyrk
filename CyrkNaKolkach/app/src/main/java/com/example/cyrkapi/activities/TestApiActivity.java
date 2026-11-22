package com.example.cyrkapi.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.cyrkapi.R;
import com.example.cyrkapi.dtos.UserMeDto;
import com.example.cyrkapi.services.ApiService;

public class TestApiActivity extends AppCompatActivity {

    private TextView tvResults;
    private Button btnTestMe, btnTestToken;
    private ApiService apiService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_test_api);

        initViews();
        initApiService();
        setupClickListeners();
    }

    private void initViews() {
        tvResults = findViewById(R.id.tvResults);
        btnTestMe = findViewById(R.id.btnTestMe);
        btnTestToken = findViewById(R.id.btnTestToken);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(this);
    }

    private void setupClickListeners() {
        btnTestMe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                testMeEndpoint();
            }
        });

        btnTestToken.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                checkToken();
            }
        });
    }

    private void testMeEndpoint() {
        tvResults.setText("Testowanie endpointu /api/Auth/me...\n");

        apiService.getCurrentUser(new ApiService.ApiCallback<UserMeDto>() {
            @Override
            public void onSuccess(UserMeDto result) {
                runOnUiThread(() -> {
                    String resultText = "✅ SUKCES!\n" +
                            "ID: " + result.id + "\n" +
                            "Email: " + result.email + "\n" +
                            "Message: " + result.message + "\n";
                    tvResults.append(resultText);
                    Toast.makeText(TestApiActivity.this, "Test zakończony sukcesem!", Toast.LENGTH_SHORT).show();
                });
            }

            @Override
            public void onError(String error) {
                runOnUiThread(() -> {
                    String errorText = "❌ BŁĄD!\n" + error + "\n";
                    tvResults.append(errorText);
                    Toast.makeText(TestApiActivity.this, "Test zakończony błędem!", Toast.LENGTH_LONG).show();
                });
            }
        });
    }

    private void checkToken() {
        String token = apiService.getAuthToken();
        String tokenInfo = "🔑 AKTUALNY TOKEN:\n" +
                (token != null ? token.substring(0, Math.min(50, token.length())) + "..." : "BRAK TOKENU") + "\n" +
                "Zalogowany: " + (apiService.isLoggedIn() ? "TAK" : "NIE") + "\n\n";
        tvResults.setText(tokenInfo);
    }
}