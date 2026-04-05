package NPGameRes.Refs.Travel;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_change")
public class RefTravelEventChange extends RefBase
{
    private static RefTravelEventChangeMgr _g_mgr = new RefTravelEventChangeMgr();
    public static RefTravelEventChangeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventChangeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventChangeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventChange newRef = (RefTravelEventChange) _newRef;
        event_id = newRef.event_id;
        event_cost = newRef.event_cost;
        exchange_item_list = newRef.exchange_item_list;
    }
    
    public static class RefTravelEventChangeMgr extends RefTableContainer<RefTravelEventChange>
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
    public NPCommonCostItem event_cost;//满足条件是否必定触发
    public ArrayList<NPCommonCostItem> exchange_item_list = new ArrayList<>();//兑换的物品列表
}
