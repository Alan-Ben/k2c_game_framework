using GOE;
namespace Common
{
/****
 大臣系统错误
 ****/
    class HeroErr
    {
        static HeroErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20001, "等级达到当前阶段上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20002, "骑士升级失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20003, "骑士等级不够"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20004, "骑士已达到最大阶段"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20005, "骑士技能不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20006, "骑士星级达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20007, "骑士经验技能等级达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20008, "大臣没有光环"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20009, "骑士资质技能不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20010, "骑士皮肤不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20011, "骑士不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20012, "已拥有骑士皮肤"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20013, "大臣一键升级功能未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20014, "大臣重复解锁光环"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20015, "大臣未驻扎"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20016, "大臣光环尚未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20017, "大臣已经存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20018, "大臣资质技能等级达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20019, "大臣无法驻扎到该相性建筑"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20020, "大臣资质技能不支持手动升级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20021, "藏品等级已达到最大"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20022, "藏品没有被穿戴"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20023, "该藏品数量达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20024, "藏品不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20025, "藏品技能不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20026, "藏品重塑失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20027, "藏品已锁定"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(20028, "藏品已穿戴"));
        }
    }
}
