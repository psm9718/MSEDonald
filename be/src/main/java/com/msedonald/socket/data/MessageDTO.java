package com.msedonald.socket.data;

import lombok.Builder;
import lombok.Getter;

import java.time.LocalDateTime;

@Getter
public record MessageDTO(LocalDateTime timestamp, String sender, String receiver, String data) {

    @Builder
    public MessageDTO {
    }
}
