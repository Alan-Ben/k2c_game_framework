package ActivitiesV01.Refs.RegularEvent;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "regular_event_shop_item")
public class RefRegularEventShopItem extends RefBase
{
    private static RefRegularEventShopItemMgr _g_mgr = new RefRegularEventShopItemMgr();

    public static RefRegularEventShopItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRegularEventShopItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRegularEventShopItemMgr) _mgr;
    }

    public static class RefRegularEventShopItemMgr extends RefTableContainer<RefRegularEventShopItem>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRegularEventShopItem newRef = (RefRegularEventShopItem) _newRef;
        id = newRef.id;
        activity_id = newRef.activity_id;
        can_direct_buy = newRef.can_direct_buy;
        item = newRef.item;
        buy_cost = newRef.buy_cost;
        buy_num = newRef.buy_num;
        use_gain_item_list = newRef.use_gain_item_list;
        use_gain_activity_currency_count = newRef.use_gain_activity_currency_count;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public long activity_id;
    public boolean can_direct_buy;
    public NPCommonCostItem item;//获得物品（CommonCostItem）
    public List<NPCommonCostItem> buy_cost;//固定单价
    public int buy_num;//可购买次数
    public List<NPCommonCostItem> use_gain_item_list;//使用获得的道具列表
    public int use_gain_activity_currency_count;//获得兑换券数量
}