using GOE;
namespace Common
{
/****
 火星系统错误
 ****/
    class MarsErr
    {
        static MarsErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560001, "火星系统未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560002, "前往火星下一个阶段未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560003, "前往火星上一个阶段未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560004, "前往火星下一个阶段错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560005, "前往火星下一个阶段时间不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560006, "前往火星未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560007, "前往火星-当前阶段已发送过留言"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560008, "前往火星-当前阶段不允许留言"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560010, "火星居民-移民未开始"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560011, "火星居民-之前的移民未结束"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560012, "火星居民-移民数量错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560013, "火星居民-进入移民次数已满"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560020, "火星居民-决策未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560021, "火星居民-决策未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560022, "火星居民-决策CD未结束"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560030, "火星居民-信件未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560031, "火星居民-信件已处理"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560032, "火星居民-信件不能处理"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560040, "火星居民-帮助未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560041, "火星居民-不是奖励型帮助"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560042, "火星居民-不是选择型帮助"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560043, "火星居民-帮助已处理"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560044, "火星居民-选择型帮助的选项下标错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560045, "火星居民-没有足够的空闲人口"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560050, "火星建筑-未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560051, "火星建筑-已建造"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560052, "火星建筑-建造失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560053, "火星建筑-尚未建造"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560054, "火星建筑-正在创建（升级）中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560055, "火星建筑-不在创建（升级）中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560056, "火星建筑-未达到创建（升级）时间"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560057, "火星建筑-部件未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560058, "火星建筑-派遣人数超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560059, "火星建筑-达到建筑允许的人口上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560060, "火星建筑-部件等级到达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560061, "火星建筑-部件等级未满级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560062, "火星建筑-建造队列超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560063, "火星建筑-已完成升级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560070, "火星科技-科技未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560071, "火星科技-前置科技未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560072, "火星科技-正在升级中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560073, "火星科技-不在升级中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560074, "火星科技-升级时间不够"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560075, "火星科技-升级队列不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560076, "火星科技-完成升级"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560080, "火星探索-探索次数不够"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560081, "火星探索队伍-队伍不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560082, "火星探索队伍-尚未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560083, "火星探索队伍-队伍状态错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560084, "火星探索队伍-大臣已经被使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560085, "火星探索队伍-大臣数量达到上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560086, "火星探索队伍-大臣重复使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560087, "火星探索事件-事件不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560088, "火星探索事件-事件已经被占据"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560089, "火星探索队伍-队伍切换状态失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560090, "火星探索事件-事件类型错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560091, "火星探索事件-事件未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560092, "火星探索事件-没有空闲位置"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560093, "火星探索事件-已领奖"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560094, "火星探索-未找到火星矿"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560095, "火星探索-火星矿已过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560096, "火星探索-火星矿已占领"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560097, "火星探索-火星矿进攻失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560098, "火星探索-火星矿无资源"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560099, "火星探索队伍-尚未修复队伍损耗"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560100, "火星探索队伍-队伍无损耗"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560101, "火星探索队伍-无占领火星矿"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560102, "火星减少时间道具-道具类型不支持"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560103, "火星探索-火星矿被其他玩家占领"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560104, "火星探索-火星矿采集速度错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560105, "火星探索-火星矿有其他玩家前往"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560106, "火星探索-已经占有"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560107, "火星公会分享矿-未找到分享记录"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560108, "火星探索-公会成员已占领该矿"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(560109, "火星探索队伍-准备修复的伤兵数超过当前已有的伤兵数"));
        }
    }
}
