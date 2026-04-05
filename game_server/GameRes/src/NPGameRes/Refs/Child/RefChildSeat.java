package NPGameRes.Refs.Child;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * @author mark
 */
@RefTable(tableName = "child_seat")
public class RefChildSeat extends RefBase
{
    private static RefChildSeatMgr _g_mgr = new RefChildSeatMgr();
    public static RefChildSeatMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChildSeatMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChildSeatMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChildSeat newRef = (RefChildSeat) _newRef;
        seat_id = newRef.seat_id;
        seat_unlock_condition = newRef.seat_unlock_condition;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return seat_id;
    }
    
    public static class RefChildSeatMgr extends RefTableContainer<RefChildSeat>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long seat_id;//子嗣相性id
    public NPPlayerConditionGroupObj seat_unlock_condition; //席位解锁条件
}
