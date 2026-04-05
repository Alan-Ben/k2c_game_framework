using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using GC2GS.p017_ActivityOp;
using GS2GC.p017_ActivityOp;
using JetBrains.Annotations;

namespace GOE
{
    public partial class CommonActivityComponent
    {
        //冲榜初始分数记录的信息
        [NotNull] private ActivityRankRushRemarkInfo _m_rankRushRemarkInfo;

        /// <summary>
        /// 获取冲榜数据列表
        /// </summary>
        /// <param name="_list">冲榜列表</param>
        /// <param name="_checkFunc">是否是展示在冲榜列表界面</param>
        public void getActivityRankRushInfoList(List<ActivityRankRushInfo> _list, Func<ActivityRankRushInfo, bool> _checkFunc = null)
        {
            if (_list == null || _m_lCommonActivityInfoList == null)
                return;

            _list.Clear();

            for (int i = 0; i < _m_lCommonActivityInfoList.Count; i++)
            {
                if(_m_lCommonActivityInfoList[i] == null)
                    continue;

                List<ActivityRankRushInfo> rankRushInfoList = _m_lCommonActivityInfoList[i].rankRushInfoList;
                if (rankRushInfoList != null && rankRushInfoList.Count > 0)
                {
                    for (int j = 0; j < rankRushInfoList.Count; j++)
                    {
                        ActivityRankRushInfo tempInfo = rankRushInfoList[j];
                        if (_checkFunc == null || _checkFunc(tempInfo))
                            _list.Add(tempInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 是否有冲榜活动正在进行中、结算中或者领奖中
        /// </summary>
        /// <returns></returns>
        public bool haveRankRushActivity()
        {
            List<long> rankRushActivityId = GRefdataCoreMgr.instance.getRankRushActivityIdList();
            if (rankRushActivityId == null)
                return false;

            foreach (long activityId in rankRushActivityId)
            {
                _ABaseActivityInfo activityInfo = getValidActivityInfoByActivityId(activityId);
                if(activityInfo == null || activityInfo.rankRushInfoList == null)
                    continue;

                bool showInRankRushWnd = false;
                for (int j = 0; j < activityInfo.rankRushInfoList.Count; j++)
                {
                    if (activityInfo.rankRushInfoList[j] != null &&
                        activityInfo.rankRushInfoList[j].activityRankRushRefObj != null && 
                        activityInfo.rankRushInfoList[j].activityRankRushRefObj.is_show_in_rank_rush_wnd)
                    {
                        showInRankRushWnd = true;
                        break;
                    }
                }

                //不展示在冲榜界面，则跳过
                if (!showInRankRushWnd)
                    continue;

                if (activityInfo.isEnable)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取记录的冲榜初始值
        /// </summary>
        /// <returns></returns>
        public long getRankRushRecordInitialValue(ActivityRankRushInfo _rankrushInfo, long _curScore)
        {
            if (_rankrushInfo == null || _rankrushInfo.activityInfo == null || _rankrushInfo.activityRankRushRefObj == null)
                return 0;

            long recordValue = _m_rankRushRemarkInfo.getRecordInitialValue(_rankrushInfo.activityInfo.instanceId, _rankrushInfo.activityRankRushRefObj.rank_id);
            if (recordValue != -1)
                return recordValue;

            if (_rankrushInfo.activityRankRushRefObj.haveMaxRecordValue && _rankrushInfo.activityRankRushRefObj.max_record_value != null)
            {
                long maxRecordValue = _rankrushInfo.activityRankRushRefObj.max_record_value.CalculateVariableResult(null);
                long initialValue = maxRecordValue - _curScore;
                //记录初始值
                _m_rankRushRemarkInfo.setRecordInitialValue(_rankrushInfo.activityInfo.instanceId, _rankrushInfo.activityRankRushRefObj.rank_id, initialValue);
                return initialValue;
            }

            return 0;
        }

        /// <summary>
        /// 初始检查冲榜排名变动
        /// </summary>
        public void initCheckRankingIsChange()
        {
            if (_m_rankRushRemarkInfo == null)
                return;

            _m_rankRushRemarkInfo.regDelegate(_m_rankRushRemarkInfo.checkRankingIsChange);
        }

        /// <summary>
        /// 刷新冲榜奖励红点提示
        /// </summary>
        public void refreshRankRushRewardRedTip()
        {
            if (!isInitDone)
                return;

            //先重置活动冲榜奖励红点
            GRefdataCoreMgr.instance.activityRankRushRefCore.dealAllRef(_ref =>
            {
                if(_ref != null && _ref.reward_red_tip_id > 0)
                    RedTipMgr.instance.setCountByRefRedTipId(_ref.reward_red_tip_id, 0);
            });

            //获取冲榜活动数据列表
            List<ActivityRankRushInfo> rankRushInfoList = new List<ActivityRankRushInfo>();
            getActivityRankRushInfoList(rankRushInfoList);
            if (rankRushInfoList.Count == 0)
            {
                //没有冲榜活动，重置冲榜页签红点
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RANK_RUSH_PAGE, 0);
                return;
            }

            //计算冲榜页签红点
            long redTipCount = 0;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(rankRushInfoList.Count);
            stepCounter.regAllDoneDelegate(() =>
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_RANK_RUSH_PAGE, redTipCount);
            });

            //遍历冲榜活动数据列表，计算红点数量
            for (int i = 0; i < rankRushInfoList.Count; i++)
            {
                ActivityRankRushInfo rankRushInfo = rankRushInfoList[i];
                if (rankRushInfo == null || rankRushInfo.activityInfo == null || rankRushInfo.activityRankRushRefObj == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }

                //是否在冲榜列表界面展示
                bool isShowInRankRushWnd = rankRushInfo.isShowInRankRushWnd;

                //如果活动是新活动，设置红点
                if (rankRushInfo.isNew && isShowInRankRushWnd)
                    redTipCount++;

                long rewardRedTipId = rankRushInfo.activityRankRushRefObj.reward_red_tip_id;
                //先重置奖励红点
                if (rewardRedTipId > 0)
                    RedTipMgr.instance.setCountByRefRedTipId(rewardRedTipId, 0);
                //如果正在领奖期，请求领奖信息刷新红点
                if (rankRushInfo.activityInfo.activityState == EActivityState.REWARDING)
                {
                    rankRushInfo.getSettleInfo(_settleInfo =>
                    {
                        //如果自己未领取奖励并且有排名，说明可领取奖励
                        if (_settleInfo != null && !_settleInfo.getHadDraw() && _settleInfo.getRank() > 0)
                        {
                            if(isShowInRankRushWnd)
                                redTipCount++;

                            //如果有奖励红点id，则设置红点数量
                            if (rewardRedTipId > 0)
                                RedTipMgr.instance.setCountByRefRedTipId(rewardRedTipId, 1);
                        }
                        else
                        {
                            //如果有奖励红点id，则设置红点数量
                            if (rewardRedTipId > 0)
                                RedTipMgr.instance.setCountByRefRedTipId(rewardRedTipId, 0);
                        }

                        stepCounter.addDoneStepCount();
                    }, rankRushInfo.isGuildRankRush);
                }
                else
                    stepCounter.addDoneStepCount();
            }
        }

        #region S2C

        /// <summary>
        /// 冲榜排名变化推送
        /// </summary>
        public void onActivityRankScoreChg(GS2GC_017_053_OnActivityRankScoreChg _msg)
        {
            if (_msg == null || !isInitDone)
                return;

            //等待记录数据初始化完成
            _m_rankRushRemarkInfo.regDelegate(() =>
            {
                //记录当前排名
                _m_rankRushRemarkInfo.setRecordRanking(_msg.getInstanceId(), _msg.getRankId(), _msg.getCurRank());

                _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
                if (activityInfo == null)
                    return;

                ActivityRankRushRefObj rankRushRef = GRefdataCoreMgr.instance.getRankRushRefByActivityIdRankId(activityInfo.activityId, _msg.getRankId());
                if (rankRushRef == null)
                    return;

                //添加tip
                ActivityRankRushChangeTipMgr.instance.addRankingChangeTipInfo(_msg.getInstanceId(), rankRushRef.id, _msg.getOriRank(), _msg.getCurRank());
            });
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求领取活动排行榜奖励
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_callback"></param>
        public void reqDrawActivityRankReward(long _instanceId, long _rankId, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_001_ReqDrawActivityRankReward(_instanceId, _rankId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_017_001_RetDrawActivityRankReward>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                    WinMsg.SendMsg(WinMsgType.ON_RANK_RUSH_GET_REWARD, _instanceId, _rankId);
                }));
        }

        /// <summary>
        /// 请求活动排行榜自己的结算信息
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_callback"></param>
        public void reqActivityRankSettleInfo(long _instanceId, long _rankId, Action<bool, GS2GC_017_002_RetActivityRankSettleInfo> _callback = null, bool _showErrorCode = false)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_002_ReqActivityRankSettleInfo(_instanceId, _rankId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_017_002_RetActivityRankSettleInfo>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc, _msg);
                    WinMsg.SendMsg(WinMsgType.ON_RANK_RUSH_GET_SETTLE_INFO, _instanceId, _rankId);
                }, null, _showErrorCode));
        }

