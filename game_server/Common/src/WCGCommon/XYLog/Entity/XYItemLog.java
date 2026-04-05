package WCGCommon.XYLog.Entity;

/**
 * 道具产出消耗信息,道具有产出消耗后上报
 */
public class XYItemLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String svr;//区服标识
    public int ts;//道具变化时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public String op;//操作
    public String nm;//物品
    public String ipos;//物品所属位置
    public int quab;//操作前数量，非负整数
    public int qua;//当次操作数量, 非负整数
    public int quaa;//操作后数量，非负整数
    public String mark;//备注(可选)
    public int fm;//是否来自商城(可选)，取值：0-未知(默认值)； 1-来自商城； 2-不来自商城

    @Override
    public String getApi()
    {
        return "item";
    }
}
