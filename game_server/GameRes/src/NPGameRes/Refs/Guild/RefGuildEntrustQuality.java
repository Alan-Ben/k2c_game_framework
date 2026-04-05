package NPGameRes.Refs.Guild;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

import java.util.List;

@RefTable(tableName = "guild_random_entrust_quality")
public class RefGuildEntrustQuality extends RefBase
{
    private static RefGuildEntrustQualityMgr _g_mgr = new RefGuildEntrustQualityMgr();

    public static RefGuildEntrustQualityMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildEntrustQualityMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildEntrustQualityMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildEntrustQuality newRef = (RefGuildEntrustQuality) _newRef;
        id = newRef.id;
        quality = newRef.quality;
        base_weight = newRef.base_weight;
        start_add_index = newRef.start_add_index;
        each_add_weight = newRef.each_add_weight;
        weight_limit = newRef.weight_limit;
        guild_random_requests_id_list = newRef.guild_random_requests_id_list;
        count = newRef.count;
        reawrd_item_list = newRef.reawrd_item_list;
        gain_currency_per = newRef.gain_currency_per;
    }

    public static class RefGuildEntrustQualityMgr extends RefTableContainer<RefGuildEntrustQuality>
    {

    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public EQuality quality;//委托品质
    public int base_weight;//基础权重
    public int start_add_index;//递增开始抽数(该抽数之后开始增加权重）
    public int each_add_weight;//单次递增权重
    public int weight_limit;//权重上限
    public List<Long> guild_random_requests_id_list;//可随机的事件id列表
    public int count;//达成所需进度
    public List<NPCommonCostItem> reawrd_item_list;//奖励列表
    public int gain_currency_per;//每次委托处理的金币收益万分比
}
