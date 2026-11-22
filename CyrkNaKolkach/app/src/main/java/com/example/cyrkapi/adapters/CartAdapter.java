package com.example.cyrkapi.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.cyrkapi.R;
import com.example.cyrkapi.models.CartItem;

import java.util.ArrayList;
import java.util.List;

public class CartAdapter extends RecyclerView.Adapter<CartAdapter.CartViewHolder> {

    private List<CartItem> cartItems = new ArrayList<>();
    private OnCartItemListener listener;

    public interface OnCartItemListener {
        void onQuantityChanged(CartItem item, int newQuantity);
        void onItemRemoved(CartItem item);
        void onItemClicked(CartItem item);
    }

    public CartAdapter(OnCartItemListener listener) {
        this.listener = listener;
    }

    public void setCartItems(List<CartItem> cartItems) {
        this.cartItems = cartItems != null ? cartItems : new ArrayList<>();
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public CartViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_cart, parent, false);
        return new CartViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull CartViewHolder holder, int position) {
        CartItem item = cartItems.get(position);
        holder.bind(item, listener);
    }

    @Override
    public int getItemCount() {
        return cartItems.size();
    }

    static class CartViewHolder extends RecyclerView.ViewHolder {
        private ImageView ivProductImage;
        private TextView tvProductName;
        private TextView tvProductPrice;
        private TextView tvQuantity;
        private TextView tvTotalPrice;
        private TextView tvStockInfo;
        private ImageButton btnDecrease;
        private ImageButton btnIncrease;
        private Button btnRemove;

        public CartViewHolder(@NonNull View itemView) {
            super(itemView);
            ivProductImage = itemView.findViewById(R.id.ivProductImage);
            tvProductName = itemView.findViewById(R.id.tvProductName);
            tvProductPrice = itemView.findViewById(R.id.tvProductPrice);
            tvQuantity = itemView.findViewById(R.id.tvQuantity);
            tvTotalPrice = itemView.findViewById(R.id.tvTotalPrice);
            tvStockInfo = itemView.findViewById(R.id.tvStockInfo);
            btnDecrease = itemView.findViewById(R.id.btnDecrease);
            btnIncrease = itemView.findViewById(R.id.btnIncrease);
            btnRemove = itemView.findViewById(R.id.btnRemove);
        }

        public void bind(CartItem item, OnCartItemListener listener) {
            if (item == null || item.getProduct() == null) return;

            // Podstawowe informacje o produkcie
            tvProductName.setText(item.getProduct().name);
            tvProductPrice.setText(String.format("%.2f zł", item.getProduct().price));
            tvQuantity.setText(String.valueOf(item.getQuantity()));
            tvTotalPrice.setText(String.format("%.2f zł", item.getTotalPrice()));

            // Informacja o dostępności
            int availableStock = item.getProduct().stock;
            if (availableStock == 0) {
                tvStockInfo.setText("Brak w magazynie");
                tvStockInfo.setTextColor(itemView.getContext().getColor(android.R.color.holo_red_dark));
                btnIncrease.setEnabled(false);
            } else if (item.getQuantity() >= availableStock) {
                tvStockInfo.setText("Maksymalna ilość");
                tvStockInfo.setTextColor(itemView.getContext().getColor(android.R.color.holo_orange_dark));
                btnIncrease.setEnabled(false);
            } else {
                tvStockInfo.setText("Dostępne: " + availableStock);
                tvStockInfo.setTextColor(itemView.getContext().getColor(android.R.color.holo_green_dark));
                btnIncrease.setEnabled(true);
            }

            // Placeholder dla obrazka
            ivProductImage.setImageResource(R.drawable.product_placeholder);

            // Obsługa przycisków
            btnDecrease.setEnabled(item.getQuantity() > 1);

            btnDecrease.setOnClickListener(v -> {
                if (listener != null) {
                    int newQuantity = item.getQuantity() - 1;
                    listener.onQuantityChanged(item, newQuantity);
                }
            });

            btnIncrease.setOnClickListener(v -> {
                if (listener != null) {
                    int newQuantity = item.getQuantity() + 1;
                    if (newQuantity <= availableStock) {
                        listener.onQuantityChanged(item, newQuantity);
                    }
                }
            });

            btnRemove.setOnClickListener(v -> {
                if (listener != null) {
                    listener.onItemRemoved(item);
                }
            });

            // Kliknięcie w cały element
            itemView.setOnClickListener(v -> {
                if (listener != null) {
                    listener.onItemClicked(item);
                }
            });
        }
    }
}