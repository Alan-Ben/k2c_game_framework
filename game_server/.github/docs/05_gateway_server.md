# 网关服务器 (GatewayServer)

## 概述

GatewayServer 是游戏系统的网络入口，负责处理所有客户端连接、协议转发和负载均衡。作为系统的门户，它确保客户端能够安全、高效地连接到合适的游戏服务器。

## 架构设计

### 核心职责
```
[客户端连接] → [身份验证] → [负载均衡] → [协议转发] → [后端服务器]
      ↓            ↓           ↓           ↓            ↓
   [TCP管理]    [令牌验证]   [权重算法]   [消息路由]   [UserServer]
```

### 目录结构
```
GatewayServer/src/NPGateServer/
├── NPGateServer.java              # 主服务器类
├── GateServerConf.java            # 配置管理
├── NPGeneralListener/             # 网络监听器
│   ├── NPGSBasicServerListener.java
│   ├── MsgDispather/             # 消息分发
│   └── RequestDispather/         # 请求分发
├── NPGSCallBack/                 # 回调处理
├── GMCommand/                    # GM命令
└── USRefVersionMgr/              # 版本管理
```

## 核心功能模块

### 1. 连接管理

#### NPGateServer 主类
```java
public class NPGateServer extends _ABasicServerObj {
    private int _m_iAreaIdx;                    // 区域索引
    private int _m_iUserMaxCount;               // 最大用户数
    private int _m_iUserHandleWeight;           // 处理权重
    private String _m_sConnectIp;               // 连接IP
    private int _m_iConnectPort;                // 连接端口
    
    @Override
    public void onPSRegSuc() {
        // 向平台服务器注册网关服务器
        sendRequestToPlat(NP2PS_R_Writer_001_BasicOp.make_002_RegGS(
            GateServerConf.getInstance().getAreaTag(),
            GateServerConf.getInstance().getUserMaxCount(),
            GateServerConf.getInstance().getUserHandleWeight(),
            GateServerConf.getInstance().getConnectIp(),
            GateServerConf.getInstance().getConnectPort()
        ), new NPGS2PS_RB_Callback_RegGSServer());
    }
}
```

#### 连接状态管理
```java
public class ClientConnectionMgr {
    private Map<Long, ClientConnection> connections;     // 连接映射
    private AtomicInteger currentUserCount;              // 当前用户数
    private int maxUserCount;                            // 最大用户数
    
    public boolean acceptNewConnection(ClientConnection conn) {
        if (currentUserCount.get() >= maxUserCount) {
            // 服务器满员，拒绝连接
            conn.sendErrorAndClose(GateErr.SERVER_FULL.getCode());
            return false;
        }
        
        connections.put(conn.getConnectionId(), conn);
        currentUserCount.incrementAndGet();
        return true;
    }
    
    public void removeConnection(long connectionId) {
        ClientConnection conn = connections.remove(connectionId);
        if (conn != null) {
            currentUserCount.decrementAndGet();
        }
    }
}
```

### 2. 负载均衡

#### 权重算法实现
```java
public class LoadBalancer {
    private List<UserServerInfo> userServers;
    private AtomicInteger roundRobinIndex;
    
    public UserServerInfo selectUserServer(long playerId) {
        // 基于权重的轮询算法
        int totalWeight = userServers.stream()
            .mapToInt(UserServerInfo::getWeight)
            .sum();
        
        int randomWeight = ThreadLocalRandom.current().nextInt(totalWeight);
        int currentWeight = 0;
        
        for (UserServerInfo server : userServers) {
            currentWeight += server.getWeight();
            if (randomWeight < currentWeight) {
                return server;
            }
        }
        
        // 备用方案：轮询
        return userServers.get(roundRobinIndex.getAndIncrement() % userServers.size());
    }
    
    public UserServerInfo selectByPlayerId(long playerId) {
        // 基于玩家ID的一致性哈希
        int hash = Long.hashCode(playerId);
        int serverIndex = Math.abs(hash) % userServers.size();
        return userServers.get(serverIndex);
    }
}
```

#### 服务器状态监控
```java
public class UserServerInfo {
    private int serverId;
    private String serverIp;
    private int serverPort;
    private int currentUserCount;
    private int maxUserCount;
    private int weight;
    private long lastHeartbeat;
    private ServerStatus status;
    
    public boolean isAvailable() {
        return status == ServerStatus.ONLINE &&
               currentUserCount < maxUserCount &&
               (System.currentTimeMillis() - lastHeartbeat) < HEARTBEAT_TIMEOUT;
    }
    
    public double getLoadFactor() {
        return (double) currentUserCount / maxUserCount;
    }
    
    public int calculateDynamicWeight() {
        // 根据负载动态调整权重
        double loadFactor = getLoadFactor();
        if (loadFactor > 0.9) {
            return weight / 4;  // 高负载时降低权重
        } else if (loadFactor > 0.7) {
            return weight / 2;  // 中等负载时减半权重
        } else {
            return weight;      // 低负载时保持权重
        }
    }
}
```

