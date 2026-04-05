package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

@RefTable(tableName = "quality_ext")
public class RefNPQualityExt extends RefBase
{
    private static RefNPQualityExtMgr _g_mgr = new RefNPQualityExtMgr();

    public static RefNPQualityExtMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefNPQualityExtMgr extends RefTableContainer<RefNPQualityExt>
    {
    }

    @Override
    public RefNPQualityExtMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNPQualityExtMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefNPQualityExt newRef = (RefNPQualityExt) _newRef;
        quality = newRef.quality;
        avatar_score_unit_star = newRef.avatar_score_unit_star;
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
        return quality.ordinal();
    }

    public EQuality quality; //品质枚举
    public int avatar_score_unit_star; //服装评分中品质对应的星级

}
