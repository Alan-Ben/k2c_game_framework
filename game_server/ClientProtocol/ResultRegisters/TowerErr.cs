using GOE;
namespace Common
{
/****
 爬塔相关错误
 ****/
    class TowerErr
    {
        static TowerErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(450001, "爬塔楼层已被占领"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(450002, "爬塔不能向前进攻关卡"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(450003, "爬塔研究已激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(450004, "爬塔章节研究未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(450005, "爬塔研究未找到"));
        }
    }
}
