package PayCenter.PayServer.RPCDispatcher;

/**
 * PCSRpcDispatcher - PayCenter RPC请求分发器
 * 
 * 主要功能：
 * 1. 继承基础RPC分发器功能
 * 2. 处理PayCenter相关的RPC请求
 * 3. 提供单例访问模式
 * 
 * 设计特点：
 * - 继承RPC.RpcDispatcher基础框架
 * - 单例模式管理全局RPC分发
 * - 支持PayCenter特定的RPC处理逻辑
 */
public class PCSRpcDispatcher extends RPC.RpcDispatcher
{
    private static PCSRpcDispatcher _g_instance = new PCSRpcDispatcher();

    public static PCSRpcDispatcher getInstance()
    {
        return _g_instance;
    }
}