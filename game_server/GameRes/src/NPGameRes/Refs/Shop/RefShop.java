package NPGameRes.Refs.Shop;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGTripleLong;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.List;

/**
 * 商店表
 * @author mark
 */
@RefTable(tableName = "shop")
public class RefShop extends RefBase
{
    private static RefShopMgr _g_mgr = new RefShopMgr();

    public static RefShopMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefShopMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefShopMgr) _mgr;
    }

    public static class RefShopMgr extends RefTableContainer<RefShop>
    {

        @Override
        public void _onTableLoaded()
        {
        }

    }
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefShop newRef = (RefShop) _newRef;
        id = newRef.id;
        unlock_cond = newRef.unlock_cond;
        refresh_clock = newRef.refresh_clock;
        need_refresh_item = newRef.need_refresh_item;
        free_refresh_cd = newRef.free_refresh_cd;
        pay_refresh_times_price_id = newRef.pay_refresh_times_price_id;
        pay_refresh_limit = newRef.pay_refresh_limit;
        shop_item_group_refresh_list = newRef.shop_item_group_refresh_list;
        refresh_shop_event_type = newRef.refresh_shop_event_type;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }


    //region 以下为配表字段
    public long id; // 唯一id

    public NPPlayerConditionGroupObj unlock_cond; // 解锁条件
    public NPRefreshTimeObj refresh_clock = new NPRefreshTimeObj();//刷新规则(ENPTimeRefreshType)
    public boolean need_refresh_item;//是否刷新商品
    public NPCommonItem free_refresh_cd;//免费刷新消耗
    public int pay_refresh_times_price_id;//付费刷新固定消耗
    public int pay_refresh_limit;//付费刷新次数上限
    public List<WCGTripleLong> shop_item_group_refresh_list = new ArrayList<>();//商品抽取数量(商品组:商品基础数量:折扣商品基础数量)
    // 刷新商店的事件枚举
    public String refresh_shop_event_type;

    //endregion

    @RefField(isIgnore = true)
    public List<ShopItemGroupDrop> discountWeightList = new ArrayList<>();

    public void setShopItemGroupDropList(List<ShopItemGroupDrop> _list)
    {
        discountWeightList = _list;
    }
}
