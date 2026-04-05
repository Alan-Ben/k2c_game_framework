using System;
using System.Collections.Generic;
using Common.ActivityEnum;
using Common.EarningsGoalObj;
using CommonEnum;
using GC2GS.p033_SimpleActivityOp;
using GS2GC.p033_SimpleActivityOp;

namespace GOE
{
    public class EarningGoalComponent : _ANPBasicPlayerComponent
    {
        
        /// <summary>
        /// 奖励信息列表
        /// </summary>
        private List<Common.EarningsGoalObj.EarningsGoal_RewardInfo> rewardInfoList;
        /// <summary>
        /// 荣誉奖励信息列表
        /// </summary>
        private List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> honorRewardInfoList;
        /// <summary>
        /// 已领取奖励列表
        /// </summary>
        private List<long> hadDrawRewardList;
        /// <summary>
        /// 已领取荣誉奖励列表
        /// </summary>
        private List<long> hadDrawHonorRewardList;

        private long _m_curActivityInstanceId;
        
        public EarningGoalComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.EARNING_GOAL; } }
        public override ENPPlayerCompType[] dependCompList { get { return new ENPPlayerCompType[]{ ENPPlayerCompType.COMMON_ACTIVITY }; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
        }

        protected override void _dealInit()
        {
            //请求初始化
            _reqEarningGoalInit(setInitDone);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_ADD, _checkEarningGoalInfo);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_START, _checkEarningGoalInfo);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_END, _checkEarningGoalInfo);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _checkEarningGoalInfo);
            WinMsg.RegisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _checkEarningGoalInfo);
        }

        protected override void _onInitDone()
        {
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_curActivityInstanceId = 0;
            rewardInfoList = null;
            honorRewardInfoList = null;
            hadDrawRewardList = null;
            hadDrawHonorRewardList = null;
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_ADD, _checkEarningGoalInfo);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_START, _checkEarningGoalInfo);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_END, _checkEarningGoalInfo);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _checkEarningGoalInfo);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _checkEarningGoalInfo);
        }
        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqEarningGoalInit(Action _initDone = null)
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(ECommonActivityType.EARNINGS_GOAL);
            if (null != activityInfo && activityInfo.isEnable && activityInfo.instanceId > 0)
            {
                _m_curActivityInstanceId = activityInfo.instanceId;
                NPGSClientListener.sendRequestByLog(new GC2GS_033_001_ReqEarningsGoalInfo(_m_curActivityInstanceId),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_001_RetEarningsGoalInfo>((_isSuc, _msg) =>
                    {
                        rewardInfoList = _msg.getRewardInfoList();
                        honorRewardInfoList = _msg.getHonorRewardInfoList();
                        hadDrawRewardList = _msg.getHadDrawRewardList();
                        hadDrawHonorRewardList = _msg.getHadDrawHonorRewardList();
                        _refreshRedTip();
                        _initDone?.Invoke();
                        WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_STATE_CHG);
                    }, null, false));
            }
            else
            {
                _resetEarningGoalInfo();
                _initDone?.Invoke();
                WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_STATE_CHG);
            }
        }

        private void _resetEarningGoalInfo()
        {
            _m_curActivityInstanceId = 0;
            rewardInfoList = new List<Common.EarningsGoalObj.EarningsGoal_RewardInfo>();
            honorRewardInfoList = new List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo>();
            hadDrawRewardList = new List<long>();
            hadDrawHonorRewardList = new List<long>();
            _refreshRedTip();
        }

        /// <summary>
        /// 检查全民赚速荣耀活动状态，如果已关闭则检查是否有新的赚速目标活动，没有则重置
        /// </summary>
        private void _checkEarningGoalInfo()
        {
            if (_m_curActivityInstanceId > 0)
            {
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_curActivityInstanceId);
                if (activityInfo != null && activityInfo.isEnable) 
                    return;
            }
            
            _resetEarningGoalInfo();
            _reqEarningGoalInit();
        }

        /// <summary>
        /// 是否已经领取过全民赚速荣耀奖励
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool hasDrawHonorReward(long _id)
        {
            if (hadDrawHonorRewardList == null)
                return false;
            return hadDrawHonorRewardList.Contains(_id);
        }

        /// <summary>
        /// 荣耀赚速目标奖励达成玩家cid
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public long honorAchievePlayerCid(long _id, out long _timestamp)
        {         
            if (honorRewardInfoList != null)
                foreach (var rewardInfo in honorRewardInfoList)
                {
                    if (rewardInfo != null && rewardInfo.getRefId() == _id)
                    {
                        _timestamp = rewardInfo.getTimestamp();
                        return rewardInfo.getFirstReachCid();
                    }
                }
            _timestamp = 0;
            return 0;
        }
        
        /// <summary>
        /// 全民赚速目标奖励达成玩家cid
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public long firstAchievePlayerCid(long _id)
        {
            if (rewardInfoList != null)
                foreach (var rewardInfo in rewardInfoList)
                {
                    if (rewardInfo != null && rewardInfo.getRefId() == _id)
                        return rewardInfo.getFirstReachCid();
                }
            return 0;
        }
       
        /// <summary>
        /// 全民赚速目标奖励是否已领取
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool hasDrawReward(long _id)
        {
            if (hadDrawRewardList == null)
                return false;
            return hadDrawRewardList.Contains(_id);
        }

        /// <summary>
        /// 赚速目标活动是否开启
        /// </summary>
        /// <returns></returns>
        public bool isEarningGoalActivityOpen()
        { 
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_curActivityInstanceId);
    
            if (activityInfo != null && activityInfo.isEnable)
                return true;
            
            return false;
        }

        /// <summary>
        /// 获取当前赚速目标活动结束时间
        /// </summary>
        /// <returns></returns>
        public long getEarningGoalActivityEndTime()
        {
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getActivityInfoByInstanceId(_m_curActivityInstanceId);

            if (activityInfo != null) return activityInfo.endTimeMs;
            return 0;
        }
        
        /// <summary>
        /// 按照大臣战力排序，有战斗次数的排在无战斗次数的前面，使用前需要用refreshRoundInfo()刷新轮次信息
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int sortEarningGoalGlobalReward(EarningGoalRewardRefObj a, EarningGoalRewardRefObj b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            
            bool achieveA = firstAchievePlayerCid(a.id) > 0;
            bool achieveB = firstAchievePlayerCid(b.id) > 0;
            
            bool aHadDraw = achieveA && hasDrawReward(a.id);
            bool bHadDraw = achieveB && hasDrawReward(b.id);
            
            
            if (bHadDraw && !aHadDraw)
                return -1;
            
            if (aHadDraw && !bHadDraw)
                return 1;
            
            if (!achieveB && achieveA)
                return -1;
            
            if (!achieveA && achieveB)
                return 1;
            

            
            return a.earning_goal > b.earning_goal ? 1 : -1;
        }
        
        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            int globalRedCount = 0;

            if (hadDrawRewardList != null && rewardInfoList != null)
            {
                foreach (var rewardInfo in rewardInfoList)
                {
                    if (rewardInfo == null)
                        continue;
                    if (rewardInfo.getFirstReachCid() > 0)
                    {
                        if(!hadDrawRewardList.Contains(rewardInfo.getRefId()))
                            globalRedCount++;
                    }
                }
            }
            int honorRedCount = 0;

            if (hadDrawHonorRewardList != null && honorRewardInfoList != null)
            {
                foreach (var rewardInfo in honorRewardInfoList)
                {
                    if (rewardInfo == null)
                        continue;
                    if (rewardInfo.getFirstReachCid() > 0 && rewardInfo.getFirstReachCid() == NPPlayer.instance.playerInfo.CID)
                    {
                        if(!hadDrawHonorRewardList.Contains(rewardInfo.getRefId()))
                            honorRedCount++;
                    }
                }
            }

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EARNING_GOAL_GLOBAL, globalRedCount);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EARNING_GOAL_HONOR, honorRedCount);
        }
        
        #region 消息

      

        /// <summary>
        /// 请求领取赚速目标奖励
        /// </summary>
        /// <param name="_refId"></param>
        /// <param name="_callback"></param>
        public void reqEarningsGoalDrawReward(long _refId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_033_002_ReqEarningsGoalDrawReward( _refId, _m_curActivityInstanceId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_002_RetEarningsGoalDrawReward>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 请求领取赚速目标荣誉奖励
        /// </summary>\
        /// <param name="_refId"></param>
        /// <param name="_callback"></param>
        public void reqEarningsGoalDrawHonorReward(long _refId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_033_003_ReqEarningsGoalDrawHonorReward(_refId, _m_curActivityInstanceId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_033_003_RetEarningsGoalDrawHonorReward>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 赚速目标全民有人达成
        /// </summary>
        /// <param name="_msg"></param>
        public void onEarningsGoalRewardReach(GS2GC_033_101_OnEarningsGoalRewardReach _msg)
        {
            if(_msg == null || _msg.getActivityInstanceId() != _m_curActivityInstanceId)
                return;
            EarningsGoal_RewardInfo reachRewardInfo = _msg.getRewardInfo();

            if (rewardInfoList != null && reachRewardInfo != null)
            {
                bool hasReward = false;
                foreach (var rewardInfo in rewardInfoList)
                {
                    if (rewardInfo != null && rewardInfo.getRefId() == reachRewardInfo.getRefId())
                    {
                        rewardInfo.setFirstReachCid(reachRewardInfo.getFirstReachCid());
                        hasReward = true;
                        break;
                    }
                }
                if (!hasReward)
                {
                    rewardInfoList.Add(reachRewardInfo);
                }
            }
            
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_SELF_REACH_CHG);
        }
        
        /// <summary>
        /// 赚速目标荣誉奖励有人达成
        /// </summary>
        /// <param name="_msg"></param>
        public void onEarningsGoalHonorRewardReach(GS2GC_033_102_OnEarningsGoalHonorRewardReach _msg)
        {
            if(_msg == null || _msg.getActivityInstanceId() != _m_curActivityInstanceId)
                return;
            EarningsGoal_HonorRewardInfo reachRewardInfo = _msg.getHonorRewardInfo();

            if (honorRewardInfoList != null && reachRewardInfo != null)
            {
                bool hasReward = false;
                foreach (var rewardInfo in honorRewardInfoList)
                {
                    if (rewardInfo != null && rewardInfo.getRefId() == reachRewardInfo.getRefId())
                    {
                        rewardInfo.setFirstReachCid(reachRewardInfo.getFirstReachCid());
                        hasReward = true;
                        break;
                    }
                }
                if (!hasReward)
                {
                    honorRewardInfoList.Add(reachRewardInfo);
                }
            }
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_HONOR_REACH_CHG);
        }

        /// <summary>
        /// 赚速目标荣耀奖励领取状态更新
        /// </summary>
        /// <param name="_msg"></param>
        public void onEarningsGoalHonorRewardDraw(GS2GC_033_106_OnEarningsGoalHonorRewardDraw _msg)
        {
            if(_msg == null || _msg.getActivityInstanceId() != _m_curActivityInstanceId)
                return;
            if (hadDrawHonorRewardList != null) hadDrawHonorRewardList.Add(_msg.getRefId());
            
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_HONOR_DRAW_CHG);
        }
        
        /// <summary>
        /// 赚速目标全民奖励领取状态更新
        /// </summary>
        /// <param name="_msg"></param>
        public void onEarningsGoalRewardDraw(GS2GC_033_105_OnEarningsGoalRewardDraw _msg)
        {
            if(_msg == null || _msg.getActivityInstanceId() != _m_curActivityInstanceId)
                return;
            if (hadDrawRewardList != null) hadDrawRewardList.Add(_msg.getRefId());
            
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_EARNINGS_GOAL_SELF_DRAW_CHG);
        }


        #endregion
    }
}