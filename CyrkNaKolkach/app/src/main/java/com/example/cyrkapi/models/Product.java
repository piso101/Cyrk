package com.example.cyrkapi.models;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Date;
import java.util.List;

public class Product implements Serializable {
    public int id;
    public String name;
    public String description;
    public BigDecimal price;
    public Integer category_id;
    public int stock;
    public Date created_at;
//    public Category category; // Może być null
//    public Inventory inventory; // Może być null
//    public List<OrderItem> orderItems; // Może być null
}