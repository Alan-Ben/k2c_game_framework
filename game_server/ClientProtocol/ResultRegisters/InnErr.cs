using GOE;
namespace Common
{
/****
 旅店相关错误
 ****/
    class InnErr
    {
        static InnErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430001, "旅店勋章等级已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430002, "旅店等级不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430003, "旅店菜品等级已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430004, "旅店菜品熟练度不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430007, "旅店接待客人数量不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430008, "旅店设施已建造"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430009, "旅店设施等级已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430010, "旅店菜品已解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430011, "旅店设施不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430012, "旅店菜品解锁条件未满足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430013, "旅店特殊客人已服务过"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430014, "旅店图鉴奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430015, "旅店客人未被服务"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430016, "旅店接待冷却中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430017, "旅店没有可接待的客人"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430018, "旅店没有可接待的菜品"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430019, "旅店没有可结算的客人"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430021, "旅店设施未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430022, "旅店菜品未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430023, "旅店特殊客人博物馆物品已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430024, "旅店客人未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430025, "旅店特殊客人未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430026, "旅店菜品菜谱未获得"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430027, "旅店客人已解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430028, "旅店客人解锁条件未满足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(430029, "旅店队伍内客人数量已达上限"));
        }
    }
}
