package com.example.cyrkapi.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.cyrkapi.R;
import com.example.cyrkapi.models.Order;

import java.text.SimpleDateFormat;
import java.util.List;
import java.util.Locale;

public class OrdersAdapter extends RecyclerView.Adapter<OrdersAdapter.OrderViewHolder> {

    private List<Order> orders;
    private SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());

    public OrdersAdapter(List<Order> orders) {
        this.orders = orders;
    }

    @NonNull
    @Override
    public OrderViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_order, parent, false);
        return new OrderViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull OrderViewHolder holder, int position) {
        Order order = orders.get(position);
        holder.bind(order, dateFormat);
    }

    @Override
    public int getItemCount() {
        return orders.size();
    }

    static class OrderViewHolder extends RecyclerView.ViewHolder {
        private TextView tvOrderId;
        private TextView tvOrderUser;
        private TextView tvOrderTotal;
        private TextView tvOrderStatus;
        private TextView tvOrderDate;

        public OrderViewHolder(@NonNull View itemView) {
            super(itemView);
            tvOrderId = itemView.findViewById(R.id.tvOrderId);
            tvOrderUser = itemView.findViewById(R.id.tvOrderUser);
            tvOrderTotal = itemView.findViewById(R.id.tvOrderTotal);
            tvOrderStatus = itemView.findViewById(R.id.tvOrderStatus);
            tvOrderDate = itemView.findViewById(R.id.tvOrderDate);
        }

        public void bind(Order order, SimpleDateFormat dateFormat) {
            tvOrderId.setText("Zamówienie #" + order.id);
            tvOrderUser.setText("Użytkownik ID: " + order.user_id);
            tvOrderTotal.setText(String.format("%.2f zł", order.total_price));
            tvOrderStatus.setText(order.status);
            tvOrderDate.setText(dateFormat.format(order.created_at));

            // Zmień kolor statusu w zależności od wartości
            int statusColor;
            switch (order.status.toLowerCase()) {
                case "pending":
                    statusColor = itemView.getContext().getColor(android.R.color.holo_orange_light);
                    break;
                case "completed":
                    statusColor = itemView.getContext().getColor(android.R.color.holo_green_light);
                    break;
                case "cancelled":
                    statusColor = itemView.getContext().getColor(android.R.color.holo_red_light);
                    break;
                default:
                    statusColor = itemView.getContext().getColor(android.R.color.darker_gray);
                    break;
            }
            tvOrderStatus.setBackgroundColor(statusColor);
        }
    }
}