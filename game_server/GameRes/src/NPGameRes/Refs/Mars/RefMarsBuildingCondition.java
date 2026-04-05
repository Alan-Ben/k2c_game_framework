package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "mars_building_condition")
public class RefMarsBuildingCondition extends RefBase
{
    private static RefMarsBuildingConditionMgr _g_mgr = new RefMarsBuildingConditionMgr();

    public static RefMarsBuildingConditionMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsBuildingConditionMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsBuildingConditionMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsBuildingCondition newRef = (RefMarsBuildingCondition) _newRef;
        id = newRef.id;
        condition = newRef.condition;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsBuildingConditionMgr extends RefTableContainer<RefMarsBuildingCondition>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//唯一 id
    public NPPlayerConditionGroupObj condition;//条件列表
}