        /// <summary>
        /// 请求活动排行榜基础信息
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_isCross"></param>
        /// <param name="_callback"></param>
        public void reqActivityRankBaseList(long _instanceId, long _rankId, bool _isCross, Action<GS2GC_017_003_RetActivityRankBaseList> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_003_ReqActivityRankBaseList(_instanceId, _rankId, _isCross),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_003_RetActivityRankBaseList>(_msg => _callback?.Invoke(_msg)));
        }

        /// <summary>
        /// 请求活动排行榜基础信息通过排名
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_rank"></param>
        /// <param name="_isCross"></param>
        /// <param name="_callback"></param>
        public void reqActivityRankBaseInfoByRank(long _instanceId, long _rankId, int _rank, bool _isCross, Action<GS2GC_017_004_RetActivityRankBaseInfoByRank> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_004_ReqActivityRankBaseInfoByRank(_instanceId, _rankId, _rank, _isCross),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_004_RetActivityRankBaseInfoByRank>(_msg => _callback?.Invoke(_msg)));
        }

        /// <summary>
        /// 请求活动排行榜基础信息通过主体id
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_key"></param>
        /// <param name="_isCross"></param>
        /// <param name="_callback"></param>
        public void reqActivityRankBaseInfoByKey(long _instanceId, long _rankId, long _key, bool _isCross, Action<GS2GC_017_005_RetActivityRankBaseInfoByKey> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_005_ReqActivityRankBaseInfoByKey(_instanceId, _rankId, _key, _isCross),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_005_RetActivityRankBaseInfoByKey>(_msg => _callback?.Invoke(_msg)));
        }

        /// <summary>
        /// 请求排行榜子数据列表
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_rankId"></param>
        /// <param name="_key"></param>
        /// <param name="_isCross"></param>
        /// <param name="_callback"></param>
        public void reqActivityRankBaseSubInfoList(long _instanceId, long _rankId, long _key, bool _isCross, Action<GS2GC_017_007_RetActivityRankBaseSubInfoList> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_007_ReqActivityRankBaseSubInfoList(_instanceId, _rankId, _key, _isCross),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_007_RetActivityRankBaseSubInfoList>(_msg => _callback?.Invoke(_msg)));
        }

        #endregion

    }
}