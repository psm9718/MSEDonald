package com.msedonald.socket;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.msedonald.socket.data.MessageDTO;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.CloseStatus;
import org.springframework.web.socket.TextMessage;
import org.springframework.web.socket.WebSocketSession;
import org.springframework.web.socket.handler.TextWebSocketHandler;

import java.io.IOException;
import java.time.LocalDateTime;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@Slf4j
@Component
@RequiredArgsConstructor
public class WebSocketHandler extends TextWebSocketHandler {

    private final ObjectMapper objectMapper;
    private final Map<String, WebSocketSession> sessions = new ConcurrentHashMap<>();

    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        String sessionId = session.getId();
        sessions.put(sessionId, session);
        log.info("session start : {}", sessionId);

        MessageDTO messageDTO = MessageDTO.builder()
                .sender(sessionId)
                .receiver("broadcast")
                .timestamp(LocalDateTime.now())
                .data("Welcome!")
                .build();

        sessions.values().forEach(s -> {
                    try {
                        if (!s.getId().equals(sessionId)) {
                            s.sendMessage(new TextMessage(objectMapper.writeValueAsString(messageDTO)));
                            log.info("> send welcome message to user {} [ {} ]", s.getId(), s.getUri());
                        }
                    } catch (IOException e) {
                        throw new RuntimeException(e);
                    }
                }
        );
    }

    @Override
    protected void handleTextMessage(WebSocketSession session, TextMessage message) throws Exception {
        String payload = message.getPayload();
        log.info("> payload {}", payload);

        MessageDTO messageDTO = objectMapper.readValue(payload, MessageDTO.class);
        log.info("> session : {} , user : {} ({})",
                session.getId(), messageDTO.sender(), messageDTO.timestamp());

        sessions.values().forEach(s -> {
                    try {
                        s.sendMessage(new TextMessage(objectMapper.writeValueAsString(messageDTO)));
                        log.info(">> message sent to user {} [ {} ]", s.getId(), s.getUri());
                    } catch (IOException e) {
                        throw new RuntimeException(e);
                    }
                }
        );

//        WebSocketSession receiver = sessions.get(session.getId());
//        if (receiver != null && receiver.isOpen()) {
//            receiver.sendMessage(new TextMessage(objectMapper.writeValueAsString(messageDTO)));
//            log.info("> message sent to {}", receiver.getId());
//        }
    }

    @Override
    public void handleTransportError(WebSocketSession session, Throwable exception) throws Exception {
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
        String sessionId = session.getId();
        sessions.remove(sessionId);

        MessageDTO messageDTO = MessageDTO.builder()
                .sender(sessionId)
                .receiver("broadcast")
                .timestamp(LocalDateTime.now())
                .data("Good bye!")
                .build();

        sessions.values().forEach(s -> {
            try {
                s.sendMessage(new TextMessage(objectMapper.writeValueAsString(messageDTO)));
                log.info(">> message sent to user {} [ {} ]", s.getId(), s.getUri());
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        });
        log.info("session {} successfully removed", sessionId);
    }
}
