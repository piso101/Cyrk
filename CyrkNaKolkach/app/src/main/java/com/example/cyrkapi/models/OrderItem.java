package com.example.cyrkapi.models;

import java.math.BigDecimal;
import java.util.Date;

public class OrderItem {
    public int id;
    public int order_id;
    public int product_id;
    public int quantity;
    public BigDecimal price;
    public Date created_at;
    public Product product; // Może być null
    public Order order; // Może być null
}