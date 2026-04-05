using GOE;
namespace Common
{
/****
 跨服组队系统错误
 ****/
    class CrossTeamErr
    {
        static CrossTeamErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60001, "分组不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60002, "队伍不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60003, "未加入队伍"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60004, "已加入队伍"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60005, "创建队伍失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60006, "加入队伍失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60007, "不是队长"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60008, "不允许自由加入队伍"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60009, "队伍已满员"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60010, "队长不能退出队伍"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60011, "不满足队伍申请条件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60012, "申请数据不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60013, "申请条件类型错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60014, "已申请加入队伍"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(60015, "队伍已禁止加入"));
        }
    }
}
