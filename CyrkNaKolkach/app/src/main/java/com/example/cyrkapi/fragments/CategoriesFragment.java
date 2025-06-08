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
import com.example.cyrkapi.adapters.CategoriesAdapter;
import com.example.cyrkapi.models.Category;
import com.example.cyrkapi.services.ApiService;
import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.ArrayList;
import java.util.List;

public class CategoriesFragment extends Fragment {

    private RecyclerView recyclerView;
    private CategoriesAdapter adapter;
    private SwipeRefreshLayout swipeRefreshLayout;
    private FloatingActionButton fabAdd;
    private ApiService apiService;
    private List<Category> categoryList = new ArrayList<>();

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_categories, container, false);

        initViews(view);
        setupRecyclerView();
        initApiService();
        loadCategories();

        return view;
    }

    private void initViews(View view) {
        recyclerView = view.findViewById(R.id.recyclerViewCategories);
        swipeRefreshLayout = view.findViewById(R.id.swipeRefreshLayout);
        fabAdd = view.findViewById(R.id.fabAddCategory);

        swipeRefreshLayout.setOnRefreshListener(this::loadCategories);
        fabAdd.setOnClickListener(v -> addCategory());
    }

    private void setupRecyclerView() {
        adapter = new CategoriesAdapter(categoryList);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        recyclerView.setAdapter(adapter);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(getContext());
    }

    private void loadCategories() {
        swipeRefreshLayout.setRefreshing(true);

        apiService.getAllCategories(new ApiService.ApiCallback<List<Category>>() {
            @Override
            public void onSuccess(List<Category> result) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        categoryList.clear();
                        categoryList.addAll(result);
                        adapter.notifyDataSetChanged();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Błąd ładowania kategorii: " + error, Toast.LENGTH_SHORT).show();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }
        });
    }

    private void addCategory() {
        Toast.makeText(getContext(), "Funkcja dodawania kategorii - do implementacji", Toast.LENGTH_SHORT).show();
    }
}