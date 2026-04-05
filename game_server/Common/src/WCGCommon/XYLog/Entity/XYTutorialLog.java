package WCGCommon.XYLog.Entity;

/**
 * 通过新手指引信息,用户到达或者通过某一指引页时上报
 */
public class XYTutorialLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//登录渠道标识
    public String svr;//区服标识
    public int ts;//到达或者通过指引页时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int step;//新手指引步骤序号，非负整数
    public byte stat;//用户在当前步骤状态，0-进入页面 1-通过页面

    @Override
    public String getApi()
    {
        return "tutorial";
    }
}
