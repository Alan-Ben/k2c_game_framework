package NPServerProtocolWriter.NP2US.Request;

import Common.ServerObj.ServerObj_PayCallbackInfo;
import WCGCS2US_R.p004_PayOp.NP2US_R_004_001_ReqRechargeNotify;

/**
 * PayCenter到UserServer的充值相关协议Writer类
 * @author: auto generated
 * @date: 2024-08-27
 */
public class NP2US_R_Writer_004_PayOp
{
    /**
     * 创建充值通知请求协议
     * @return 充值通知协议对象
     */
    public static NP2US_R_004_001_ReqRechargeNotify make_004_001_ReqRechargeNotify(ServerObj_PayCallbackInfo _data)
    {
        NP2US_R_004_001_ReqRechargeNotify proto = new NP2US_R_004_001_ReqRechargeNotify();
        proto.setPayCallbackInfo(_data);
        return proto;
    }
}