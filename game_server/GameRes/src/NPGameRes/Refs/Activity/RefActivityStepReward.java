package NPGameRes.Refs.Activity;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "activity_step_reward")
public class RefActivityStepReward extends RefBase
{
    private static RefActivityStepRewardStepMgr _g_mgr = new RefActivityStepRewardStepMgr();

    public static RefActivityStepRewardStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityStepRewardStepMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityStepRewardStepMgr) _mgr;
    }

    public static class RefActivityStepRewardStepMgr extends RefTableContainer<RefActivityStepReward>
    {
        private Map<Long, List<RefActivityStepReward>> _m_hmActivityStepRewardListMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            //整理阶段奖励步骤数据
            Map<Long, List<RefActivityStepReward>> listMap = new HashMap<>();
            for (RefActivityStepReward ref : getList())
            {
                if (null == ref)
                    continue;

                listMap.computeIfAbsent(ref.step_reward_set_id, k -> new ArrayList<>()).add(ref);
            }

            //最后直接替换原来的map, 保证线程安全
            _m_hmActivityStepRewardListMap = listMap;
        }

        /**
         * 通过阶段奖励步骤查找对应的配置数据
         * @param _stepRewardId 阶段奖励id
         * @param _step         步骤
         * @return 配置数据
         */
        public RefActivityStepReward lookupByActivityStepRewardStep(long _stepRewardId, int _step)
        {
            List<RefActivityStepReward> stepRewardStepList = _m_hmActivityStepRewardListMap.get(_stepRewardId);
            if (stepRewardStepList == null)
                return null;

            for (RefActivityStepReward ref : stepRewardStepList)
            {
                if (ref.step == _step)
                    return ref;
            }

            return null;
        }

        /**
         * 获取可以领取的奖励列表
         * @param _stepRewardId  阶段奖励id
         * @param _completeCount 完成计数
         * @return 可以领取的奖励列表
         */
        public List<RefActivityStepReward> getCanDrawList(long _stepRewardId, long _completeCount)
        {
            List<RefActivityStepReward> refList = _m_hmActivityStepRewardListMap.get(_stepRewardId);
            if (null == refList)
                return null;

            List<RefActivityStepReward> canDrawList = new ArrayList<>();
            for (RefActivityStepReward ref : refList)
            {
                if (ref.complete_count <= _completeCount)
                    canDrawList.add(ref);
            }

            return canDrawList;
        }

        /**
         * 获取阶段奖励列表
         * @return
         */
        public List<RefActivityStepReward> getStepRewardList(long _stepRewardId)
        {
            return _m_hmActivityStepRewardListMap.get(_stepRewardId);
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityStepReward newRef = (RefActivityStepReward) _newRef;
        id = newRef.id;
        step_reward_set_id = newRef.step_reward_set_id;
        step = newRef.step;
        complete_count = newRef.complete_count;
        reward_item_list = newRef.reward_item_list;
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long step_reward_set_id; //阶段奖励ID
    public int step; //阶段奖励步骤
    public long complete_count; //进度条计数目标值
    public ArrayList<NPCommonCostItem> reward_item_list = new ArrayList<>(); //完成阶段奖励物品列表

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
