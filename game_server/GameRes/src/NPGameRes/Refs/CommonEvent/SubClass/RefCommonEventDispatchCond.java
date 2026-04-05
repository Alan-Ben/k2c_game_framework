package NPGameRes.Refs.CommonEvent.SubClass;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.HeroCondition.HeroConditionGroupObj;

@RefTable(tableName = "common_event_dispatch_cond")
public class RefCommonEventDispatchCond extends RefBase
{
    private static RefTableContainer<RefCommonEventDispatchCond> _g_mgr = new RefTableContainer<RefCommonEventDispatchCond>();

    public static RefTableContainer<RefCommonEventDispatchCond> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEventDispatchCond> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEventDispatchCond>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEventDispatchCond newRef = (RefCommonEventDispatchCond) _newRef;
        id = newRef.id;
        condition = newRef.condition;
        num = newRef.num;
    }
    
    @Override
    public long Id()
    {
        return id;
    }

    public long id;//唯一id
    public HeroConditionGroupObj condition;//条件列表
    public int num;//需要大臣数量
}
