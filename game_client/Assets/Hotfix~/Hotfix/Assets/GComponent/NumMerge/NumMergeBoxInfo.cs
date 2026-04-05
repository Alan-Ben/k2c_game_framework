using Hotfix.Common.NumMergeObj;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏宝箱信息
    /// </summary>
    public class NumMergeBoxInfo
    {
        private int _m_curLevel; // 当前的宝箱等级
        private long _m_currentScore; // 当前剩余分数
        private NumMergeBoxRefObj _m_currentBoxRef; // 当前正在进行的宝箱配置（下一个要领取的）
        private NumMergeBoxRefObj _m_nextBoxRef; // 下一个宝箱配置


        public NumMergeBoxInfo()
        {
            _m_curLevel = 0;
            _m_currentScore = 0;
            _m_currentBoxRef = null;
            _m_nextBoxRef = null;
        }


        /// <summary>
        /// 当前的宝箱等级
        /// </summary>
        public int curLevel { get { return _m_curLevel; } }
        /// <summary>
        /// 当前剩余分数
        /// </summary>
        public long currentScore { get { return _m_currentScore; } }
        /// <summary>
        /// 当前正在进行的宝箱配置（下一个要领取的）
        /// </summary>
        [CanBeNull] public NumMergeBoxRefObj currentBoxRef { get { return _m_currentBoxRef; } }
        /// <summary>
        /// 下一个宝箱配置
        /// </summary>
        [CanBeNull] public NumMergeBoxRefObj nextBoxRef { get { return _m_nextBoxRef; } }


        /// <summary>
        /// 从服务器数据更新
        /// </summary>
        public void updateFromServer(NumMerge_BoxInfo _boxInfo)
        {
            if (_boxInfo == null)
                return;

            // 服务端的 level 会一直递增，我们直接使用
            _m_curLevel = _boxInfo.getLevel();
            _m_currentScore = _boxInfo.getScore();

            // 更新缓存的配置引用
            _updateBoxRefs();
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            _m_curLevel = 0;
            _m_currentScore = 0;
            _m_currentBoxRef = null;
            _m_nextBoxRef = null;
        }
        
        /// <summary>
        /// 获取还需要多少分数才能领取下一个宝箱
        /// </summary>
        /// <returns>还需要的分数，如果没有宝箱配置则返回 -1</returns>
        public long getScoreNeededForNext()
        {
            if (_m_currentBoxRef == null)
                return -1; // 没有宝箱配置

            long needed = _m_currentBoxRef.upgrade_need_score - _m_currentScore;
            return needed > 0 ? needed : 0;
        }
        /// <summary>
        /// 获取当前堆叠的可领取宝箱数量
        /// </summary>
        public int getClaimableCount()
        {
            if (_m_currentBoxRef == null)
                return 0;

            int count = 0;
            long tempScore = _m_currentScore;
            int maxLevel = HotfixRefdataCoreMgr.instance.getNumMergeBoxMaxLevel();

            // 先计算到达最大配置等级前能领取多少个（包含最大等级）
            int tempLevel = _m_curLevel;
            while (tempLevel <= maxLevel)
            {
                NumMergeBoxRefObj boxRef = HotfixRefdataCoreMgr.instance.numMergeBoxRefCore.getRef(tempLevel);
                if (boxRef == null)
                    break;

                if (tempScore >= boxRef.upgrade_need_score)
                {
                    count++;
                    tempScore -= boxRef.upgrade_need_score;
                    tempLevel++;
                }
                else
                {
                    break;
                }
            }

            // 如果已经超过最大等级，使用最大等级配置循环计算
            if (tempLevel > maxLevel)
            {
                NumMergeBoxRefObj maxBoxRef = HotfixRefdataCoreMgr.instance.numMergeBoxRefCore.getRef(maxLevel);
                if (maxBoxRef != null && maxBoxRef.upgrade_need_score > 0)
                {
                    // 直接计算还能领多少次最大等级的宝箱
                    int maxLevelCount = (int)(tempScore / maxBoxRef.upgrade_need_score);
                    count += maxLevelCount;
                }
            }

            return count;
        }
        /// <summary>
        /// 是否有可领取的宝箱
        /// </summary>
        public bool hasClaimableBox()
        {
            return getClaimableCount() > 0;
        }
        /// <summary>
        /// 获取当前进度百分比（0.0 - 1.0）
        /// </summary>
        public float getCurrentProgress()
        {
            if (_m_currentBoxRef == null)
                return 0f; // 没有宝箱配置

            if (_m_currentBoxRef.upgrade_need_score <= 0)
                return 0f;

            float progress = (float)_m_currentScore / _m_currentBoxRef.upgrade_need_score;
            return Mathf.Clamp01(progress);
        }
        
        
        /// <summary>
        /// 更新缓存的宝箱配置引用
        /// </summary>
        private void _updateBoxRefs()
        {
            int maxLevel = HotfixRefdataCoreMgr.instance.getNumMergeBoxMaxLevel();

            // 当前宝箱配置：当前要领取的等级，但不超过最大配置等级
            int currentStep = _m_curLevel;
            if (currentStep > maxLevel)
                currentStep = maxLevel;
            _m_currentBoxRef = HotfixRefdataCoreMgr.instance.numMergeBoxRefCore.getRef(currentStep);

            // 下一个宝箱配置：当前等级 + 1，但不超过最大配置等级
            int nextStep = _m_curLevel + 1;
            if (nextStep > maxLevel)
                nextStep = maxLevel;
            _m_nextBoxRef = HotfixRefdataCoreMgr.instance.numMergeBoxRefCore.getRef(nextStep);
        }
    }
}
