package NPUSServer.NPUSUserMgr.UserComp.OrderComp;

import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.RefPay;

import java.util.ArrayList;
import java.util.List;

/**
 * 订单商品数据
 * 包含订单中商品的详细信息，包括商品类型、ID、支付信息和具体物品列表
 * 在创建订单时由对应的可购买组件生成
 */
public class OrderGoodsData
{
    // 商品ID
    private long _m_goodsId;

    // 支付配置
    private RefPay _m_refPay;

    // 订单ID
    private String _m_orderId;
    
    // 购买后获得的物品列表
    private List<NPCommonCostItem> _m_itemList = new ArrayList<>();

    public long getGoodsId()
    {
        return _m_goodsId;
    }

    public void setGoodsId(long _goodsId)
    {
        _m_goodsId = _goodsId;
    }

    public long getPayId()
    {
        return _m_refPay.Id();
    }

    public RefPay getRefPay()
    {
        return _m_refPay;
    }

    public void setRefPay(RefPay _refPay)
    {
        _m_refPay = _refPay;
    }

    public String getOrderId()
    {
        return _m_orderId;
    }

    public void setOrderId(String _orderId)
    {
        _m_orderId = _orderId;
    }

    public List<NPCommonCostItem> getItemList()
    {
        return _m_itemList;
    }
}
