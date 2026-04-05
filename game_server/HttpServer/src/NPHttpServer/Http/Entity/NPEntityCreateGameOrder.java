package NPHttpServer.Http.Entity;

import java.util.ArrayList;
import java.util.List;

/**
 * 游戏下单请求数据对象
 */
public class NPEntityCreateGameOrder
{
    // 玩家cid
    private long cid;

    // 商品id列表（可重复，每个id生成一笔订单）
    private List<String> productIds;

    // 渠道（网页充值站点）订单标识
    private String channelCode;

    // 操作原因（选填）
    private String reason;

    public NPEntityCreateGameOrder()
    {
        productIds = new ArrayList<>();
        channelCode = "";
        reason = "";
    }

    public long getCid() { return cid; }

    public void setCid(long cid) { this.cid = cid; }

    public List<String> getProductIds() { return productIds; }

    public void addProductId(String id) { productIds.add(id); }

    public String getChannelCode() { return channelCode; }

    public void setChannelCode(String channelCode) { this.channelCode = channelCode; }

    public String getReason() { return reason; }

    public void setReason(String reason) { this.reason = reason; }
}
