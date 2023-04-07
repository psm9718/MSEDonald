package com.msedonald.socket.data;

import lombok.Builder;
import lombok.Getter;

@Getter
public record MessageDTO(String type, String sender, String receiver, String data) {

    @Builder
    public MessageDTO {
    }
}
