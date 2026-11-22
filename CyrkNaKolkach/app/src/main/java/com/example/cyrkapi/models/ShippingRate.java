package com.example.cyrkapi.models;

import java.math.BigDecimal;
import java.util.Date;

public class ShippingRate {
    public int id;
    public String carrier;
    public String package_type;
    public BigDecimal price;
    public Date created_at;
}