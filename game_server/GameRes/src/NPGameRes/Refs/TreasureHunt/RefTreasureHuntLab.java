package NPGameRes.Refs.TreasureHunt;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * id	解锁条件
 * id	unlock_condition
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_lab")
public class RefTreasureHuntLab extends RefBase
{
    private static RefTreasureHuntLabMgr _g_mgr = new RefTreasureHuntLabMgr();

    public static RefTreasureHuntLabMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntLabMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntLabMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntLab newRef = (RefTreasureHuntLab) _newRef;
        id = newRef.id;
        unlock_condition = newRef.unlock_condition;
    }

    public static class RefTreasureHuntLabMgr extends RefTableContainer<RefTreasureHuntLab>
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

    public long id;//id
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件
}
