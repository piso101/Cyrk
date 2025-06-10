package com.example.cyrkapi.fragments;

import android.app.AlertDialog;
import android.content.Intent;
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
import com.example.cyrkapi.activities.ProductDetailActivity;
import com.example.cyrkapi.adapters.ProductsAdapter;
import com.example.cyrkapi.models.Product;
import com.example.cyrkapi.services.ApiService;
import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.ArrayList;
import java.util.List;

public class ProductsFragment extends Fragment {

    private RecyclerView recyclerView;
    private ProductsAdapter adapter;
    private SwipeRefreshLayout swipeRefreshLayout;
    private FloatingActionButton fabAdd;
    private ApiService apiService;
    private List<Product> productList = new ArrayList<>();

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_products, container, false);

        initViews(view);
        setupRecyclerView();
        initApiService();
        loadProducts();

        return view;
    }

    private void initViews(View view) {
        recyclerView = view.findViewById(R.id.recyclerViewProducts);
        swipeRefreshLayout = view.findViewById(R.id.swipeRefreshLayout);
        fabAdd = view.findViewById(R.id.fabAddProduct);

        swipeRefreshLayout.setOnRefreshListener(this::loadProducts);
        fabAdd.setOnClickListener(v -> showAddProductDialog());
    }

    private void setupRecyclerView() {
        adapter = new ProductsAdapter(productList, new ProductsAdapter.OnProductClickListener() {
            @Override
            public void onProductClick(Product product) {
                // Przejdź do szczegółów produktu
                Intent intent = new Intent(getContext(), ProductDetailActivity.class);
                intent.putExtra(ProductDetailActivity.EXTRA_PRODUCT_ID, product.id);
                startActivity(intent);
            }

            // Usunięte: onEditClick i onDeleteClick
        });

        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        recyclerView.setAdapter(adapter);
    }

    private void initApiService() {
        apiService = ApiService.getInstance(getContext());
    }

    private void loadProducts() {
        swipeRefreshLayout.setRefreshing(true);

        apiService.getAllProducts(new ApiService.ApiCallback<List<Product>>() {
            @Override
            public void onSuccess(List<Product> result) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        productList.clear();
                        productList.addAll(result);
                        adapter.notifyDataSetChanged();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Błąd ładowania produktów: " + error, Toast.LENGTH_SHORT).show();
                        swipeRefreshLayout.setRefreshing(false);
                    });
                }
            }
        });
    }

    private void showProductDetails(Product product) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setTitle(product.name)
                .setMessage("Opis: " + (product.description != null ? product.description : "Brak opisu") + "\n" +
                        "Cena: " + product.price + " zł\n" +
                        "Stan magazynowy: " + product.stock + "\n" +
                        "ID kategorii: " + product.category_id)
                .setPositiveButton("OK", null)
                .show();
    }

    private void showAddProductDialog() {
        // TODO: Implementacja dialogu dodawania produktu
        Toast.makeText(getContext(), "Funkcja dodawania produktu - do implementacji", Toast.LENGTH_SHORT).show();
    }

    private void showEditProductDialog(Product product) {
        // TODO: Implementacja dialogu edycji produktu
        Toast.makeText(getContext(), "Funkcja edycji produktu - do implementacji", Toast.LENGTH_SHORT).show();
    }

    private void showDeleteConfirmation(Product product) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setTitle("Usuwanie produktu")
                .setMessage("Czy na pewno chcesz usunąć produkt: " + product.name + "?")
                .setPositiveButton("Usuń", (dialog, which) -> deleteProduct(product))
                .setNegativeButton("Anuluj", null)
                .show();
    }

    private void deleteProduct(Product product) {
        apiService.deleteProduct(product.id, new ApiService.ApiCallback<Void>() {
            @Override
            public void onSuccess(Void result) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Produkt został usunięty", Toast.LENGTH_SHORT).show();
                        loadProducts(); // Odśwież listę
                    });
                }
            }

            @Override
            public void onError(String error) {
                if (getActivity() != null) {
                    getActivity().runOnUiThread(() -> {
                        Toast.makeText(getContext(), "Błąd usuwania produktu: " + error, Toast.LENGTH_SHORT).show();
                    });
                }
            }
        });
    }
}