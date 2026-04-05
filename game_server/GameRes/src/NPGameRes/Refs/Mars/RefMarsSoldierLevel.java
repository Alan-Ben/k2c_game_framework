package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_soldier_level")
public class RefMarsSoldierLevel extends RefBase
{
    private static RefMarsSoldierLevelMgr _g_mgr = new RefMarsSoldierLevelMgr();

    public static RefMarsSoldierLevelMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsSoldierLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsSoldierLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsSoldierLevel newRef = (RefMarsSoldierLevel) _newRef;
        soldier_level = newRef.soldier_level;
    }

    @Override
    public long Id()
    {
        return soldier_level;
    }

    public static class RefMarsSoldierLevelMgr extends RefTableContainer<RefMarsSoldierLevel>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public int soldier_level;//士兵等级
}