### 3. 协议转发

#### 消息路由机制
```java
public class MessageRouter {
    private Map<Integer, ProtocolHandler> protocolHandlers;
    
    public void routeMessage(ClientConnection clientConn, ByteBuffer msgBuffer) {
        try {
            // 解析协议头
            int mainProtocol = msgBuffer.get();
            int subProtocol = msgBuffer.get();
            
            // 查找处理器
            ProtocolHandler handler = findHandler(mainProtocol, subProtocol);
            
            if (handler != null) {
                handler.handleMessage(clientConn, msgBuffer);
            } else {
                // 默认转发到用户服务器
                forwardToUserServer(clientConn, msgBuffer);
            }
            
        } catch (Exception e) {
            GSLog.error("Message routing failed", e);
            clientConn.sendError(GateErr.PROTOCOL_ERROR.getCode());
        }
    }
    
    private void forwardToUserServer(ClientConnection clientConn, ByteBuffer msgBuffer) {
        // 获取玩家对应的用户服务器
        UserServerInfo userServer = getUserServerForPlayer(clientConn.getPlayerId());
        
        if (userServer == null || !userServer.isAvailable()) {
            clientConn.sendError(GateErr.SERVER_UNAVAILABLE.getCode());
            return;
        }
        
        // 构造转发消息
        ForwardMessage forwardMsg = new ForwardMessage();
        forwardMsg.setClientId(clientConn.getConnectionId());
        forwardMsg.setPlayerId(clientConn.getPlayerId());
        forwardMsg.setOriginalMessage(msgBuffer);
        
        // 发送到用户服务器
        userServer.sendMessage(forwardMsg);
    }
}
```

#### 响应回传机制
```java
public class ResponseHandler {
    
    public void handleUserServerResponse(long clientId, ByteBuffer responseBuffer) {
        ClientConnection clientConn = getClientConnection(clientId);
        
        if (clientConn != null && clientConn.isConnected()) {
            // 直接转发响应到客户端
            clientConn.sendMessage(responseBuffer);
        } else {
            // 客户端已断线，记录日志
            GSLog.warn("Client {} disconnected, response dropped", clientId);
        }
    }
    
    public void handleBroadcastMessage(List<Long> targetClients, ByteBuffer messageBuffer) {
        for (Long clientId : targetClients) {
            ClientConnection clientConn = getClientConnection(clientId);
            if (clientConn != null && clientConn.isConnected()) {
                clientConn.sendMessage(messageBuffer.duplicate());
            }
        }
    }
}
```

### 4. 身份验证

#### 登录验证流程
```java
public class AuthenticationHandler {
    
    public void handleLoginRequest(ClientConnection clientConn, GC2GS_001_001_ReqLogin loginReq) {
        try {
            // 1. 基础参数验证
            if (!validateLoginParams(loginReq)) {
                clientConn.sendError(LoginErr.INVALID_PARAMS.getCode());
                return;
            }
            
            // 2. 令牌验证
            AuthToken token = parseAuthToken(loginReq.getToken());
            if (!isValidToken(token)) {
                clientConn.sendError(LoginErr.INVALID_TOKEN.getCode());
                return;
            }
            
            // 3. 检查用户状态
            PlayerStatus status = checkPlayerStatus(token.getPlayerId());
            if (status == PlayerStatus.BANNED) {
                clientConn.sendError(LoginErr.PLAYER_BANNED.getCode());
                return;
            }
            
            // 4. 分配用户服务器
            UserServerInfo userServer = loadBalancer.selectUserServer(token.getPlayerId());
            if (userServer == null) {
                clientConn.sendError(GateErr.NO_AVAILABLE_SERVER.getCode());
                return;
            }
            
            // 5. 转发登录请求到用户服务器
            forwardLoginToUserServer(clientConn, loginReq, userServer);
            
        } catch (Exception e) {
            GSLog.error("Login authentication failed", e);
            clientConn.sendError(CommErr.SYS_ERR.getCode());
        }
    }
    
    private boolean isValidToken(AuthToken token) {
        // 验证令牌签名
        if (!verifyTokenSignature(token)) {
            return false;
        }
        
        // 检查令牌过期时间
        if (token.isExpired()) {
            return false;
        }
        
        // 检查令牌来源
        if (!isValidTokenSource(token.getSource())) {
            return false;
        }
        
        return true;
    }
}
```

