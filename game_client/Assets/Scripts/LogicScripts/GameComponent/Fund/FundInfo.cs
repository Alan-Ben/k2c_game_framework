using System.Collections.Generic;
using Common.ActivityFundObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基金信息
    /// </summary>
    public class FundInfo
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
        //当前等级配置缓存
        private ActivityFundLevelRefObj _m_currentLevelRef;
        

        public FundInfo(ActivityFund_Info _serverFundInfo)
        {
            updateInfo(_serverFundInfo);
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
        /// 总得分 (公式分数 + 任务分数)
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
        public bool isUnlock { get { return _m_fundRefObj != null && GCommon.isSimpleUnlock(_m_fundRefObj.fund_simple_unlock_id); } }
        

        /// <summary>
        /// 更新信息
        /// </summary>
        public void updateInfo(ActivityFund_Info _serverFundInfo)
        {
            if (_serverFundInfo == null)
                return;

            _m_lActivityInstanceId = _serverFundInfo.getActivityInstanceId();
            _m_lFundId = _serverFundInfo.getFundId();
            _m_lFormulaScore = _serverFundInfo.getFormulaScore();
            _m_lTaskScore = _serverFundInfo.getTaskScore();
            _m_iHadDrawFreeStep = _serverFundInfo.getHadDrawFreeStep();
            _m_iHadDrawPayStep = _serverFundInfo.getHadDrawPayStep();

            _m_fundRefObj = GRefdataCoreMgr.instance.activityFundRefCore.getRef(_m_lFundId);
            _refreshCurrentLevel();
        }
        /// <summary>
        /// 更新分数
        /// </summary>
        public void updateScore(long _formulaScore, long _taskScore)
        {
            _m_lFormulaScore = _formulaScore;
            _m_lTaskScore = _taskScore;
        }
        /// <summary>
        /// 更新领取状态
        /// </summary>
        public void updateDrawReward(int _hadDrawFreeStep, int _hadDrawPayStep)
        {
            _m_iHadDrawFreeStep = _hadDrawFreeStep;
            _m_iHadDrawPayStep = _hadDrawPayStep;
            _refreshCurrentLevel();
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
        /// 检查是否购买了基金（当前等级的高级战令）
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
        
        
        internal bool _refreshItemScore()
        {
            long newItemScore = 0;
            if (fundRef?.level_ref_list != null)
            {
                foreach (ActivityFundLevelRefObj refObj in fundRef.level_ref_list)
                {
                    if (refObj?.distinguish_item == null)
                        continue;

                    if (GCommon.isItemEnough(refObj.distinguish_item.itemType, refObj.distinguish_item.itemId, 1, false))
                        newItemScore += refObj.activate_exp_count;
                }
            }
            
            if (newItemScore != _m_lItemScore)
            {
                _m_lItemScore = newItemScore;
                return true;
            }
            
            return false;
        }
        
        
        //刷新当前等级缓存
        //逻辑：用最小领取step的下一个step所属的等级作为当前等级
        //这样如果玩家没有购买高级战令，会一直停留在该等级，直到购买后才能升级
        private void _refreshCurrentLevel()
        {
            _m_currentLevelRef = null;

            if (_m_fundRefObj == null)
                return;

            List<ActivityFundLevelRefObj> levelRefList = getLevelRefList();
            if (levelRefList.Count == 0)
                return;

            int minDrawnStep = getMinDrawnStep();

            //找到下一个step所属的等级
            //下一个step = minDrawnStep + 1，找到第一个 last_step >= 下一个step 的等级即可
            long nextStep = minDrawnStep + 1;
            foreach (ActivityFundLevelRefObj levelRef in levelRefList)
            {
                // 如果 last_step <= 0 则表示该等级没有上限，直接返回该等级
                if (levelRef.last_step <= 0 || levelRef.last_step >= nextStep)
                {
                    _m_currentLevelRef = levelRef;
                    return;
                }
            }

            //如果所有step都已领取，返回最后一个等级（满级）
            if (levelRefList.Count > 0)
                _m_currentLevelRef = levelRefList[^1];
        }
    }
}