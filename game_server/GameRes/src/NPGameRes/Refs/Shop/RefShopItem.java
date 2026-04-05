package NPGameRes.Refs.Shop;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.List;

/**
 * 商品表
 * @author mark
 */
@RefTable(tableName = "shop_item")
public class RefShopItem extends RefBase
{
    private static RefShopItemMgr _g_mgr = new RefShopItemMgr();

    public static RefShopItemMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefShopItemMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefShopItemMgr) _mgr;
    }

    public static class RefShopItemMgr extends RefTableContainer<RefShopItem>
    {

        @Override
        public void _onTableLoaded()
        {
        }
    }
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefShopItem newRef = (RefShopItem) _newRef;
        id = newRef.id;
        item = newRef.item;
        discount_group_id = newRef.discount_group_id;
        base_buy_num = newRef.base_buy_num;
        ext_buy_num = newRef.ext_buy_num;
        cost_item = newRef.cost_item;
        times_price_type_id = newRef.times_price_type_id;
        is_forever = newRef.is_forever;
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
    public long id; //唯一id

    public NPCommonCostItem item;//获得物品
    public long discount_group_id;//被选为打折商品时的折扣权重表组id
    public int base_buy_num;//固定购买次数
    public NPPlayerVariableGroupObj ext_buy_num;//额外购买次数(高级公式)
    public List<NPCommonCostItem> cost_item;// 固定单价
    public int times_price_type_id;//单价递增消耗(优先于固定消耗)
    public boolean is_forever;//是否终身限购

    //endregion

    @RefField(isIgnore = true)
    public WeightValueList<RefShopItemDiscount> discountWeightList = new WeightValueList<>();

    public void setDiscountWeightList(WeightValueList<RefShopItemDiscount> weightList)
    {
        discountWeightList = weightList;
    }
}
