package NPGameRes.Refs.Shop;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 商品打折表
 * @author mark
 */
@RefTable(tableName = "shop_item_discount")
public class RefShopItemDiscount extends RefBase
{
    private static RefShopDiscountItemMgr _g_mgr = new RefShopDiscountItemMgr();

    public static RefShopDiscountItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefShopDiscountItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefShopDiscountItemMgr) _mgr;
    }

    public static class RefShopDiscountItemMgr extends RefTableContainer<RefShopItemDiscount>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefShopItemDiscount newRef = (RefShopItemDiscount) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        discount = newRef.discount;
        wei = newRef.wei;
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
    public long group_id; // 分组id
    public long discount; // 折扣
    public long wei; // 选取权重

    //endregion

}
