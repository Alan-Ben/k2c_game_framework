using GOE;
namespace Common
{
/****
 建筑系统错误
 ****/
    class BuildingErr
    {
        static BuildingErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80002, "建筑不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80003, "建筑已经存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80004, "建筑创建失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80005, "建筑类型错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80006, "建筑满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80007, "经营建筑雇佣人数上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80008, "经营建筑委派伙伴数量上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80009, "经营建筑产品已经解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80010, "农田建筑点击间隔时间不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(80011, "经营建筑产品解锁条件未达成"));
        }
    }
}
