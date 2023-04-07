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

@Slf4j
@Component
@RequiredArgsConstructor
public class WebSocketHandler extends TextWebSocketHandler {

    private final SocketService socketService;
    private final ObjectMapper objectMapper;

    @Override
    public void afterConnectionEstablished(WebSocketSession session) throws Exception {
        String sessionId = session.getId();
        socketService.createSession(sessionId, session);

        MessageDTO messageDTO = MessageDTO.builder()
                .userId(sessionId)
                .data("Welcome!")
                .build();

//        session.sendMessage(Utils.get);

    }

    @Override
    protected void handleTextMessage(WebSocketSession session, TextMessage message) throws Exception {
        String payload = message.getPayload();
        log.info("> payload {}", payload);

        MessageDTO messageDTO = objectMapper.readValue(payload, MessageDTO.class);

        log.info("session : {} , user : {} ({})",
                session.getId(), messageDTO.getUserId(), messageDTO.getTimestamp());


        socketService.send(session, messageDTO);

    }

    @Override
    public void handleTransportError(WebSocketSession session, Throwable exception) throws Exception {
        super.handleTransportError(session, exception);
    }

    @Override
    public void afterConnectionClosed(WebSocketSession session, CloseStatus status) throws Exception {
        super.afterConnectionClosed(session, status);
    }
}
