using GOE;
namespace Common
{
/****
 竞技场错误
 ****/
    class ArenaErr
    {
        static ArenaErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230001, "英雄已经上场过"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230002, "已经选择对手"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230003, "对手数据加载失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230004, "对手未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230005, "对手未选择"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230006, "已购买回合加成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230007, "不符合购买当前buff的条件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230008, "对手大臣数据不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230009, "血量为空"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230010, "战报未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230011, "购买随机攻击次数超上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230012, "指定攻击次数超上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230013, "随机攻击次数超上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(230014, "英雄数量不足以进入竞技场"));
        }
    }
}
