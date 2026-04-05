package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_consort")
public class RefTravelConsort extends RefBase
{
    private static RefTravelConsortMgr _g_mgr = new RefTravelConsortMgr();
    public static RefTravelConsortMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelConsortMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelConsortMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelConsort newRef = (RefTravelConsort) _newRef;
        consort_id = newRef.consort_id;
        marry_need_like = newRef.marry_need_like;
    }
    
    public static class RefTravelConsortMgr extends RefTableContainer<RefTravelConsort>
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
        return consort_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long consort_id;//妃子id
    public int marry_need_like;//迎娶所需好感度
    
}
