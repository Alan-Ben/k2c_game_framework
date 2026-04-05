package NPGameRes.Refs.CrystalGiftPack;

import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "crystal_gift_pack_group")
public class RefCrystalGiftPackGroup extends RefBase
{
    private static RefCrystalGiftPackGroupMgr _g_mgr = new RefCrystalGiftPackGroupMgr();

    public static RefCrystalGiftPackGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefCrystalGiftPackGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefCrystalGiftPackGroupMgr) _mgr;
    }

    public static class RefCrystalGiftPackGroupMgr extends RefTableContainer<RefCrystalGiftPackGroup>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCrystalGiftPackGroup newRef = (RefCrystalGiftPackGroup) _newRef;
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