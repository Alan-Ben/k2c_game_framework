package NPGameRes.Refs.Quest.SystemQuest;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENpRewardShowType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "system_quest")
public class RefSystemQuest extends RefBase
{
    private static RefSystemQuestMgr _g_mgr = new RefSystemQuestMgr();

    public static RefSystemQuestMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefSystemQuest> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefSystemQuestMgr) _mgr;
    }

    public static class RefSystemQuestMgr extends RefTableContainer<RefSystemQuest>
    {
        private Map<Long, List<RefSystemQuest>> _m_groupQuestMap = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            Map<Long, List<RefSystemQuest>> groupQuestMap = new HashMap<>();

            for (RefSystemQuest refSystemQuest : getList())
            {
                if (null == refSystemQuest)
                    continue;

                List<RefSystemQuest> questList = groupQuestMap.computeIfAbsent(refSystemQuest.group_id, k -> new ArrayList<>());
                questList.add(refSystemQuest);
            }

            _m_groupQuestMap = groupQuestMap;
        }

        /**
         * 获取任务组id列表
         * @return
         */
        public List<Long> getQuestGroupList()
        {
            return new ArrayList<>(_m_groupQuestMap.keySet());
        }

        /**
         * 获取任务配置
         * @return
         */
        public RefSystemQuest getQuestRef(long _groupId, int _step)
        {
            List<RefSystemQuest> refSystemQuestList = _m_groupQuestMap.get(_groupId);
            if (null == refSystemQuestList)
                return null;

            for (RefSystemQuest refSystemQuest : refSystemQuestList)
            {
                if (refSystemQuest.step == _step)
                    return refSystemQuest;
            }

            return null;
        }

        /**
         * 根据组ID获取任务列表
         * @param _groupId 任务组ID
         * @return 任务列表
         */
        public List<RefSystemQuest> getQuestListByGroupId(long _groupId)
        {
            return _m_groupQuestMap.get(_groupId);
        }


    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSystemQuest newRef = (RefSystemQuest) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        step = newRef.step;
        tip_reward = newRef.tip_reward;
        done_gain_item_list = newRef.done_gain_item_list;
        process_count = newRef.process_count;
        process_cur_count = newRef.process_cur_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        ext_trigger_effect = newRef.ext_trigger_effect;
        is_client_target = newRef.is_client_target;
        is_set = newRef.is_set;
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

    public long id;
    public long group_id;//任务组ID
    public int step;//阶段
    public ENpRewardShowType tip_reward;//奖励展示样式（ENpRewardShowType）
    public List<NPCommonCostItem> done_gain_item_list;//完成后奖励物品列表
    public long process_count;//进度条计数目标值
    public NPPlayerVariableGroupObj process_cur_count; //计数器的高级公式(ENPPlayerVariableType)
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public String trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public NPPlayerEffectListParse ext_trigger_effect; //触发时额外触发的效果
    public boolean is_client_target;//是否客户端判断的目标值，true的情况下服务器会通过客户端提交的消息增加进度
    public boolean is_set;//"是否设置值 默认false：使用变量，true-使用当前量"

}