package com.msedonald.socket.data;

import lombok.Builder;

import java.time.LocalDateTime;

public record MessageDTO(LocalDateTime timestamp, String sender, String receiver, String data) {

    @Builder
    public MessageDTO {
    }
}