#### 会话管理
```java
public class SessionManager {
    private Map<Long, PlayerSession> playerSessions;
    private Map<Long, Long> connectionToPlayer;
    
    public void createSession(long playerId, long connectionId, UserServerInfo userServer) {
        PlayerSession session = new PlayerSession();
        session.setPlayerId(playerId);
        session.setConnectionId(connectionId);
        session.setUserServer(userServer);
        session.setLoginTime(System.currentTimeMillis());
        session.setLastActiveTime(System.currentTimeMillis());
        
        playerSessions.put(playerId, session);
        connectionToPlayer.put(connectionId, playerId);
    }
    
    public void updateSessionActivity(long playerId) {
        PlayerSession session = playerSessions.get(playerId);
        if (session != null) {
            session.setLastActiveTime(System.currentTimeMillis());
        }
    }
    
    public void removeSession(long playerId) {
        PlayerSession session = playerSessions.remove(playerId);
        if (session != null) {
            connectionToPlayer.remove(session.getConnectionId());
            
            // 通知用户服务器玩家下线
            notifyUserServerPlayerOffline(session.getUserServer(), playerId);
        }
    }
    
    public void cleanupExpiredSessions() {
        long currentTime = System.currentTimeMillis();
        
        playerSessions.entrySet().removeIf(entry -> {
            PlayerSession session = entry.getValue();
            boolean expired = (currentTime - session.getLastActiveTime()) > SESSION_TIMEOUT;
            
            if (expired) {
                connectionToPlayer.remove(session.getConnectionId());
                GSLog.info("Session expired for player: {}", session.getPlayerId());
            }
            
            return expired;
        });
    }
}
```

## 配置管理

### GateServerConf 配置类
```java
public class GateServerConf {
    private static final GateServerConf g_instance = new GateServerConf();
    
    // 网络配置
    private String _m_sConnectIp = "0.0.0.0";
    private int _m_iConnectPort = 9001;
    private int _m_iUserMaxCount = 5000;
    private int _m_iUserHandleWeight = 100;
    
    // 区域配置
    private String _m_sAreaTag = "area1";
    private int _m_iGateTypeId = 1;
    
    // 性能配置
    private int _m_iMessageQueueSize = 10000;
    private int _m_iWorkerThreadCount = 4;
    private int _m_iHeartbeatInterval = 30;
    
    public boolean init() {
        // 加载基础配置
        if (!_init("./conf/GateServerConf.properties")) {
            return false;
        }
        
        // 加载环境特定配置
        _init("./customConf/GateServerConf.properties");
        
        return true;
    }
}
```

### 配置文件示例
```properties
# conf/GateServerConf.properties

# 网络配置
gate.connect.ip=0.0.0.0
gate.connect.port=9001
gate.user.max.count=5000
gate.user.handle.weight=100

# 区域配置
gate.area.tag=area1
gate.type.id=1

# 性能配置
gate.message.queue.size=10000
gate.worker.thread.count=4
gate.heartbeat.interval=30

# 安全配置
gate.token.verify.enabled=true
gate.token.expire.time=3600
gate.max.login.attempts=5

# 日志配置
gate.log.level=INFO
gate.log.file.size=100MB
gate.log.file.count=10
```

## 性能优化

### 1. 连接池管理
```java
public class ConnectionPool {
    private final BlockingQueue<ClientConnection> idleConnections;
    private final Set<ClientConnection> activeConnections;
    private final int maxConnections;
    
    public ConnectionPool(int maxConnections) {
        this.maxConnections = maxConnections;
        this.idleConnections = new LinkedBlockingQueue<>();
        this.activeConnections = ConcurrentHashMap.newKeySet();
    }
    
    public ClientConnection acquireConnection() {
        if (activeConnections.size() >= maxConnections) {
            return null; // 连接池已满
        }
        
        ClientConnection conn = idleConnections.poll();
        if (conn == null) {
            conn = createNewConnection();
        }
        
        activeConnections.add(conn);
        return conn;
    }
    
    public void releaseConnection(ClientConnection conn) {
        activeConnections.remove(conn);
        
        if (conn.isReusable()) {
            conn.reset();
            idleConnections.offer(conn);
        } else {
            conn.close();
        }
    }
}
```

### 2. 消息队列优化
```java
public class MessageQueue {
    private final RingBuffer<MessageEvent> ringBuffer;
    private final Disruptor<MessageEvent> disruptor;
    
    public MessageQueue(int bufferSize) {
        disruptor = new Disruptor<>(MessageEvent::new, bufferSize, 
            Executors.defaultThreadFactory());
        
        disruptor.handleEventsWith(this::handleMessageEvent);
        ringBuffer = disruptor.getRingBuffer();
        disruptor.start();
    }
    
    public void publishMessage(ClientConnection conn, ByteBuffer message) {
        long sequence = ringBuffer.next();
        try {
            MessageEvent event = ringBuffer.get(sequence);
            event.setConnection(conn);
            event.setMessage(message);
        } finally {
            ringBuffer.publish(sequence);
        }
    }
    
    private void handleMessageEvent(MessageEvent event, long sequence, boolean endOfBatch) {
        try {
            messageRouter.routeMessage(event.getConnection(), event.getMessage());
        } catch (Exception e) {
            GSLog.error("Message processing failed", e);
        }
    }
}
```

