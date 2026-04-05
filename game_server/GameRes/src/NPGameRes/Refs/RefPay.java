package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 支付配置表
 * Generated with Claude Code
 */
@RefTable(tableName = "pay")
public class RefPay extends RefBase {
    private static RefTableContainer<RefPay> _g_mgr = new RefTableContainer<RefPay>();

    public static RefTableContainer<RefPay> getMgr() {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPay> getStaticContainer() {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr) {
        _g_mgr = (RefTableContainer<RefPay>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPay newRef = (RefPay) _newRef;
        id = newRef.id;
        sdk_pay_id = newRef.sdk_pay_id;
        vip_exp = newRef.vip_exp;
        show_price = newRef.show_price;
        voucher_item = newRef.voucher_item;
        recharge_diamond_num = newRef.recharge_diamond_num;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id() {
        return id;
    }

    public long id;        // ID
    public String sdk_pay_id;  // 后台商品支付id（对应sdk参数app_product_id）
    public int vip_exp;    // 获得VIP经验
    public float show_price;    // 美元价格
    public NPCommonCostItem voucher_item;  // 档位对应代金券物品
    public long recharge_diamond_num;  // 付费钻石数量
}