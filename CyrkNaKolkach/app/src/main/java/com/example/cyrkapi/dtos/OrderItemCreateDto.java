package com.example.cyrkapi.dtos;

import java.math.BigDecimal;

public class OrderItemCreateDto {
    public int order_id;
    public int product_id;
    public int quantity;
    public BigDecimal price;
}