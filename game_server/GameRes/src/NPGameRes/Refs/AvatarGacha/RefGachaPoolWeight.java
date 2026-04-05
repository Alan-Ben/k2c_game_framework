package NPGameRes.Refs.AvatarGacha;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

@RefTable(tableName = "gacha_quality_weight")
public class RefGachaPoolWeight extends RefBase
{
    private static RefTableContainer<RefGachaPoolWeight> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefGachaPoolWeight> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefGachaPoolWeight> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefGachaPoolWeight>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGachaPoolWeight newRef = (RefGachaPoolWeight) _newRef;
        id = newRef.id;
        pool_id = newRef.pool_id;
        quality = newRef.quality;
        base_weight = newRef.base_weight;
        start_add_index = newRef.start_add_index;
        each_add_weight = newRef.each_add_weight;
        weight_limit = newRef.weight_limit;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long pool_id;
    public EQuality quality;//品质
    public int base_weight;//基础权重
    public int start_add_index;//递增开始抽数
    public int each_add_weight;//单次递增权重
    public int weight_limit;//权重上限
}