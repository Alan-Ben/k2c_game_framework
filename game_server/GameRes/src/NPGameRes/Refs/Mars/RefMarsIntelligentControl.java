package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

@RefTable(tableName = "mars_intelligent_control")
public class RefMarsIntelligentControl extends RefBase
{
    private static RefMarsIntelligentControlMgr _g_mgr = new RefMarsIntelligentControlMgr();

    public static RefMarsIntelligentControlMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsIntelligentControlMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsIntelligentControlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsIntelligentControl newRef = (RefMarsIntelligentControl) _newRef;
        id = newRef.id;
        cooling_time = newRef.cooling_time;
        cost_satisfaction_value = newRef.cost_satisfaction_value;
        unlock_cond = newRef.unlock_cond;
        use_cond = newRef.use_cond;
        deal_effect = newRef.deal_effect;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsIntelligentControlMgr extends RefTableContainer<RefMarsIntelligentControl>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;
    public int cooling_time;//冷却时间（秒）
    public int cost_satisfaction_value;//消耗满意值
    public NPPlayerConditionGroupObj unlock_cond;//解锁条件
    public NPPlayerConditionGroupObj use_cond;//使用条件
    public NPPlayerEffectListParse deal_effect;//处理效果
}