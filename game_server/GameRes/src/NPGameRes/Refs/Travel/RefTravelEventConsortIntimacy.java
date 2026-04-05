package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_consort_intimacy")
public class RefTravelEventConsortIntimacy extends RefBase
{
    private static RefTravelEventConsortIntimacyMgr _g_mgr = new RefTravelEventConsortIntimacyMgr();
    public static RefTravelEventConsortIntimacyMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventConsortIntimacyMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventConsortIntimacyMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventConsortIntimacy newRef = (RefTravelEventConsortIntimacy) _newRef;
        event_id = newRef.event_id;
        consort_id = newRef.consort_id;
        add_intimacy = newRef.add_intimacy;
    }
    
    public static class RefTravelEventConsortIntimacyMgr extends RefTableContainer<RefTravelEventConsortIntimacy>
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
    public long consort_id;//妃子
    public int add_intimacy;//增加的亲密度
}
