package WCGCommon.XYLog.Entity;

/**
 * 角色等级信息,角色等级发生变化后上报
 */
public class XYRoleLevelLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//登录渠道标识
    public String svr;//区服标识
    public int ts;//等级变化时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int lv;//角色等级，非负整数

    @Override
    public String getApi()
    {
        return "rolelevel";
    }
}
