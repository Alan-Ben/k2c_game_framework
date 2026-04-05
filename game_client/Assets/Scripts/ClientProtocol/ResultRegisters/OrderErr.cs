using GOE;
namespace Common
{
/****
 订单相关错误
 ****/
    class OrderErr
    {
        static OrderErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470001, "订单已失效"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470002, "订单不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470003, "不支持现金购买"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470004, "购买次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470005, "代金券不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470006, "礼包关联活动未开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470007, "平台回调支付用户ID不匹配"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470008, "创建订单失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470009, "平台回调支付信息不匹配"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470010, "推送礼包未过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470011, "触发冷却中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470012, "推送礼包未激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470013, "推送礼包无法自动触发"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(470014, "推送礼包没有下一个可触发礼包"));
        }
    }
}
