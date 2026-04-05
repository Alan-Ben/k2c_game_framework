using Common.RechargeRebateObj;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 充值返利信息数据
    /// </summary>
    public class RechargeRebateInfo
    {
        //组ID
        private long _m_lGroupId;
        //活动实例id
        private long _m_lActivityInstanceId;
        //充值返利组配置
        private RechargeRebateGroupRefObj _m_groupRefObj;
        //计数 VIP经验或充值天数
        private long _m_lCount;
        //已领取档位ID列表
        private List<long> _m_lHadDrawStepList;

        /// <summary>
        /// 当前计数
        /// </summary>
        public long curCount { get { return _m_lCount; } }
        /// <summary>
        /// 组id
        /// </summary>
        public long groupId { get { return _m_lGroupId; } }
        /// <summary>
        /// 活动实例id
        /// </summary>
        public long activityInstanceId { get { return _m_lActivityInstanceId; } }
        /// <summary>
        /// 充值返利组配置
        /// </summary>
        public RechargeRebateGroupRefObj groupRefObj { get { return _m_groupRefObj; } }

        public RechargeRebateInfo(long _activityInstanceId, RechargeRebate_GroupInfo _groupInfo)
        {
            updateInfo(_activityInstanceId, _groupInfo);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_groupInfo"></param>
        public void updateInfo(long _activityInstanceId, RechargeRebate_GroupInfo _groupInfo)
        {
            if (_groupInfo == null)
                return;

            _m_lActivityInstanceId = _activityInstanceId;
            _m_lGroupId = _groupInfo.getGroupId();
            _m_lCount = _groupInfo.getCount();
            _m_lHadDrawStepList = _groupInfo.getHadDrawStepList();
            _m_groupRefObj = GRefdataCoreMgr.instance.rechargeRebateGroupRefCore.getRef(_m_lGroupId);
        }

        /// <summary>
        /// 更新已领取档位ID列表
        /// </summary>
        /// <param name="_stepId"></param>
        public void addHadDrawStepList(long _stepId)
        {
            if(_m_lHadDrawStepList == null)
                _m_lHadDrawStepList = new List<long>();

            if(!_m_lHadDrawStepList.Contains(_stepId))
                _m_lHadDrawStepList.Add(_stepId);
        }

        /// <summary>
        /// 更新计数 VIP经验或充值天数
        /// </summary>
        public void updateCount(long _count)
        {
            _m_lCount = _count;
        }

        /// <summary>
        /// 是否已领取该档位奖励
        /// </summary>
        /// <param name="_stepId"></param>
        /// <returns></returns>
        public bool hadDrawStep(long _stepId)
        {
            if (_m_lHadDrawStepList == null)
                return false;

            return _m_lHadDrawStepList.Contains(_stepId);
        }

        /// <summary>
        /// 获取档位奖励状态
        /// </summary>
        /// <param name="_stepRef"></param>
        /// <returns></returns>
        public ECommonRewardType getStepRewardType(RechargeRebateStepRefObj _stepRef)
        {
            if (_stepRef == null)
                return ECommonRewardType.NONE;
            if (hadDrawStep(_stepRef.step))
                return ECommonRewardType.HAS_GET_REWARD;
            else if (curCount >= _stepRef.target_count)
                return ECommonRewardType.CAN_GET_REWARD;
            else
                return ECommonRewardType.NOT_GET_REWARD;
        }

        /// <summary>
        /// 获取排序后的档位列表
        /// </summary>
        /// <returns></returns>
        public List<RechargeRebateStepRefObj> getStepListBySort()
        {
            List<RechargeRebateStepRefObj> stepRefList = GRefdataCoreMgr.instance.getRechargeRebateStepRefList(_m_lGroupId);
            if (stepRefList == null)
                return null;

            //排序，可领取>未达到>已领取，一样按id从小到大
            stepRefList.Sort((_a, _b) =>
            {
                ECommonRewardType rewardTypeA = getStepRewardType(_a);
                ECommonRewardType rewardTypeB = getStepRewardType(_b);
                bool canGetA = rewardTypeA == ECommonRewardType.CAN_GET_REWARD;
                bool canGetB = rewardTypeB == ECommonRewardType.CAN_GET_REWARD;
                bool notGetA = rewardTypeA == ECommonRewardType.NOT_GET_REWARD;
                bool notGetB = rewardTypeB == ECommonRewardType.NOT_GET_REWARD;

                if (canGetA != canGetB)
                    return -(canGetA.CompareTo(canGetB));

                if (notGetA != notGetB)
                    return -(notGetA.CompareTo(notGetB));

                //按步骤从小到大
                return _a.step.CompareTo(_b.step);
            });
            return stepRefList;
        }

        /// <summary>
        /// 是否有档位可以领取奖励
        /// </summary>
        /// <returns></returns>
        public bool haveStepCanGetReward()
        {
            List<RechargeRebateStepRefObj> stepRefList = GRefdataCoreMgr.instance.getRechargeRebateStepRefList(_m_lGroupId);
            if(stepRefList == null)
                return false;

            foreach (RechargeRebateStepRefObj stepRef in stepRefList)
            {
                if (stepRef == null)
                    continue;

                if (getStepRewardType(stepRef) == ECommonRewardType.CAN_GET_REWARD)
                    return true;
            }
            return false;
        }
    }
}