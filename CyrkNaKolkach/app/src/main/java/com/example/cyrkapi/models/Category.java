package com.example.cyrkapi.models;

import java.util.Date;
import java.util.List;

public class Category {
    public int id;
    public String name;
    public String description;
    public Date created_at;
    public List<Product> products; // Może być null
}