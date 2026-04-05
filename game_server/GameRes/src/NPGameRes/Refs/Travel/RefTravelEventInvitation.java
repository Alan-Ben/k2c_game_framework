package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_invitation")
public class RefTravelEventInvitation extends RefBase
{
    private static RefTravelEventInvitationMgr _g_mgr = new RefTravelEventInvitationMgr();
    public static RefTravelEventInvitationMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventInvitationMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventInvitationMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventInvitation newRef = (RefTravelEventInvitation) _newRef;
        event_id = newRef.event_id;
    }
    
    public static class RefTravelEventInvitationMgr extends RefTableContainer<RefTravelEventInvitation>
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
}
