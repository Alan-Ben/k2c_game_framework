package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_once")
public class RefTravelEventOnce extends RefBase
{
    private static RefTravelEventOnceMgr _g_mgr = new RefTravelEventOnceMgr();
    public static RefTravelEventOnceMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventOnceMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventOnceMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventOnce newRef = (RefTravelEventOnce) _newRef;
        event_id = newRef.event_id;
        is_trigger = newRef.is_trigger;
    }
    
    public static class RefTravelEventOnceMgr extends RefTableContainer<RefTravelEventOnce>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return event_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long event_id;//事件id
    public boolean is_trigger;//满足条件是否必定触发
}
