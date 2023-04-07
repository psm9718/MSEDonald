package com.msedonald.socket.data;

import lombok.Builder;
import lombok.Getter;

import java.time.LocalDateTime;

@Getter
public record MessageDTO(LocalDateTime timestamp, String userId, String data) {

    @Builder
    public MessageDTO {
    }
}