### 3. 缓存优化
```java
public class GatewayCache {
    private final Cache<Long, PlayerSession> sessionCache;
    private final Cache<String, AuthToken> tokenCache;
    private final Cache<Integer, UserServerInfo> serverCache;
    
    public GatewayCache() {
        sessionCache = Caffeine.newBuilder()
            .maximumSize(10000)
            .expireAfterAccess(Duration.ofMinutes(30))
            .build();
            
        tokenCache = Caffeine.newBuilder()
            .maximumSize(50000)
            .expireAfterWrite(Duration.ofMinutes(10))
            .build();
            
        serverCache = Caffeine.newBuilder()
            .maximumSize(100)
            .expireAfterWrite(Duration.ofMinutes(5))
            .build();
    }
}
```

## 监控和诊断

### 性能指标监控
```java
public class GatewayMonitor {
    private final MeterRegistry meterRegistry;
    private final Counter connectionsTotal;
    private final Gauge currentConnections;
    private final Timer messageProcessingTime;
    
    public GatewayMonitor() {
        meterRegistry = Metrics.globalRegistry;
        
        connectionsTotal = Counter.builder("gateway.connections.total")
            .description("Total number of connections")
            .register(meterRegistry);
            
        currentConnections = Gauge.builder("gateway.connections.current")
            .description("Current number of connections")
            .register(meterRegistry, this, GatewayMonitor::getCurrentConnectionCount);
            
        messageProcessingTime = Timer.builder("gateway.message.processing.time")
            .description("Message processing time")
            .register(meterRegistry);
    }
    
    public void recordConnection() {
        connectionsTotal.increment();
    }
    
    public void recordMessageProcessing(Duration duration) {
        messageProcessingTime.record(duration);
    }
}
```

### 健康检查
```java
public class HealthChecker {
    
    public HealthStatus checkHealth() {
        HealthStatus status = new HealthStatus();
        
        // 检查连接状态
        status.setConnectionsHealthy(checkConnectionsHealth());
        
        // 检查用户服务器状态
        status.setUserServersHealthy(checkUserServersHealth());
        
        // 检查系统资源
        status.setSystemResourcesHealthy(checkSystemResources());
        
        return status;
    }
    
    private boolean checkConnectionsHealth() {
        return connectionMgr.getCurrentConnectionCount() < 
               connectionMgr.getMaxConnectionCount() * 0.9;
    }
    
    private boolean checkUserServersHealth() {
        List<UserServerInfo> servers = serverMgr.getAllUserServers();
        long healthyServers = servers.stream()
            .filter(UserServerInfo::isAvailable)
            .count();
            
        return healthyServers >= servers.size() * 0.5; // 至少50%可用
    }
}
```

## 故障排除

### 常见问题和解决方案

#### 1. 连接超时
**症状**：客户端连接经常超时
**排查步骤**：
```java
// 检查网络配置
GSLog.info("Gateway config - IP: {}, Port: {}, MaxUsers: {}", 
    config.getConnectIp(), config.getConnectPort(), config.getUserMaxCount());

// 检查当前连接数
GSLog.info("Current connections: {}/{}", 
    connectionMgr.getCurrentConnectionCount(), 
    connectionMgr.getMaxConnectionCount());

// 检查网络延迟
long ping = networkMonitor.measurePing(clientIp);
GSLog.info("Client {} ping: {}ms", clientIp, ping);
```

#### 2. 负载不均衡
**症状**：某些用户服务器负载过高
**排查步骤**：
```java
// 检查服务器权重配置
for (UserServerInfo server : userServers) {
    GSLog.info("Server {} - Weight: {}, Load: {}/{}", 
        server.getId(), server.getWeight(), 
        server.getCurrentUserCount(), server.getMaxUserCount());
}

// 检查负载均衡算法
GSLog.info("Load balancer algorithm: {}", loadBalancer.getAlgorithmType());
```

#### 3. 内存泄露
**症状**：网关服务器内存持续增长
**排查步骤**：
```java
// 检查连接池状态
GSLog.info("Connection pool - Active: {}, Idle: {}", 
    connectionPool.getActiveCount(), connectionPool.getIdleCount());

// 检查缓存大小
GSLog.info("Cache sizes - Sessions: {}, Tokens: {}, Servers: {}", 
    sessionCache.estimatedSize(), tokenCache.estimatedSize(), serverCache.estimatedSize());
```

---

**相关文档**：
- [系统总体架构](01_system_architecture.md)
- [用户服务器详解](04_user_server.md)
- [登录服务器详解](08_login_server.md)
