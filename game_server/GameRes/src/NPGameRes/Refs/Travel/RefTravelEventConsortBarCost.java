package NPGameRes.Refs.Travel;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_consort_bar_cost")
public class RefTravelEventConsortBarCost extends RefBase
{
    private static RefTravelEventConsortBarCostMgr _g_mgr = new RefTravelEventConsortBarCostMgr();
    public static RefTravelEventConsortBarCostMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventConsortBarCostMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventConsortBarCostMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventConsortBarCost newRef = (RefTravelEventConsortBarCost) _newRef;
        id = newRef.id;
        cost = newRef.cost;
        add_like = newRef.add_like;
        add_intimacy = newRef.add_intimacy;
    }
    
    public static class RefTravelEventConsortBarCostMgr extends RefTableContainer<RefTravelEventConsortBarCost>
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
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public NPCommonCostItem cost;
    public int add_like;//增加的好感度
    public int add_intimacy;//增加的亲密度
}
