package com.example.cyrkapi.dtos;

import java.util.Date;

public class AuthResultDto {
    public String token;
    public Date expires_at;
    public UserReadDto user;
}