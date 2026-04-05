package NPGameRes.Refs.Activity;

import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "activity_shop")
public class RefActivityShop extends RefBase
{
    private static RefActivityShopMgr _g_mgr = new RefActivityShopMgr();

    public static RefActivityShopMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityShopMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityShopMgr) _mgr;
    }

    public static class RefActivityShopMgr extends RefTableContainer<RefActivityShop>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityShop newRef = (RefActivityShop) _newRef;
        id = newRef.id;
        refresh_clock = newRef.refresh_clock;
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
    public NPRefreshTimeObj refresh_clock = new NPRefreshTimeObj();//刷新规则(ENPTimeRefreshType) 会重置上限次数、付费timepiece
}