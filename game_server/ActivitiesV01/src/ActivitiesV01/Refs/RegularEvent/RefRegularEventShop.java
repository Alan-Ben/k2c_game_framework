package ActivitiesV01.Refs.RegularEvent;

import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "regular_event_shop")
public class RefRegularEventShop extends RefBase
{
    private static RefRegularEventShopMgr _g_mgr = new RefRegularEventShopMgr();

    public static RefRegularEventShopMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefRegularEventShopMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefRegularEventShopMgr) _mgr;
    }

    public static class RefRegularEventShopMgr extends RefTableContainer<RefRegularEventShop>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRegularEventShop newRef = (RefRegularEventShop) _newRef;
        activity_id = newRef.activity_id;
        refresh_clock = newRef.refresh_clock;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return activity_id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long activity_id;
    public NPRefreshTimeObj refresh_clock = new NPRefreshTimeObj();//刷新规则(ENPTimeRefreshType) 会重置上限次数、付费timepiece
}