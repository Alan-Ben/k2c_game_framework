using GOE;
namespace Common
{
/****
 妃子系统错误
 ****/
    class ConsortErr
    {
        static ConsortErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40001, "家人不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40002, "家人资质升级条件未满足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40003, "家人技能不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40004, "家人技能未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40005, "家人技能已经满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40006, "家人皮肤不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40007, "家人CG未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40008, "家人CG已领奖"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40009, "家人皮肤已解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40010, "家人星辉已解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40011, "家人已经存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40012, "家人对话未触发"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40013, "家人对话奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40014, "家人朋友圈聊天次数超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40015, "家人主动发起聊天次数超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(40016, "评价回复次数已达上限"));
        }
    }
}
