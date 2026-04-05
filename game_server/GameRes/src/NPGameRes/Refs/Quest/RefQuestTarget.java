package NPGameRes.Refs.Quest;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "quest_target")
public class RefQuestTarget extends RefBase
{
    private static RefQuestTargetMgr _g_mgr = new RefQuestTargetMgr();

    public static RefQuestTargetMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefQuestTargetMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefQuestTargetMgr) _mgr;
    }

    public static class RefQuestTargetMgr extends RefTableContainer<RefQuestTarget>
    {
        private HashMap<Long, ArrayList<RefQuestTarget>> _m_hmQuestStepTargetMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefQuestTarget newRef = (RefQuestTarget) _newRef;
        id = newRef.id;
        step_id = newRef.step_id;
        process_count = newRef.process_count;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        ext_trigger_effect = newRef.ext_trigger_effect;
        is_client_target = newRef.is_client_target;
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
        return id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //主键 id
    public long step_id; //步骤 id
    public long process_count; //进度条计数目标值
    public NPPlayerVariableGroupObj process_cur_count; //进度当前值（高级公式）(计数器的话可以不用配置)(进度条计数目标达到时就算完成)
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public ArrayList<String> trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public NPPlayerEffectListParse ext_trigger_effect; //触发时额外触发的效果

    public boolean is_client_target;//是否客户端判断的目标值，true的情况下服务器会通过客户端提交的消息增加进度

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
