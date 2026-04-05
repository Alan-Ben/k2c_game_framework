package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_consort_bar")
public class RefTravelEventConsortBar extends RefBase
{
    private static RefTravelEventConsortBarMgr _g_mgr = new RefTravelEventConsortBarMgr();
    public static RefTravelEventConsortBarMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventConsortBarMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventConsortBarMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventConsortBar newRef = (RefTravelEventConsortBar) _newRef;
        event_id = newRef.event_id;
        trigger_consort_id_list = newRef.trigger_consort_id_list;
    }
    
    public static class RefTravelEventConsortBarMgr extends RefTableContainer<RefTravelEventConsortBar>
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
    public ArrayList<Long> trigger_consort_id_list = new ArrayList<>();//触发的妃子id列表
}
