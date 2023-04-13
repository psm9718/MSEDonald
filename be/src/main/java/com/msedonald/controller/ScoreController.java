package com.msedonald.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class ScoreController {

    @PostMapping("/scores/{userId}")
    public void score(@PathVariable Long userId) {

    }

    @GetMapping("/scores")
    public void getResults() {

    }
}
