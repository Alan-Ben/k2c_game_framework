package NPGameRes.Refs.Activity;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "activity_shop_item")
public class RefActivityShopItem extends RefBase
{
    private static RefActivityShopItemMgr _g_mgr = new RefActivityShopItemMgr();

    public static RefActivityShopItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityShopItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityShopItemMgr) _mgr;
    }

    public static class RefActivityShopItemMgr extends RefTableContainer<RefActivityShopItem>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityShopItem newRef = (RefActivityShopItem) _newRef;
        id = newRef.id;
        activity_shop_id = newRef.activity_shop_id;
        item = newRef.item;
        discount = newRef.discount;
        buy_num = newRef.buy_num;
        cost_item = newRef.cost_item;
        times_price_type_id = newRef.times_price_type_id;
        is_recommend = newRef.is_recommend;
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
    public long activity_shop_id;
    public NPCommonCostItem item;//获得物品（CommonCostItem）
    public int discount;//折扣
    public int buy_num;//可购买次数
    public List<NPCommonCostItem> cost_item;//固定单价
    public int times_price_type_id;//单价递增消耗(有递增消耗则固定单价无效)
    public boolean is_recommend;//是否为推荐商品
}