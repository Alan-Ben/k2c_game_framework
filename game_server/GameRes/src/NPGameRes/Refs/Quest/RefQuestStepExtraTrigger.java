package NPGameRes.Refs.Quest;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;

/**
 * @author mark 通用任务额外触发
 */
@RefTable(tableName = "quest_step_extra_trigger")
public class RefQuestStepExtraTrigger extends RefBase
{
    private static RefQuestStepExtraTriggerMgr _g_mgr = new RefQuestStepExtraTriggerMgr();

    public static RefQuestStepExtraTriggerMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefQuestStepExtraTriggerMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefQuestStepExtraTriggerMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefQuestStepExtraTrigger newRef = (RefQuestStepExtraTrigger) _newRef;
        step_id = newRef.step_id;
        trigger_event_list = newRef.trigger_event_list;
        trigger_condition = newRef.trigger_condition;
        ext_trigger_effect = newRef.ext_trigger_effect;
    }

    public static class RefQuestStepExtraTriggerMgr extends RefTableContainer<RefQuestStepExtraTrigger>
    {
        @Override
        public void _onTableLoaded()
        {
        }
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
        return step_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long step_id; //步骤 id
    public ArrayList<String> trigger_event_list; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public NPPlayerEffectListParse ext_trigger_effect; //触发时额外触发的效果

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
