using GOE;
namespace Common
{
/****
 背包道具错误
 ****/
    class BagItemErr
    {
        static BagItemErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330001, "背包物品错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330002, "背包物品不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330003, "背包物品不能出售"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330004, "背包物品不能使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330005, "背包物品使用条件错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330006, "背包物品使用下多选奖励物品错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(330007, "背包道具一键转换校验失败"));
        }
    }
}
