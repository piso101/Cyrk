package com.example.cyrkapi.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.cyrkapi.R;
import com.example.cyrkapi.dtos.UserReadDto;

import java.util.List;

public class UsersAdapter extends RecyclerView.Adapter<UsersAdapter.UserViewHolder> {

    private List<UserReadDto> users;

    public UsersAdapter(List<UserReadDto> users) {
        this.users = users;
    }

    @NonNull
    @Override
    public UserViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_user, parent, false);
        return new UserViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull UserViewHolder holder, int position) {
        UserReadDto user = users.get(position);
        holder.bind(user);
    }

    @Override
    public int getItemCount() {
        return users.size();
    }

    static class UserViewHolder extends RecyclerView.ViewHolder {
        private TextView tvUserName;
        private TextView tvUserEmail;
        private TextView tvUserPhone;
        private TextView tvUserAddress;
        private TextView tvUserId;

        public UserViewHolder(@NonNull View itemView) {
            super(itemView);
            tvUserName = itemView.findViewById(R.id.tvUserName);
            tvUserEmail = itemView.findViewById(R.id.tvUserEmail);
            tvUserPhone = itemView.findViewById(R.id.tvUserPhone);
            tvUserAddress = itemView.findViewById(R.id.tvUserAddress);
            tvUserId = itemView.findViewById(R.id.tvUserId);
        }

        public void bind(UserReadDto user) {
            tvUserName.setText(user.name + " " + user.surname);
            tvUserEmail.setText(user.email);
            tvUserPhone.setText(user.phone != null ? user.phone : "Brak telefonu");
            tvUserAddress.setText(user.address != null ? user.address : "Brak adresu");
            tvUserId.setText("ID: " + user.id);
        }
    }
}