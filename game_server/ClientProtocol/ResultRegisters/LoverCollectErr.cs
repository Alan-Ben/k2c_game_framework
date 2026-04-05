using GOE;
namespace Common
{
/****
 情人收集错误
 ****/
    class LoverCollectErr
    {
        static LoverCollectErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(610001, "已选择情人目标，不可修改"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(610002, "未选择情人目标"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(610003, "情人已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(610004, "赚速不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(610005, "情人ID不在可选列表中"));
        }
    }
}
