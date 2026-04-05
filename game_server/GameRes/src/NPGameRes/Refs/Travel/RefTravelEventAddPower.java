package NPGameRes.Refs.Travel;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "travel_event_add_power")
public class RefTravelEventAddPower extends RefBase
{
    private static RefTravelEventAddPowerMgr _g_mgr = new RefTravelEventAddPowerMgr();
    public static RefTravelEventAddPowerMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTravelEventAddPowerMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTravelEventAddPowerMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTravelEventAddPower newRef = (RefTravelEventAddPower) _newRef;
        event_id = newRef.event_id;
        add_power = newRef.add_power;
    }
    
    public static class RefTravelEventAddPowerMgr extends RefTableContainer<RefTravelEventAddPower>
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
    public int add_power;//增加的属性
}
