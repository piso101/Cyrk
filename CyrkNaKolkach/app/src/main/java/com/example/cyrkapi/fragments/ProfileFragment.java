package com.example.cyrkapi.fragments;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import com.example.cyrkapi.R;
import com.example.cyrkapi.activities.EditProfileActivity;
import com.example.cyrkapi.activities.LoginActivity;
import com.example.cyrkapi.models.User;
import com.example.cyrkapi.services.ApiService;

public class ProfileFragment extends Fragment {

    private static final String TAG = "ProfileFragment";

    private SwipeRefreshLayout swipeRefreshLayout;
    private TextView tvProfileName, tvProfileSurname, tvProfileEmail, tvProfilePhone, tvProfileAddress;
    private ImageButton btnEditProfile;
    private LinearLayout layoutChangePassword, layoutLogout;

    private ApiService apiService;
    private User currentUser;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_profile, container, false);

        initViews(view);
        initApiService();
        setupClickListeners();
        loadUserProfile();

        return view;
    }

    private void initViews(View view) {
        swipeRefreshLayout = view.findViewById(R.id.swipeRefreshLayout);
        tvProfileName = view.findViewById(R.id.tvProfileName);
        tvProfileSurname = view.findViewById(R.id.tvProfileSurname);
        tvProfileEmail = view.findViewById(R.id.tvProfileEmail);
        tvProfilePhone = view.findViewById(R.id.tvProfilePhone);
        tvProfileAddress = view.findViewById(R.id.tvProfileAddress);
        btnEditProfile = view.findViewById(R.id.btnEditProfile);
        layoutChangePassword = view.findViewById(R.id.layoutChangePassword);
        layoutLogout = view.findViewById(R.id.layoutLogout);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(getContext());
    }

    private void setupClickListeners() {
        btnEditProfile.setOnClickListener(v -> openEditProfile());

        layoutChangePassword.setOnClickListener(v -> {
            Toast.makeText(getContext(), "Funkcja zmiany hasła - do implementacji", Toast.LENGTH_SHORT).show();
        });

        layoutLogout.setOnClickListener(v -> showLogoutConfirmation());

        swipeRefreshLayout.setOnRefreshListener(this::loadUserProfile);
    }

    private void loadUserProfile() {
        swipeRefreshLayout.setRefreshing(true);

        Log.d(TAG, "Loading user profile...");

        apiService.getCurrentUserDetails(new ApiService.ApiCallback<User>() {
            @Override
            public void onSuccess(User user) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Log.d(TAG, "User profile loaded successfully - ID: " + user.id + ", Email: " + user.email);
                        currentUser = user;
                        updateUserInterface(user);
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Log.e(TAG, "Error loading user profile: " + error);
                        Toast.makeText(getContext(), "Błąd ładowania profilu: " + error, Toast.LENGTH_SHORT).show();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }
        });
    }

    private void updateUserInterface(User user) {
        tvProfileName.setText(user.name != null ? user.name : "Brak");
        tvProfileSurname.setText(user.surname != null ? user.surname : "Brak");
        tvProfileEmail.setText(user.email != null ? user.email : "Brak");
        tvProfilePhone.setText(user.phone != null ? user.phone : "Brak");
        tvProfileAddress.setText(user.address != null ? user.address : "Brak");

        Log.d(TAG, "UI updated with user data");
    }

    private void openEditProfile() {
        if (currentUser == null) {
            Toast.makeText(getContext(), "Dane użytkownika nie zostały jeszcze załadowane", Toast.LENGTH_SHORT).show();
            return;
        }

        Log.d(TAG, "Opening edit profile for user ID: " + currentUser.id);

        Intent intent = new Intent(getContext(), EditProfileActivity.class);

        // Przekaż wszystkie dane jako parametry
        intent.putExtra("user_name", currentUser.name);
        intent.putExtra("user_surname", currentUser.surname);
        intent.putExtra("user_email", currentUser.email);
        intent.putExtra("user_phone", currentUser.phone);
        intent.putExtra("user_address", currentUser.address);
        intent.putExtra("user_id", currentUser.id);

        Log.d(TAG, "Starting EditProfileActivity with user data");
        startActivity(intent);
    }

    private void showLogoutConfirmation() {
        new AlertDialog.Builder(getContext())
                .setTitle("Wylogowanie")
                .setMessage("Czy na pewno chcesz się wylogować?")
                .setPositiveButton("Wyloguj", (dialog, which) -> logout())
                .setNegativeButton("Anuluj", null)
                .show();
    }

    private void logout() {
        Log.d(TAG, "User logging out");

        apiService.clearAuthToken();

        Intent intent = new Intent(getContext(), LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);

        if (getActivity() != null) {
            getActivity().finish();
        }

        Toast.makeText(getContext(), "Wylogowano pomyślnie", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onResume() {
        super.onResume();
        Log.d(TAG, "Fragment resumed - refreshing user data");
        // Odśwież dane gdy wracamy z edycji
        loadUserProfile();
    }
}