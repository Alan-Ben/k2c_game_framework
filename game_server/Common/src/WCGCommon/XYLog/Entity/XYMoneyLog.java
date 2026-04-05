package WCGCommon.XYLog.Entity;

/**
 * 二级代币变化日志,二级代币变化后上报,不要求实时
 */
public class XYMoneyLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String svr;//区服标识
    public int ts;//代币产出/消耗时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int ot;//取值：1–产出；2–消耗；3–其他
    public String op;//操作
    public String nm;//代币名称
    public String mpos;//位置信息(如仓库、背包等，可选字段非必填)
    public int quab;//操作前数量，非负整数
    public int qua;//当次操作数量
    public int quaa;//操作后数量，非负整数
    public String mark;//备注(可选)

    @Override
    public String getApi()
    {
        return "money";
    }
}
