package NPGameRes.Refs.Common;

import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

@RefTable(tableName = "common_refresh")
public class RefCommonRefresh extends RefBase
{
    private static RefTableContainer<RefCommonRefresh> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefCommonRefresh> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonRefresh> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonRefresh>) _mgr;
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonRefresh newRef = (RefCommonRefresh) _newRef;
        id = newRef.id;
        refresh_clock = newRef.refresh_clock;
        effect = newRef.effect;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//刷新id
    public NPRefreshTimeObj refresh_clock;//刷新时间
    public NPPlayerEffectListParse effect;//执行效果

}