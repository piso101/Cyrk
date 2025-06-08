package com.example.cyrkapi.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import com.example.cyrkapi.R;
import com.example.cyrkapi.adapters.UsersAdapter;
import com.example.cyrkapi.dtos.UserReadDto;
import com.example.cyrkapi.services.ApiService;

import java.util.ArrayList;
import java.util.List;

public class UsersFragment extends Fragment {

    private RecyclerView recyclerView;
    private UsersAdapter adapter;
    private SwipeRefreshLayout swipeRefreshLayout;
    private ApiService apiService;
    private List<UserReadDto> userList = new ArrayList<>();

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_users, container, false);

        initViews(view);
        setupRecyclerView();
        initApiService();
        loadUsers();

        return view;
    }

    private void initViews(View view) {
        recyclerView = view.findViewById(R.id.recyclerViewUsers);
        swipeRefreshLayout = view.findViewById(R.id.swipeRefreshLayout);

        swipeRefreshLayout.setOnRefreshListener(this::loadUsers);
    }

    private void setupRecyclerView() {
        adapter = new UsersAdapter(userList);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        recyclerView.setAdapter(adapter);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(getContext());
    }

    private void loadUsers() {
        swipeRefreshLayout.setRefreshing(true);

        apiService.getAllUsers(new ApiService.ApiCallback<List<UserReadDto>>() {
            @Override
            public void onSuccess(List<UserReadDto> result) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        userList.clear();
                        userList.addAll(result);
                        adapter.notifyDataSetChanged();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Błąd ładowania użytkowników: " + error, Toast.LENGTH_SHORT).show();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }
        });
    }
}