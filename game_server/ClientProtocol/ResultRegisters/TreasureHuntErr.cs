using GOE;
namespace Common
{
/****
 太空寻宝相关错误
 ****/
    class TreasureHuntErr
    {
        static TreasureHuntErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460001, "太空寻宝矿石记录奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460002, "太空寻宝矿石记录奖励未达到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460003, "太空寻宝技能已激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460004, "太空寻宝技能激活失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460005, "太空寻宝奇物无产出"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460007, "太空寻宝无法领取钻石奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460008, "太空寻宝没有可领取的捕获物品"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460009, "太空寻宝捕获物品数量已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460010, "太空寻宝矿石不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460011, "太空寻宝奇物不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460012, "太空寻宝技能不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460013, "太空寻宝矿石组合不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460014, "太空寻宝没有待转换的矿石"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460015, "太空寻宝矿石记录奖励不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(460016, "太空寻宝待处理矿石数量已达上限"));
        }
    }
}
