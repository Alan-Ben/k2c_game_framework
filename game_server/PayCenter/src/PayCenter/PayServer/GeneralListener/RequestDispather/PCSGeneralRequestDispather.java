package PayCenter.PayServer.GeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

/**
 * PCSGeneralRequestDispather - PayCenter通用请求分发器
 * 
 * 主要功能：
 * 1. 处理来自平台服务器的请求消息分发
 * 2. 根据请求协议号路由到对应处理器
 * 3. 管理请求-响应的生命周期
 * 4. 提供统一的请求处理接口
 * 
 * 设计特点：
 * - 继承NPRequestDispatcher框架
 * - 单例模式管理全局请求分发
 * - 初始化器模式注册子分发器
 * - 协议号路由和响应管理
 */
public class PCSGeneralRequestDispather extends NPRequestDispatcher
{
    private static final PCSGeneralRequestDispather _g_instance = new PCSGeneralRequestDispather();

    public static PCSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected PCSGeneralRequestDispather()
    {
        PayGeneral_001_RequestDispatcher_PayOp.init(this);
        PayGeneral_255_RequestDispatcher.init(this);
    }
}