package PayCenter.PayServer.GeneralListener.MsgDispatcher;

import NPCommon.Dispather.NPCustomMsgDispatcher;

/**
 * PCSGeneralMsgDispatcher - PayCenter通用消息分发器
 * 
 * 主要功能：
 * 1. 处理来自客户端的消息分发
 * 2. 根据消息协议号路由到对应处理器
 * 3. 提供统一的消息处理接口
 * 4. 管理消息处理的生命周期
 * 
 * 设计特点：
 * - 继承NPCustomMsgDispatcher框架
 * - 单例模式管理全局消息分发
 * - 依赖基类的消息处理机制
 */
public class PCSGeneralMsgDispatcher extends NPCustomMsgDispatcher
{
    private static final PCSGeneralMsgDispatcher _g_instance = new PCSGeneralMsgDispatcher();

    public static PCSGeneralMsgDispatcher getInstance()
    {
        return _g_instance;
    }
}