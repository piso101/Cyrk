package com.example.cyrkapi.dtos;

import java.math.BigDecimal;

public class OrderCreateDto {
    public int user_id;
    public BigDecimal total_price;
    public String status;
}