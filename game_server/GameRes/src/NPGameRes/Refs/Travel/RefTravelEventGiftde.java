package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_giftde")
public class RefTravelEventGiftde extends RefBase
{
    private static RefTravelEventGiftdeMgr _g_mgr = new RefTravelEventGiftdeMgr();
    public static RefTravelEventGiftdeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventGiftdeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventGiftdeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventGiftde newRef = (RefTravelEventGiftde) _newRef;
        event_id = newRef.event_id;
    }
    
    public static class RefTravelEventGiftdeMgr extends RefTableContainer<RefTravelEventGiftde>
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
