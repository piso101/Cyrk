package com.example.cyrkapi.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;

import com.example.cyrkapi.R;
import com.example.cyrkapi.fragments.CategoriesFragment;
import com.example.cyrkapi.fragments.OrdersFragment;
import com.example.cyrkapi.fragments.ProductsFragment;
import com.example.cyrkapi.fragments.UsersFragment;
import com.example.cyrkapi.dtos.UserMeDto;
import com.example.cyrkapi.services.ApiService;
import com.google.android.material.bottomnavigation.BottomNavigationView;

public class MainActivity extends AppCompatActivity {

    private BottomNavigationView bottomNavigationView;
    private ApiService apiService;
    private Toolbar toolbar;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        initViews();
        initApiService();
        setupToolbar();
        setupBottomNavigation();
        loadUserInfo();

        // Domyślnie załaduj fragment produktów
        if (savedInstanceState == null) {
            loadFragment(new ProductsFragment());
        }
    }

    private void initViews() {
        bottomNavigationView = findViewById(R.id.bottom_navigation);
        toolbar = findViewById(R.id.toolbar);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(this);
    }

    private void setupToolbar() {
        setSupportActionBar(toolbar);
        if (getSupportActionBar() != null) {
            getSupportActionBar().setTitle("Cyrk API Manager");
        }
    }

    private void setupBottomNavigation() {
        bottomNavigationView.setOnItemSelectedListener(item -> {
            Fragment selectedFragment = null;

            int itemId = item.getItemId();
            if (itemId == R.id.nav_products) {
                selectedFragment = new ProductsFragment();
            } else if (itemId == R.id.nav_categories) {
                selectedFragment = new CategoriesFragment();
            } else if (itemId == R.id.nav_orders) {
                selectedFragment = new OrdersFragment();
            } else if (itemId == R.id.nav_users) {
                selectedFragment = new UsersFragment();
            }

            if (selectedFragment != null) {
                loadFragment(selectedFragment);
                return true;
            }
            return false;
        });
    }

    private void loadFragment(Fragment fragment) {
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.fragment_container, fragment);
        transaction.commit();
    }

    private void loadUserInfo() {
        apiService.getCurrentUser(new ApiService.ApiCallback<UserMeDto>() {
            @Override
            public void onSuccess(UserMeDto result) {
                if (getSupportActionBar() != null) {
                    getSupportActionBar().setSubtitle("Zalogowany: " + result.email);
                }
            }

            @Override
            public void onError(String error) {
                Toast.makeText(MainActivity.this, "Błąd ładowania danych użytkownika", Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int itemId = item.getItemId();

        // DODAJ TO:
        if (itemId == R.id.action_cart) {
            Intent intent = new Intent(this, CartActivity.class);
            startActivity(intent);
            return true;
        }

        if (itemId == R.id.action_logout) {
            logout();
            return true;
        } else if (itemId == R.id.action_refresh) {
            refreshCurrentFragment();
            return true;
        } else if (itemId == R.id.action_test_api) {
            Intent intent = new Intent(this, TestApiActivity.class);
            startActivity(intent);
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    private void logout() {
        apiService.clearAuthToken();
        Intent intent = new Intent(this, LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
        Toast.makeText(this, "Wylogowano pomyślnie", Toast.LENGTH_SHORT).show();
    }

    private void refreshCurrentFragment() {
        Fragment currentFragment = getSupportFragmentManager().findFragmentById(R.id.fragment_container);
        if (currentFragment != null) {
            FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
            transaction.detach(currentFragment);
            transaction.attach(currentFragment);
            transaction.commit();
        }
        Toast.makeText(this, "Odświeżono", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        // Zablokuj wracanie do ekranu logowania
        // Zamiast tego minimalizuj aplikację
        moveTaskToBack(true);
    }
}