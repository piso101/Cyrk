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
import com.google.android.material.button.MaterialButton;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class ProductsFragment extends Fragment {

    private RecyclerView recyclerView;
    private ProductsAdapter adapter;
    private SwipeRefreshLayout swipeRefreshLayout;
    private MaterialButton btnSort;
    private ApiService apiService;
    private List<Product> productList = new ArrayList<>();
    private List<Product> originalProductList = new ArrayList<>();

    // Enum dla opcji sortowania
    private enum SortOption {
        DEFAULT("Domyślne"),
        PRICE_ASC("Cena: od najniższej"),
        PRICE_DESC("Cena: od najwyższej"),
        STOCK_ASC("Stan: od najmniejszego"),
        STOCK_DESC("Stan: od największego");

        private final String displayName;

        SortOption(String displayName) {
            this.displayName = displayName;
        }

        public String getDisplayName() {
            return displayName;
        }
    }

    private SortOption currentSortOption = SortOption.DEFAULT;

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
        btnSort = view.findViewById(R.id.btnSort);

        swipeRefreshLayout.setOnRefreshListener(this::loadProducts);
        btnSort.setOnClickListener(v -> showSortDialog());
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
                        originalProductList.clear();
                        originalProductList.addAll(result);

                        productList.clear();
                        productList.addAll(result);

                        // Zastosuj bieżące sortowanie
                        applySorting(currentSortOption);

                        adapter.notifyDataSetChanged();
                        swipeRefreshLayout.setRefreshing(false);

                        updateSortButtonText();
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

    private void showSortDialog() {
        SortOption[] options = SortOption.values();
        String[] optionNames = new String[options.length];

        for (int i = 0; i < options.length; i++) {
            optionNames[i] = options[i].getDisplayName();
        }

        int selectedIndex = getCurrentSortIndex();

        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        builder.setTitle("📊 Sortowanie produktów")
                .setSingleChoiceItems(optionNames, selectedIndex, (dialog, which) -> {
                    currentSortOption = options[which];
                    applySorting(currentSortOption);
                    adapter.notifyDataSetChanged();
                    updateSortButtonText();
                    dialog.dismiss();

                    Toast.makeText(getContext(), "Sortowanie: " + currentSortOption.getDisplayName(), Toast.LENGTH_SHORT).show();
                })
                .setNegativeButton("Anuluj", null)
                .show();
    }

    private int getCurrentSortIndex() {
        SortOption[] options = SortOption.values();
        for (int i = 0; i < options.length; i++) {
            if (options[i] == currentSortOption) {
                return i;
            }
        }
        return 0;
    }

    private void applySorting(SortOption sortOption) {
        switch (sortOption) {
            case DEFAULT:
                productList.clear();
                productList.addAll(originalProductList);
                break;

            case PRICE_ASC:
                Collections.sort(productList, new PriceComparatorAsc());
                break;

            case PRICE_DESC:
                Collections.sort(productList, new PriceComparatorDesc());
                break;

            case STOCK_ASC:
                Collections.sort(productList, new StockComparatorAsc());
                break;

            case STOCK_DESC:
                Collections.sort(productList, new StockComparatorDesc());
                break;
        }
    }

    // Klasy Comparator dla sortowania - poprawione dla BigInteger
    private static class PriceComparatorAsc implements Comparator<Product> {
        @Override
        public int compare(Product p1, Product p2) {
            if (p1.price == null && p2.price == null) return 0;
            if (p1.price == null) return -1;
            if (p2.price == null) return 1;
            return p1.price.compareTo(p2.price);
        }
    }

    private static class PriceComparatorDesc implements Comparator<Product> {
        @Override
        public int compare(Product p1, Product p2) {
            if (p1.price == null && p2.price == null) return 0;
            if (p1.price == null) return 1;
            if (p2.price == null) return -1;
            return p2.price.compareTo(p1.price);
        }
    }

    private static class StockComparatorAsc implements Comparator<Product> {
        @Override
        public int compare(Product p1, Product p2) {
            return Integer.compare(p1.stock, p2.stock);
        }
    }

    private static class StockComparatorDesc implements Comparator<Product> {
        @Override
        public int compare(Product p1, Product p2) {
            return Integer.compare(p2.stock, p1.stock);
        }
    }

    private void updateSortButtonText() {
        if (currentSortOption == SortOption.DEFAULT) {
            btnSort.setText("📊 Sortuj");
        } else {
            btnSort.setText("📊 " + getShortSortName(currentSortOption));
        }
    }

    private String getShortSortName(SortOption option) {
        switch (option) {
            case PRICE_ASC: return "Cena ↑";
            case PRICE_DESC: return "Cena ↓";
            case STOCK_ASC: return "Stan ↑";
            case STOCK_DESC: return "Stan ↓";
            default: return "Sortuj";
        }
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