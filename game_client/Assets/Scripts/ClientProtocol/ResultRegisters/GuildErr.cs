using GOE;
namespace Common
{
/****
 联盟相关错误
 ****/
    class GuildErr
    {
        static GuildErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370001, "联盟名称已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370002, "联盟不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370003, "不是联盟成员"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370004, "没有权限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370005, "联盟简称已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370006, "加入请求不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370007, "加入请求已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370008, "玩家已加入其它联盟"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370009, "联盟成员已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370010, "不符合联盟加入条件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370011, "联盟不允许加入"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370012, "没有可加入的联盟"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370013, "联盟已解散"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370014, "成员不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370015, "不是副盟主"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370016, "还有其他成员"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370017, "联盟职位不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370018, "联盟贡献度不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370019, "不能处理非本盟的请求"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370020, "找不到盟主"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370021, "联盟职位已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370022, "字符长度超过限制"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370023, "加入联盟冷却中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370024, "没有可以处理的加入请求"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370025, "弹劾事件不能操作"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370026, "弹劾事件已经投票"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370027, "事件不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370028, "盟主主动转让冷却中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370029, "重复申请加入"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370030, "联盟满员"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370031, "联盟公开招募冷却中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370032, "联盟已领取建设阶段奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370033, "超过领取联盟活跃宝箱上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370034, "已领取联盟活跃宝箱"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370035, "超过领取联盟大礼上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370036, "联盟活跃宝箱不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370037, "没有可以领取的联盟大礼"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370038, "没有可以领取的联盟活跃宝箱"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370039, "联盟活跃宝箱已过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370040, "已领取联盟大礼"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370041, "联盟大礼已过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370042, "联盟建设阶段积分不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370043, "联盟大臣派遣属性上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370050, "公会副本数据错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370051, "无法找到公会副本"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370052, "公会副本没有解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370053, "公会副本自动启动时间不符合要求"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370054, "公会副本没有开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370055, "公会副本已开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370056, "公会副本开启失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370057, "公会副本设置等级失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370058, "公会副本等级已变化"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370059, "公会副本攻击失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370060, "公会副本怪物不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370061, "公会副本出战大臣失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370062, "公会副本怪物已击杀"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370063, "公会副本怪物前置怪物不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370064, "公会副本怪物前置怪物未击杀"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370065, "公会副本出战大臣无需恢复"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370066, "公会副本恢复次数超过上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370067, "公会副本怪物未击杀"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370068, "公会副本怪物无奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370069, "公会副本怪物已领奖"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370070, "公会副本奖励为空"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370075, "公会火星求助建筑不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370076, "公会火星求助目标不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370077, "公会火星求助自身数据"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370078, "公会火星求助目标已发起"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370079, "公会火星求助目标不能求助"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370080, "公会火星求助目标发起失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370090, "公会宝箱类型错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370091, "公会宝箱不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(370092, "公会宝箱不存在可领取的宝箱"));
        }
    }
}
