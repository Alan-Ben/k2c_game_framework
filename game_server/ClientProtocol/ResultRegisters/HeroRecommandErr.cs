using GOE;
namespace Common
{
/****
 大臣推荐系统错误
 ****/
    class HeroRecommandErr
    {
        static HeroRecommandErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(340001, "大臣推荐事件不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(340002, "推荐大臣不在池子中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(340003, "推荐大臣获得条件不满足"));
        }
    }
}
