package WCGCommon.XYLog.Entity;

/**
 * 充值信息,用户充值成功后上报(为防止发货过程中可能的问题数据没有成功上报导致经分上遗漏订单数据，要求在校验渠道订单合法后即刻向经分上报充值信息)
 */
public class XYChargeLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String chl;//充值渠道标识
    public String svr;//区服标识
    public int ts;//充值时间，大于0的10位Unix时间戳
    public String oid;//订单ID
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String aid;//账号ID，渠道唯一
    public int mny;//充值金额(分)，大于0；渠道提供的实际充值金额，同样的充值档位不同渠道可能会有不同折扣
    public String gc;//商品编码(充值对应渠道商品id)
    public String gn;//商品名称(钻石／元宝／月卡／某某大礼包等)
    public int ga;//充值直接获取一级代币数量，非负整数(不包含充值赠送部分、不包含首充翻倍等，例如充值6元获得60钻石+额外赠送6钻石，这里只上报60)
    public String did;//设备id
    public String ip;//客户端ip地址，可选字段

    @Override
    public String getApi()
    {
        return "charge";
    }
}
