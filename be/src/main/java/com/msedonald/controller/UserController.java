package com.msedonald.controller;

import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class UserController {

    @PostMapping("/users")
    public void signup() {

    }

    @GetMapping("/users")
    public void login() {

    }

    @DeleteMapping("/users")
    public void logout() {

    }
}
