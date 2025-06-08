package com.example.cyrkapi.models;

import java.util.Date;

public class Shipping {
    public int id;
    public int order_id;
    public String carrier;
    public String tracking_number;
    public String status;
    public Date estimated_delivery;
    public String address;
    public Date created_at;
    public Order order; // Może być null
}