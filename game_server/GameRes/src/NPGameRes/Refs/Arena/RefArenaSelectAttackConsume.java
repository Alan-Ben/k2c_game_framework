package NPGameRes.Refs.Arena;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "arena_select_attack_consume")
public class RefArenaSelectAttackConsume extends RefBase
{
    private static RefTableContainer<RefArenaSelectAttackConsume> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefArenaSelectAttackConsume> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaSelectAttackConsume newRef = (RefArenaSelectAttackConsume) _newRef;
        id = newRef.id;
        cost = newRef.cost;
        ratio = newRef.ratio;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    @SuppressWarnings("unchecked")
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefArenaSelectAttackConsume>) _mgr;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public NPCommonCostItem cost;//消耗
    public int ratio;//获得影响力和商会硬币和奖励倍数
}
