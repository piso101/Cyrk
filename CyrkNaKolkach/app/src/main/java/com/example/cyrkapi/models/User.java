package com.example.cyrkapi.models;

import java.util.Date;
import java.util.List;

public class User {
    public int id;
    public String name;
    public String surname;
    public String email;
    public String password_hash;
    public String address;
    public String phone;
    public Date created_at;
    public List<Order> orders; // Może być null
}