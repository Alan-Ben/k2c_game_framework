package NPGameRes.Refs.Quest.DailyQuest;

import Common.QuestEnum.EDailyQuestType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


@RefTable(tableName = "daily_quest_active_reward")
public class RefDailyQuestActiveReward extends RefBase
{
    private static RefDailyQuestActiveRewardMgr _g_mgr = new RefDailyQuestActiveRewardMgr();

    public static RefDailyQuestActiveRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefDailyQuestActiveRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefDailyQuestActiveRewardMgr) _mgr;
    }

    public static class RefDailyQuestActiveRewardMgr extends RefTableContainer<RefDailyQuestActiveReward>
    {
        //对应任务类型的奖励列表的Map
        private HashMap<EDailyQuestType, List<RefDailyQuestActiveReward>> _m_hmRewardMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            HashMap<EDailyQuestType, List<RefDailyQuestActiveReward>> tempMap = new HashMap<>();

            for (RefDailyQuestActiveReward ref : getList())
            {
                if (null == ref)
                    continue;

                tempMap.computeIfAbsent(ref.daily_quest_type, k -> new ArrayList<>()).add(ref);
            }

            //替换map
            _m_hmRewardMap = tempMap;
        }

        /**
         * 根据任务类型获取对应的奖励列表
         * @param _type 任务类型
         * @return
         */
        public List<RefDailyQuestActiveReward> getRewardListByType(EDailyQuestType _type)
        {
            return _m_hmRewardMap.get(_type);
        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyQuestActiveReward newRef = (RefDailyQuestActiveReward) _newRef;
        id = newRef.id;
        daily_quest_type = newRef.daily_quest_type;
        draw_active_reward_need = newRef.draw_active_reward_need;
        reward_id = newRef.reward_id;
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
    public EDailyQuestType daily_quest_type;//日常任务类型枚举
    public NPCommonCostItem draw_active_reward_need;//领奖条件，需要达到的积分，达成条件即可领奖
    public long reward_id; //奖励


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
