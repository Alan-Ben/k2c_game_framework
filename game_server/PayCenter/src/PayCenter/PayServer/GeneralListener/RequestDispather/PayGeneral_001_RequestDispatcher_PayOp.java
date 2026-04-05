package PayCenter.PayServer.GeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp.RequestDealer_ToPay_R_001_001_GetPayCallbackList;
import PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp.RequestDealer_ToPay_R_001_002_NotifyPayCallbackHadPush;
import PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp.RequestDealer_ToPay_R_001_003_NotifyPayCallbackHadProcess;

/**
 * PCSGeneral_001_RequestDispatcher_PayOp - 支付操作请求分发器
 * 
 * 主要功能：
 * 1. 注册协议号001相关的请求处理器
 * 2. 管理支付操作相关的请求路由
 * 
 * 设计特点：
 * - 静态初始化模式
 * - 集中管理支付操作请求处理器
 */
public class PayGeneral_001_RequestDispatcher_PayOp extends NPRequestDispatcher
{
    public static void init(PCSGeneralRequestDispather _dispatcher)
    {
        // 注册支付操作相关的请求处理器
         _dispatcher.regHandler(new RequestDealer_ToPay_R_001_001_GetPayCallbackList());
         _dispatcher.regHandler(new RequestDealer_ToPay_R_001_002_NotifyPayCallbackHadPush());
         _dispatcher.regHandler(new RequestDealer_ToPay_R_001_003_NotifyPayCallbackHadProcess());
    }
}