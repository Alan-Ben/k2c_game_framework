package NPGameRes.Refs.CrystalGiftPack;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "crystal_gift_pack")
public class RefCrystalGiftPack extends RefBase
{
    private static RefCrystalGiftPackMgr _g_mgr = new RefCrystalGiftPackMgr();

    public static RefCrystalGiftPackMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefCrystalGiftPackMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefCrystalGiftPackMgr) _mgr;
    }

    public static class RefCrystalGiftPackMgr extends RefTableContainer<RefCrystalGiftPack>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCrystalGiftPack newRef = (RefCrystalGiftPack) _newRef;
        id = newRef.id;
        crystal_gift_pack_group_id = newRef.crystal_gift_pack_group_id;
        item_list = newRef.item_list;
        discount = newRef.discount;
        buy_num = newRef.buy_num;
        cost_item = newRef.cost_item;
        times_price_type_id = newRef.times_price_type_id;
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
    public long crystal_gift_pack_group_id;//礼包组id
    public ArrayList<NPCommonCostItem> item_list = new ArrayList<>();//获得物品列表
    public int discount;//折扣
    public int buy_num;//可购买次数
    public List<NPCommonCostItem> cost_item;//固定单价
    public int times_price_type_id;//单价递增消耗(有递增消耗则固定单价无效)
}