package NPGameRes.Refs.GiftPack;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "gift_pack_extra_gain")
public class RefGiftPackExtraGain extends RefBase
{
    private static RefGiftPackExtraGainMgr _g_mgr = new RefGiftPackExtraGainMgr();

    public static RefGiftPackExtraGainMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGiftPackExtraGainMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGiftPackExtraGainMgr) _mgr;
    }

    public static class RefGiftPackExtraGainMgr extends RefTableContainer<RefGiftPackExtraGain>
    {

        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGiftPackExtraGain newRef = (RefGiftPackExtraGain) _newRef;
        id = newRef.id;
        gift_pack_id = newRef.gift_pack_id;
        buy_times_range = newRef.buy_times_range;
        extra_gain_list = newRef.extra_gain_list;
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
    public long id; // 礼包ID
    public long gift_pack_id; // 礼包ID
    public WCGPairInt buy_times_range; // 购买次数范围
    public List<NPCommonCostItem> extra_gain_list = new ArrayList<>(); // 额外获得道具列表
}