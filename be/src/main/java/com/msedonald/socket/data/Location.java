package com.msedonald.socket.data;

import lombok.Builder;
import lombok.Getter;

@Getter
public record Location(Long x, Long y) {

    @Builder
    public Location {
    }
}
