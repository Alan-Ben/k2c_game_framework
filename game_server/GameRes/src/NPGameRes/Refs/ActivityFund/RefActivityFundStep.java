package NPGameRes.Refs.ActivityFund;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.*;

/**
 * 活动基金阶段配置表
 */
@RefTable(tableName = "activity_fund_step")
public class RefActivityFundStep extends RefBase
{
    private static RefActivityFundStepMgr _g_mgr = new RefActivityFundStepMgr();

    public static RefActivityFundStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityFundStepMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityFundStepMgr) _mgr;
    }

    public static class RefActivityFundStepMgr extends RefTableContainer<RefActivityFundStep>
    {
        // 按基金ID分组的阶段列表
        private Map<Long, List<RefActivityFundStep>> _m_stepMapByFundId = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            // 按基金ID分组
            Map<Long, List<RefActivityFundStep>> tempFundMap = new HashMap<>();

            for (RefActivityFundStep ref : getList())
            {
                if (null == ref)
                    continue;

                // 按基金ID分组
                tempFundMap.computeIfAbsent(ref.activity_fund_id, k -> new ArrayList<>()).add(ref);
            }

            // 按照step排序
            tempFundMap.forEach((key, value) -> value.sort(Comparator.comparingLong(o -> o.step)));

            _m_stepMapByFundId = tempFundMap;
        }

        /**
         * 根据基金ID和阶段号查找配置
         *
         * @param _fundId 基金ID
         * @param _step   阶段号
         * @return 阶段配置
         */
        public RefActivityFundStep getStepByFundIdAndStep(long _fundId, int _step)
        {
            List<RefActivityFundStep> stepList = _m_stepMapByFundId.get(_fundId);
            if (null == stepList)
                return null;

            for (RefActivityFundStep ref : stepList)
            {
                if (ref.step == _step)
                    return ref;
            }

            return null;
        }

        /**
         * 计算当前阶段
         *
         * @param _fundId     基金ID
         * @param _totalScore 总分数
         * @return 当前阶段
         */
        public int calculateCurrentStep(long _fundId, long _totalScore)
        {
            int step = 0;

            List<RefActivityFundStep> stepList = _m_stepMapByFundId.get(_fundId);
            for (RefActivityFundStep ref : stepList)
            {
                if (ref.need_count > _totalScore)
                    break;

                step = ref.step;
            }

            return step;
        }

        /**
         * 获取区间内的阶段列表
         *
         * @param _fundId    基金ID
         * @param _startStep 开始阶段
         * @param _endStep   结束阶段
         * @return 阶段列表
         */
        public List<RefActivityFundStep> getStepListByRange(long _fundId, int _startStep, int _endStep)
        {
            if (_endStep < _startStep)
                return null;

            List<RefActivityFundStep> stepList = _m_stepMapByFundId.get(_fundId);
            if (null == stepList)
                return null;

            List<RefActivityFundStep> resultList = new ArrayList<>();
            for (RefActivityFundStep ref : stepList)
            {
                if (ref.step > _endStep)
                    break;

                if (ref.step < _startStep)
                    continue;

                resultList.add(ref);
            }

            return resultList;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityFundStep newRef = (RefActivityFundStep) _newRef;
        id = newRef.id;
        activity_fund_id = newRef.activity_fund_id;
        step = newRef.step;
        need_count = newRef.need_count;
        free_reward_item_list = newRef.free_reward_item_list;
        pay_reward_item_list = newRef.pay_reward_item_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 唯一ID
    public long id;

    // 基金ID
    public long activity_fund_id;

    // 阶段
    public int step;

    // 领取所需计数（分数）
    public long need_count;

    // 免费档奖励
    public ArrayList<NPCommonCostItem> free_reward_item_list = new ArrayList<>();

    // 付费档奖励
    public ArrayList<NPCommonCostItem> pay_reward_item_list = new ArrayList<>();
}
