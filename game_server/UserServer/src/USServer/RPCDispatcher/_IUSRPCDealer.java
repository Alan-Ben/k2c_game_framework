package USServer.RPCDispatcher;

import NPUSServer.NPUserServer;

/**
 * 处理RPC对象的接口，主要是为了取到USServer
 */
public interface _IUSRPCDealer {
    NPUserServer getUSServer();
}
