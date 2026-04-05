package WCGCommon.XYLog.Entity;

/**
 * 副本使用信息,用户到达或者通过某一副本时上报
 */
public class XYCopyLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//登录渠道标识
    public String svr;//区服标识
    public int ts;//到达或者通过副本时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public String nm;//副本名称，游戏唯一
    public String cid;//副本id，对应每个副本实例，服务器唯一
    public int sta;//用户在当前副本状态，0-进入 1-成功 2-失败

    @Override
    public String getApi()
    {
        return "copy";
    }
}
