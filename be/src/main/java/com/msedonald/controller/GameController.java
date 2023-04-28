package com.msedonald.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class GameController {

    @GetMapping("/api/test")
    public String startGame() {
        return "test api";
    }
}
