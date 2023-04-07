package com.msedonald.socket;

import com.msedonald.socket.data.GameRoom;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.web.socket.WebSocketSession;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@Slf4j
@Service
public class SocketService {

    private final Map<String, WebSocketSession> sessions = new ConcurrentHashMap<>();

    public GameRoom findRoomById(String roomId) {
        return null;
    }

    public void createSession(String sessionId, WebSocketSession session) {
        sessions.put(sessionId, session);
    }
}
