package com.example.cyrkapi.models;

import java.util.Date;

public class Inventory {
    public int id;
    public int product_id;
    public int quantity;
    public Date last_updated;
    public Date created_at;
    public Product product; // Może być null
}