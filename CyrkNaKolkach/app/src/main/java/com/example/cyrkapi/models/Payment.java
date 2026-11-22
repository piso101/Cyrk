package com.example.cyrkapi.models;

import java.util.Date;

public class Payment {
    public int id;
    public int order_id;
    public String payment_method;
    public String status;
    public String transaction_id;
    public Date created_at;
    public Order order; // Może być null
}