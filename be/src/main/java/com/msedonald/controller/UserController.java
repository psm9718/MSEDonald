package com.msedonald.controller;

import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class UserController {

    @PostMapping("/api/users")
    public void signup() {

    }

    @GetMapping("/api/users")
    public void login() {

    }

    @DeleteMapping("/api/users")
    public void logout() {

    }
}
