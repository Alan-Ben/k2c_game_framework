using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基金信息快照（UI专用，页面打开期间不更新等级）
    /// </summary>
    public class FundInfoSnapshot
    {
        //活动实例ID
        private long _m_lActivityInstanceId;
        //基金ID
        private long _m_lFundId;
        //公式分数
        private long _m_lFormulaScore;
        //任务分数
        private long _m_lTaskScore;
        //物品分数
        private long _m_lItemScore;
        //已领取免费档阶段
        private int _m_iHadDrawFreeStep;
        //已领取付费档阶段
        private int _m_iHadDrawPayStep;

        //基金配置
        private ActivityFundRefObj _m_fundRefObj;
        //当前等级配置（快照，不会自动更新）
        private ActivityFundLevelRefObj _m_currentLevelRef;


        public FundInfoSnapshot([NotNull] FundInfo _source)
        {
            _m_lActivityInstanceId = _source.activityInstanceId;
            _m_lFundId = _source.fundId;
            _m_lFormulaScore = _source.formulaScore;
            _m_lTaskScore = _source.taskScore;
            _m_lItemScore = _source.totalScore - _source.formulaScore - _source.taskScore;
            _m_iHadDrawFreeStep = _source.hadDrawFreeStep;
            _m_iHadDrawPayStep = _source.hadDrawPayStep;
            _m_fundRefObj = _source.fundRef;
            _m_currentLevelRef = _source.levelRef;
        }


        /// <summary>
        /// 活动实例ID
        /// </summary>
        public long activityInstanceId { get { return _m_lActivityInstanceId; } }
        /// <summary>
        /// 基金ID
        /// </summary>
        public long fundId { get { return _m_lFundId; } }
        /// <summary>
        /// 基金配置
        /// </summary>
        public ActivityFundRefObj fundRef { get { return _m_fundRefObj; } }
        /// <summary>
        /// 当前等级配置
        /// </summary>
        public ActivityFundLevelRefObj levelRef { get { return _m_currentLevelRef; } }
        /// <summary>
        /// 公式分数
        /// </summary>
        public long formulaScore { get { return _m_lFormulaScore; } }
        /// <summary>
        /// 任务分数
        /// </summary>
        public long taskScore { get { return _m_lTaskScore; } }
        /// <summary>
        /// 总得分
        /// </summary>
        public long totalScore { get { return _m_lFormulaScore + _m_lTaskScore + _m_lItemScore; } }
        /// <summary>
        /// 已领取免费档阶段
        /// </summary>
        public int hadDrawFreeStep { get { return _m_iHadDrawFreeStep; } }
        /// <summary>
        /// 已领取付费档阶段
        /// </summary>
        public int hadDrawPayStep { get { return _m_iHadDrawPayStep; } }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool isUnlock { get { return _m_fundRefObj != null && GCommon.isSimpleUnlock(_m_fundRefObj.fund_simple_unlock_id); } }


        /// <summary>
        /// 从数据源更新（不更新等级）
        /// </summary>
        /// <returns>是否升级了（数据层等级变化）</returns>
        public void updateFromSource([NotNull] FundInfo _source)
        {
            _m_lFormulaScore = _source.formulaScore;
            _m_lTaskScore = _source.taskScore;
            _m_lItemScore = _source.totalScore - _source.formulaScore - _source.taskScore;
            _m_iHadDrawFreeStep = _source.hadDrawFreeStep;
            _m_iHadDrawPayStep = _source.hadDrawPayStep;
            //不更新 _m_currentLevelRef，保持快照的等级不变
        }
        /// <summary>
        /// 获取当前到达的阶段
        /// </summary>
        public int getCurrentStep()
        {
            List<ActivityFundStepRefObj> stepRefList = getStepRefList();
            if (stepRefList.Count == 0)
                return 0;

            long score = totalScore;
            int currentStep = 0;

            for (int i = 0; i < stepRefList.Count; i++)
            {
                if (score >= stepRefList[i].need_count)
                    currentStep = (int)stepRefList[i].step;
                else
                    break;
            }

            return currentStep;
        }
        /// <summary>
        /// 检查免费档是否有可领取奖励
        /// </summary>
        public bool checkCanDrawFreeReward()
        {
            int currentStep = getCurrentStep();
            return currentStep > _m_iHadDrawFreeStep;
        }
        /// <summary>
        /// 检查付费档是否有可领取奖励
        /// </summary>
        public bool checkCanDrawPayReward()
        {
            if (!checkHasBuyFund())
                return false;

            int currentStep = getCurrentStep();
            return currentStep > _m_iHadDrawPayStep;
        }
        /// <summary>
        /// 检查是否有任意可领取奖励
        /// </summary>
        public bool checkHasAnyRewardCanDraw()
        {
            return checkCanDrawFreeReward() || checkCanDrawPayReward();
        }
        /// <summary>
        /// 检查是否购买了基金
        /// </summary>
        public bool checkHasBuyFund()
        {
            if (_m_currentLevelRef == null || _m_currentLevelRef.distinguish_item == null)
                return false;

            return GCommon.isItemEnough(_m_currentLevelRef.distinguish_item.itemType, _m_currentLevelRef.distinguish_item.itemId, 1, false);
        }
        /// <summary>
        /// 获取阶段列表（当前等级的）
        /// </summary>
        [ItemNotNull, NotNull]
        public List<ActivityFundStepRefObj> getStepRefList()
        {
            if (_m_currentLevelRef is { step_ref_list: not null })
                return _m_currentLevelRef.step_ref_list;

            return new List<ActivityFundStepRefObj>(0);
        }
        /// <summary>
        /// 获取所有等级列表
        /// </summary>
        [ItemNotNull, NotNull]
        public List<ActivityFundLevelRefObj> getLevelRefList()
        {
            if (_m_fundRefObj is { level_ref_list: not null })
                return _m_fundRefObj.level_ref_list;

            return new List<ActivityFundLevelRefObj>(0);
        }
        /// <summary>
        /// 获取已领取的最小阶段
        /// </summary>
        public int getMinDrawnStep()
        {
            return _m_iHadDrawFreeStep < _m_iHadDrawPayStep ? _m_iHadDrawFreeStep : _m_iHadDrawPayStep;
        }
        /// <summary>
        /// 获取当前等级
        /// </summary>
        public long getCurrentLevel()
        {
            return _m_currentLevelRef?.level ?? 0;
        }
    }
}
