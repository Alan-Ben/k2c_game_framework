using GOE;
namespace Common
{
/****
 冲榜礼包错误
 ****/
    class RankGiftPackErr
    {
        static RankGiftPackErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(580001, "当前没有激活的礼包"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(580002, "礼包已过期,请刷新"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(580003, "购买次数超限"));
        }
    }
}
