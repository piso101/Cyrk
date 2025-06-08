package com.example.cyrkapi.dtos;

import java.math.BigDecimal;
import java.util.Date;

public class OrderReadDto {
    public int id;
    public int user_id;
    public BigDecimal total_price;
    public String status;
    public Date created_at;
}