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
import com.example.cyrkapi.adapters.OrdersAdapter;
import com.example.cyrkapi.models.Order;
import com.example.cyrkapi.services.ApiService;

import java.util.ArrayList;
import java.util.List;

public class OrdersFragment extends Fragment {

    private RecyclerView recyclerView;
    private OrdersAdapter adapter;
    private SwipeRefreshLayout swipeRefreshLayout;
    private ApiService apiService;
    private List<Order> orderList = new ArrayList<>();

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_orders, container, false);

        initViews(view);
        setupRecyclerView();
        initApiService();
        loadUserOrders(); // ZMIANA: loadOrders() -> loadUserOrders()

        return view;
    }

    private void initViews(View view) {
        recyclerView = view.findViewById(R.id.recyclerViewOrders);
        swipeRefreshLayout = view.findViewById(R.id.swipeRefreshLayout);

        swipeRefreshLayout.setOnRefreshListener(this::loadUserOrders); // ZMIANA: loadOrders() -> loadUserOrders()
    }

    private void setupRecyclerView() {
        adapter = new OrdersAdapter(orderList);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        recyclerView.setAdapter(adapter);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(getContext());
    }

    private void loadUserOrders() { // ZMIANA NAZWY: loadOrders() -> loadUserOrders()
        swipeRefreshLayout.setRefreshing(true);

        apiService.getUserOrders(new ApiService.ApiCallback<List<Order>>() { // ZMIANA: getAllOrders() -> getUserOrders()
            @Override
            public void onSuccess(List<Order> result) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        orderList.clear();
                        orderList.addAll(result);
                        adapter.notifyDataSetChanged();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Błąd ładowania zamówień: " + error, Toast.LENGTH_SHORT).show();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }
        });
    }
}