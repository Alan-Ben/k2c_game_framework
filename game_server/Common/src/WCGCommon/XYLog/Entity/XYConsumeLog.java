package WCGCommon.XYLog.Entity;

/**
 * 一级代币消费信息,用户产生消费记录后上报，一级代币的流水需要记录的越详细越好，每一次变动都要记录下来，同一次变动如果有多个原因导致，
 * 也需要尽量拆分成不同的记录
 */
public class XYConsumeLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//登录渠道标识
    public String svr;//区服标识
    public int ts;//消耗时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int cnt;//消耗数量，非负整数
    public int cnta;//	一级代币消耗后当前值，非负整数
    public String tc;//道具编码(如不是道具，则填“0”)
    public String tn;//道具名称(如不是道具，则填消耗原因)
    public int ta;//道具数量，非负整数(如不是道具，则填0)
    public String did;//设备id

    @Override
    public String getApi()
    {
        return "consume";
    }
}
