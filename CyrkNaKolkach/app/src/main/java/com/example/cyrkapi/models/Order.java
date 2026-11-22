package com.example.cyrkapi.models;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

public class Order {
    public int id;
    public int user_id;
    public BigDecimal total_price;
    public String status;
    public Date created_at;
    public List<OrderItem> orderItems; // Może być null
    public User user; // Może być null
    public Payment payment; // Może być null
    public Shipping shipping; // Może być null
}