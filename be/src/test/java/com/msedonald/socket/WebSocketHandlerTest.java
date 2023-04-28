package com.msedonald.socket;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.msedonald.socket.data.MessageDTO;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;

@SpringBootTest
@Transactional
class WebSocketHandlerTest {

    @Autowired ObjectMapper objectMapper;

    @Test
    @DisplayName("JSON message parse test")
    void jsonParser() throws Exception {

        LocalDateTime now = LocalDateTime.now();
        String sender = "Alice";
        String receiver = "Bob";
        String data = "Hello World!";
        String json = objectMapper.writeValueAsString(new MessageDTO(now, sender, receiver, data));

        System.out.println(json);

    }


}