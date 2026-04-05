package WCGCommon.XYLog.Entity;

/**
 * 一级代币产出信息,用户有一级代币产出后上报，一级代币的流水需要记录的越详细越好，每一次变动都要记录下来，同一次变动如果有多个原因导致，
 * 也需要尽量拆分成不同的记录
 * （例如首充648翻倍，基础部分充值获得6480钻石、基础赠送部分赠送获得1000钻石，648首充翻倍赠送获得6480钻石，虽然是一次充值获得
 * ，但流水日志要分3笔，分别是3个不同的原因）；
 * 另外，角色初始创建时初始化的一级代币也要记录到一级代币流水中（例如角色创角时初始就有50钻石，那该角色的第一笔钻石流水就应该是获得50钻石
 * ，变动后余额50钻石，变动原因初始获得）
 */
public class XYOutputLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//登录渠道标识
    public String svr;//区服标识
    public int ts;//产出时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int cnt;//产出一级代币数量，非负整数
    public int cnta;//一级代币产出后当前值，非负整数
    public int owt;//产出途径类型，取值：0-其他(默认值)；1-充值基础获得； 2-充值赠送获得； 3-充值翻倍获得；例如：首充648翻倍，基础部分充值获得6480钻石、基础赠送部分赠送获得1000钻石，648首充翻倍赠送获得6480钻石，这种情况下对应三条一级代币产出流水记录，分别是：类型为 1，数量为 6480；类型为 2，数量为 1000；类型为 3，数量为 6480
    public String src;//产出途径，对应不同产出途径类型

    @Override
    public String getApi()
    {
        return "output";
    }
}
