package NPGameRes.Refs.Equip;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.List;

@RefTable(tableName = "equip_awaken")
public class RefEquipAwaken extends RefBase
{
    private static RefEquipAwakenMgr _g_mgr = new RefEquipAwakenMgr();

    public static RefEquipAwakenMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return _g_mgr;
    }

    public static class RefEquipAwakenMgr extends RefListContainer<RefEquipAwaken>
    {

    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEquipAwakenMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEquipAwaken newRef = (RefEquipAwaken) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        level = newRef.level;
        cost_item = newRef.cost_item;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public long group_id;
    public int level;//数量上限
    public List<NPCommonCostItem> cost_item;//等级上限
}
