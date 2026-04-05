using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 冲榜排名变更提示信息管理类
    /// </summary>
    public class ActivityRankRushChangeTipMgr
    {
        private static ActivityRankRushChangeTipMgr _g_instance;
        [NotNull] public static ActivityRankRushChangeTipMgr instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new ActivityRankRushChangeTipMgr();
                return _g_instance;
            }
        }

        //冲榜提升变化tip信息列表
        private List<ActivityRankRushChangeTipInfo> _m_lRankingChangeTipInfoList;
        //是否正在处理tip
        private bool _m_bIsDealingTip;

        public ActivityRankRushChangeTipMgr()
        {
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            _m_bIsDealingTip = false;
            _m_lRankingChangeTipInfoList?.Clear();
            _m_lRankingChangeTipInfoList = null;
        }

        /// <summary>
        /// 添加冲榜排名变更提示信息
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_rankRushId"></param>
        /// <param name="_lastRanking"></param>
        /// <param name="_curRanking"></param>
        public void addRankingChangeTipInfo(long _activityInstanceId, long _rankRushId, long _lastRanking, long _curRanking)
        {
            //如果排名没有变化，则不处理
            if (_lastRanking == _curRanking)
                return;

            //该冲榜是否需要展示排名变更tip
            ActivityRankRushRefObj rankRushRefObj = GRefdataCoreMgr.instance.activityRankRushRefCore.getRef(_rankRushId);
            if (rankRushRefObj == null || !rankRushRefObj.is_show_ranking_change_tip)
                return;

            //判断是否满足展示条件
            long simpleUnlockId = GRefdataCoreMgr.instance.npGeneral.rank_rush_ranking_chg_tip_show_simple_unlock_id;
            if (simpleUnlockId > 0 && !GCommon.isSimpleUnlock(simpleUnlockId))
                return;

            if (_m_lRankingChangeTipInfoList == null)
                _m_lRankingChangeTipInfoList = new List<ActivityRankRushChangeTipInfo>();

            //遍历是否有记录，有则更新，没有则新增
            bool isFind = false;
            for (int i = 0; i < _m_lRankingChangeTipInfoList.Count; i++)
            {
                if (_m_lRankingChangeTipInfoList[i] != null &&
                    _m_lRankingChangeTipInfoList[i].activityInstanceId == _activityInstanceId &&
                    _m_lRankingChangeTipInfoList[i].rankRushId == _rankRushId)
                {
                    //更新信息和触发时间
                    _m_lRankingChangeTipInfoList[i].updateInfo(_activityInstanceId, _rankRushId, _lastRanking, _curRanking);
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
            {
                ActivityRankRushChangeTipInfo tipInfo = new ActivityRankRushChangeTipInfo(_activityInstanceId, _rankRushId, _lastRanking, _curRanking);
                _m_lRankingChangeTipInfoList.Add(tipInfo);
            }

            //检查是否可以展示tip
            _checkCanShowTip();
        }

        /// <summary>
        /// 检查是否可以展示tip
        /// </summary>
        private void _checkCanShowTip()
        {
            if (_m_lRankingChangeTipInfoList == null || _m_lRankingChangeTipInfoList.Count == 0)
                return;

            //是否正在处理tip
            if (_m_bIsDealingTip)
                return;

            //查找触发时间最近的需要展示的tip信息
            ActivityRankRushChangeTipInfo nextTriggerInfo = null;
            for (int i = 0; i < _m_lRankingChangeTipInfoList.Count; i++)
            {
                if (_m_lRankingChangeTipInfoList[i] != null && (nextTriggerInfo == null || nextTriggerInfo.triggerTimeMs > _m_lRankingChangeTipInfoList[i].triggerTimeMs))
                    nextTriggerInfo = _m_lRankingChangeTipInfoList[i];
            }

            //没有找到下一个触发的tip信息
            if (nextTriggerInfo == null)
                return;

            //设置正在处理tip
            _m_bIsDealingTip = true;

            //判断触发时间是否到了
            if (nextTriggerInfo.triggerTimeMs <= FpsAndPingMgr.instance.serverTimeTag)
            {
                long rankRushId = nextTriggerInfo.rankRushId;
                long lastRanking = nextTriggerInfo.lastRanking;
                long curRanking = nextTriggerInfo.curRanking;

                //移除数据
                _m_lRankingChangeTipInfoList?.Remove(nextTriggerInfo);

                //判断排名是否有变化
                if (lastRanking != curRanking)
                {
                    //展示tip，tip展示完后，继续检查下一个tip
                    NPGUIAddSceneCenterTip.instance.showRankRushRankingChgTip(rankRushId, lastRanking, curRanking, () =>
                    {
                        _m_bIsDealingTip = false;
                        _checkCanShowTip();
                    });
                }
                else
                {
                    _m_bIsDealingTip = false;
                    _checkCanShowTip();
                }
            }
            else
            {
                //触发时间未到，延时处理
                float delayTimeSec = (nextTriggerInfo.triggerTimeMs - FpsAndPingMgr.instance.serverTimeTag) / 1000f;
                ALCommonTaskController.CommonActionAddMonoTask(()=>
                {
                    //触发时间到了，重新检查是否可以展示tip
                    _m_bIsDealingTip = false;
                    _checkCanShowTip();
                }, delayTimeSec);
            }
        }
    }
}
