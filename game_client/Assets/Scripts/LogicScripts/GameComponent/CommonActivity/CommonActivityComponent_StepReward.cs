using GC2GS.p017_ActivityOp;
using GS2GC.p017_ActivityOp;
using System;
using System.Collections.Generic;

namespace GOE
{
    public partial class CommonActivityComponent
    {
        /// <summary>
        /// 获取阶段奖励数据列表
        /// </summary>
        /// <returns></returns>
        public void getActivityStepRewardInfoList(List<ActivityStepRewardInfo> _list, Func<ActivityStepRewardInfo,bool> _checkFunc = null)
        {
            if (_list == null || _m_lCommonActivityInfoList == null)
                return;

            _list.Clear();

            for (int i = 0; i < _m_lCommonActivityInfoList.Count; i++)
            {
                if (_m_lCommonActivityInfoList[i] == null)
                    continue;

                List<ActivityStepRewardInfo> stepRewardInfoList = _m_lCommonActivityInfoList[i].stepRewardInfoList;
                if (stepRewardInfoList != null && stepRewardInfoList.Count > 0)
                {
                    for (int j = 0; j < stepRewardInfoList.Count; j++)
                    {
                        ActivityStepRewardInfo tempInfo = stepRewardInfoList[j];
                        if (_checkFunc == null || _checkFunc(tempInfo))
                            _list.Add(tempInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 获取阶段奖励信息
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_stepRewardSetId"></param>
        /// <returns></returns>
        public ActivityStepRewardInfo getActivityStepInfo(long _activityInstanceId, long _stepRewardSetId)
        {
            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_activityInstanceId);
            if(activityInfo == null)
                return null;
            
            return activityInfo.getStepRewardInfo(_stepRewardSetId);
        }

        /// <summary>
        /// 刷新阶段奖励奖励红点提示
        /// </summary>
        public void refreshStepRewardRedTip()
        {
            if (!isInitDone)
                return;

            //========刷新阶段奖励页签内的红点========
            List<ActivityStepRewardInfo> stepRewardInfoList = new List<ActivityStepRewardInfo>();
            getActivityStepRewardInfoList(stepRewardInfoList, (_info) =>
            {
                return _info != null && _info.isShowInStepRewardWnd;
            });
            long redTipCount = 0;
            //遍历冲榜活动数据列表，计算红点数量
            for (int i = 0; i < stepRewardInfoList.Count; i++)
            {
                ActivityStepRewardInfo stepRewardInfo = stepRewardInfoList[i];
                if (stepRewardInfo == null || stepRewardInfo.activityInstanceId <= 0)
                    continue;

                redTipCount += stepRewardInfo.getCanGetRewardCount();
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_STEP_REWARD_PAGE, redTipCount);


            //========刷新阶段奖励独立界面的红点========
            GRefdataCoreMgr.instance.activityMainRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.step_reward_red_tip_id > 0)
                {
                    redTipCount = 0;
                    _ABaseActivityInfo activityInfo = getValidActivityInfoByActivityId(_ref.activity_id);
                    if (activityInfo != null && activityInfo.stepRewardInfoList != null)
                    {
                        for (int j = 0; j < activityInfo.stepRewardInfoList.Count; j++)
                        {
                            ActivityStepRewardInfo stepRewardInfo = activityInfo.stepRewardInfoList[j];
                            if (stepRewardInfo == null || stepRewardInfo.isShowInStepRewardWnd)
                                continue;

                            redTipCount += stepRewardInfo.getCanGetRewardCount();
                        }
                    }
                    RedTipMgr.instance.setCountByRefRedTipId(_ref.step_reward_red_tip_id, redTipCount);
                }
            });
        }

        #region S2C

        public void onActivityStepRewardScoreChg(GS2GC_017_057_OnActivityStepRewardScoreChg _msg)
        {
            if (_msg == null)
                return;

            ActivityStepRewardInfo stepRewardInfo = getActivityStepInfo(_msg.getInstanceId(), _msg.getStepRewardId());
            stepRewardInfo?.updateScore(_msg.getScore());
            
            refreshStepRewardRedTip();
            WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG);
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 活动阶段事件任务奖励分数变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityStepRewardEventTaskScoreChg(GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg _msg)
        {
            if (_msg == null)
                return;

            // 获取阶段奖励信息
            ActivityStepRewardInfo stepRewardInfo = getActivityStepInfo(_msg.getActivityInstanceId(), _msg.getStepRewardId());
            if (stepRewardInfo == null)
                return;

            // 更新事件任务分数
            stepRewardInfo.updateEventTaskScore(_msg.getEventTaskId(), _msg.getScore());

            // 刷新红点
            refreshStepRewardRedTip();
            WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG);
            GCommon.reloadCustomLoadPrefab();
        }

        #endregion
       
        #region C2S

        /// <summary>
        /// 请求领取活动阶段奖励
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_stepRewardId"></param>
        /// <param name="_stepId"></param>
        /// <param name="_callback"></param>
        public void reqDrawActivityStepReward(long _instanceId, long _stepRewardId, int _stepId, Action<bool, GS2GC_017_011_RetDrawActivityStepReward> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_011_ReqDrawActivityStepReward(_instanceId, _stepRewardId, _stepId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_017_011_RetDrawActivityStepReward>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        ActivityStepRewardInfo stepRewardInfo = getActivityStepInfo(_instanceId, _stepRewardId);
                        stepRewardInfo?.setStepHasGet(_stepId);

                        refreshStepRewardRedTip();
                        WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_GET);
                        GCommon.reloadCustomLoadPrefab();
                    }
                    _callback?.Invoke(_isSuc, _msg);
                }));
        }

        /// <summary>
        /// 一键领取活动阶段奖励
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_stepRewardId"></param>
        /// <param name="_callback"></param>
        public void reqAKeyDrawActivityStepReward(long _instanceId, long _stepRewardId,
            Action<bool, GS2GC_017_013_RetAKeyDrawActivityStepReward> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(
                new GC2GS_017_013_ReqAKeyDrawActivityStepReward(_instanceId, _stepRewardId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_017_013_RetAKeyDrawActivityStepReward>((_isSuc, _msg) =>
                    {
                        if (_isSuc)
                        {
                            ActivityStepRewardInfo stepRewardInfo = getActivityStepInfo(_instanceId, _stepRewardId);
                            stepRewardInfo?.setAllCanGetStepHasGet();

                            refreshStepRewardRedTip();
                            WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_ONE_KEY_GET);
                            GCommon.reloadCustomLoadPrefab();
                        }

                        _callback?.Invoke(_isSuc, _msg);
                    }));
        }
        
        // /// <summary>
        // /// 请求活动阶段信息
        // /// </summary>
        // /// <param name="_instanceId"></param>
        // /// <param name="_stepRewardId"></param>
        // /// <param name="_callback"></param>
        // public void reqActivityStepRewardInfo(long _instanceId, long _stepRewardId, Action<GS2GC_017_012_RetActivityStepRewardInfo> _callback = null)
        // {
        //     NPGSClientListener.sendRequestByLog(new GC2GS_017_012_ReqActivityStepRewardInfo(_instanceId, _stepRewardId),
        //         new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_012_RetActivityStepRewardInfo>(_msg => _callback?.Invoke(_msg)));
        // }
        
        #endregion
    }
}