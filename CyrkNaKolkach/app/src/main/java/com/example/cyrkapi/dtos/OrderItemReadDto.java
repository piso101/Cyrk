package com.example.cyrkapi.dtos;

import java.math.BigDecimal;
import java.util.Date;

public class OrderItemReadDto {
    public int id;
    public int order_id;
    public int product_id;
    public int quantity;
    public BigDecimal price;
    public Date created_at;
}