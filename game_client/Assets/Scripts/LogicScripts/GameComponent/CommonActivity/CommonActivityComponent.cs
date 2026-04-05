using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using Common;
using Common.ActivityEnum;
using Common.ActivityObj;
using CommonEnum;
using GC2GS.p017_ActivityOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p017_ActivityOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 活动组件
    /// </summary>
    public partial class CommonActivityComponent : _ANPBasicPlayerComponent
    {
        /**
         * _m_lCommonActivityInfoList代表原始活动数据列表，可能包含多个相同活动id不同实例id的活动数据，跟服务端同步
         * _m_validActivityInfoList 代表有效活动数据列表，每个活动id只保留一个最新的活动数据，方便UI等模块查询
         * 每次更新活动数据后都要调用_updateValidActivityList方法更新有效活动数据列表
         */
        //活动原始数据列表
        [NotNull]private List<_ABaseActivityInfo> _m_lCommonActivityInfoList = new List<_ABaseActivityInfo>();
        //有效的活动数据列表,每个活动id只保留一个最新的活动数据
        [NotNull]private List<_ABaseActivityInfo> _m_validActivityInfoList = new List<_ABaseActivityInfo>();
        
        //活动货币数据列表 <活动id，数量>
        private Dictionary<long, long> _m_lActivityCurrencyInfoDic;

        public CommonActivityComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_rankRushRemarkInfo = new ActivityRankRushRemarkInfo();
        }

        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.BASIC_INFO };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COMMON_ACTIVITY; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            reqActivityList();
            reqActivityCurrencyList();
        }

        protected override void _dealInit()
        {
            _m_rankRushRemarkInfo.sendRequest();
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.RegisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            foreach (_ABaseActivityInfo info in _m_lCommonActivityInfoList)
            {
                if (info != null) 
                    info.discard();
            }
            _m_lCommonActivityInfoList.Clear();

            _m_lActivityCurrencyInfoDic?.Clear();
            _m_lActivityCurrencyInfoDic = null;

            ActivityRankRushChangeTipMgr.instance.clear();
            WinMsg.UnregisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            WinMsg.UnregisterMsg(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();

            //处理冲榜奖励红点提示
            refreshRankRushRewardRedTip();
            //处理阶段奖励红点提示
            refreshStepRewardRedTip();
            //刷新钻石礼包商店红点
            refreshGemGiftPackRedTip();
            //刷新钻石礼包商店每日红点
            refreshGemGiftPackDialyRedTip();
        }

        /// <summary>
        /// 更新有效活动列表，目前客户端自己维护一份每个活动id一个活动数据的列表，方便UI等模块查询
        /// </summary>
        private void _updateValidActivityList()
        {
            if (_m_lCommonActivityInfoList.Count == 0)
            {
                _m_validActivityInfoList.Clear();
                return;
            }

            _m_validActivityInfoList.Clear();

            Dictionary<long, _ABaseActivityInfo> dic = new Dictionary<long, _ABaseActivityInfo>();
            foreach (var activityInfo in _m_lCommonActivityInfoList)
            {
                if (activityInfo == null)
                    continue;

                long activityId = activityInfo.activityId;

                if (!dic.TryAdd(activityId, activityInfo))
                {
                    _ABaseActivityInfo currentActivity = dic[activityId];
            
                    // 比较优先级，选择优先级更高的活动
                    if (_shouldReplaceActivity(activityInfo, currentActivity))
                    {
                        dic[activityId] = activityInfo;
                    }
                }
            }
            
            _m_validActivityInfoList.AddRange(dic.Values.ToList());
        }
        
        /// <summary>
        /// 判断是否应该替换当前活动（newActivity优先级是否高于currentActivity）
        /// </summary>
        private bool _shouldReplaceActivity(_ABaseActivityInfo newActivity, _ABaseActivityInfo currentActivity)
        {
            var newPriority = _getStatePriority(newActivity.activityState);
            var currentPriority = _getStatePriority(currentActivity.activityState);
    
            // 如果状态优先级不同，选择优先级高的
            if (newPriority != currentPriority)
                return newPriority < currentPriority;
    
            // 如果状态优先级相同，按开启时间排序（开启时间早的优先级高）
            return newActivity.startTimeMs < currentActivity.startTimeMs;
        }

        /// <summary>
        /// 获取活动状态优先级
        /// </summary>
        private int _getStatePriority(EActivityState state)
        {
            return state switch
            {
                EActivityState.PLAYING => 1,
                EActivityState.SETTLING => 2,
                EActivityState.REWARDING => 3,
                EActivityState.PLAN => 4,
                // 其他状态按开启时间排序，使用相同优先级
                _ => 5
            };
        }
        
        /// <summary>
        /// 新增或修改活动信息
        /// </summary>
        /// <param name="_serverActivityInfo"></param>
        private void _addOrUpdateActivityInfo(Activity_Info _serverActivityInfo)
        {
            if (_serverActivityInfo == null)
                return;
            
            _ABaseActivityInfo customActivityInfo = getActivityInfoByInstanceId(_serverActivityInfo.getInstanceId());//先通过活动id获取现有活动数据
                
            if (customActivityInfo == null)//若当前没有对应活动数据, 表示新增活动
            {
                GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_serverActivityInfo.getActivityId());
                if(null == activityMainRef)
                {
                    Debug.LogError($"新增活动id:{_serverActivityInfo.getActivityId()}的活动数据，但未找到对应的活动主表数据，活动数据异常，无法创建活动实例");
                    return;
                }
                
                customActivityInfo = CommonActivityFactory.instance.createInstance(activityMainRef.type_id, _serverActivityInfo);
                if(null == customActivityInfo)
                    return;
                customActivityInfo.subInit(null);
                _m_lCommonActivityInfoList.Add(customActivityInfo);
                
                _updateValidActivityList();
                
                WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, customActivityInfo.activityId, customActivityInfo.instanceId);
            }
            else//表示变更活动数据
            {
                customActivityInfo.updateActivityInfo(_serverActivityInfo);//变更活动数据

                _updateValidActivityList();

                WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, customActivityInfo.activityId, customActivityInfo.instanceId);
            }
        }
       
        /// <summary>
        /// 通过活动id获取有效的活动信息
        /// </summary>
        /// <param name="_activityId"></param>
        /// <returns></returns>
        public _ABaseActivityInfo getValidActivityInfoByActivityId(long _activityId)
        {
            _ABaseActivityInfo result = null;
            foreach (_ABaseActivityInfo commonActivityInfo in _m_validActivityInfoList)
            {
                if(null != commonActivityInfo && commonActivityInfo.activityId == _activityId)
                {
                    result = commonActivityInfo;
                    break;
                }
            }
            
            //TODO 这个容错代码应该可以删除
            if (result == null)
            {
                foreach (_ABaseActivityInfo activityInfo in _m_lCommonActivityInfoList)
                {
                    if (activityInfo != null && activityInfo.activityId == _activityId)
                    {
                        Debug.LogError($"找客户端检查，有效活动数据列表中未找到活动id:{_activityId}的数据，返回原始数据列表中的第一个数据");
                        return activityInfo;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 通过条件获取活动信息列表
        /// </summary>
        /// <param name="_action"></param>
        /// <returns></returns>
        public List<_ABaseActivityInfo> getValidActivityListByFunc(Func<_ABaseActivityInfo, bool> _action)
        {
            if(_action == null)
                return null;
            
            List<_ABaseActivityInfo> activityInfoList = new List<_ABaseActivityInfo>();
            foreach (_ABaseActivityInfo activityInfo in _m_validActivityInfoList)
            {
                if (activityInfo != null && _action(activityInfo))
                    activityInfoList.Add(activityInfo);
            }

            return activityInfoList;
        }
        
        /// <summary>
        /// 根据活动类型获取活动信息列表
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public _ABaseActivityInfo getValidActivityInfoByType(ECommonActivityType _type)
        {
            foreach (_ABaseActivityInfo activityInfo in _m_validActivityInfoList)
            {
                if(activityInfo == null)
                    continue;
                GActivityMainRefObj activityMainRefObj = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityInfo.activityId);
                if (activityMainRefObj != null && activityMainRefObj.type_id == _type)
                {
                    return activityInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据活动类型获取活动信息列表
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public List<_ABaseActivityInfo> getValidActivityListInfoByType(ECommonActivityType _type)
        {
            List<_ABaseActivityInfo> activityInfoList = new List<_ABaseActivityInfo>();
            foreach (_ABaseActivityInfo activityInfo in _m_validActivityInfoList)
            {
                if(activityInfo == null)
                    continue;
                GActivityMainRefObj activityMainRefObj = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityInfo.activityId);
                if (activityMainRefObj != null && activityMainRefObj.type_id == _type)
                {
                    activityInfoList.Add(activityInfo);
                }
            }

            return activityInfoList;
        }
        
        /// <summary>
        /// 通过活动实例id获取活动信息
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public _ABaseActivityInfo getActivityInfoByInstanceId(long _instanceId)
        {
            foreach (_ABaseActivityInfo activityInfo in _m_lCommonActivityInfoList)
            {
                if (activityInfo != null && activityInfo.instanceId == _instanceId)
                    return activityInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 通过队伍id获取活动信息
        ///  teamId（队伍实例ID = groupId * 10000000  + 队伍自增ID）
        /// </summary>
        public _ABaseActivityInfo getActivityInfoByTeamId(long _teamId)
        {
            long usGroupId = _teamId / 10000000;
            foreach (_ABaseActivityInfo activityInfo in _m_lCommonActivityInfoList)
            {
                if (activityInfo != null && activityInfo.usGroupId == usGroupId)
                    return activityInfo;
            }

            return null;
        }

        /// <summary>
        /// 获取活动货币数量
        /// </summary>
        /// <param name="_activityId"></param>
        public long getActivityCurrencyCount(long _activityId)
        {
            if (_m_lActivityCurrencyInfoDic == null)
                return 0;

            if (_m_lActivityCurrencyInfoDic.TryGetValue(_activityId, out long count))
                return count;

            return 0;
        }

        /// <summary>
        /// 加入联盟
        /// </summary>
        /// <param name="_objects"></param>
        private void _onJoinGuild(params object[] _objects)
        {
            //刷新冲榜奖励红点提示
            refreshRankRushRewardRedTip();
        }

        /// <summary>
        /// 离开联盟
        /// </summary>
        /// <param name="_objects"></param>
        private void _onLeaveGuild(params object[] _objects)
        {
            //刷新冲榜奖励红点提示
            refreshRankRushRewardRedTip();
        }

        /// <summary>
        /// 发生跨天
        /// </summary>
        private void _onCrossDay()
        {
            //刷新钻石礼包商店每日红点
            refreshGemGiftPackDialyRedTip();
        }

        #region S2C

        /// <summary>
        /// 活动数据初始化回包
        /// </summary>
        /// <param name="_msg"></param>
        public void retActivityList(GS2GC_002_018_RetActivityList _msg)
        {
            if (_msg == null)
            {
                Debug.LogError("[CommonActivityComponent retActivityList] 活动组件初始化回包为空");
                setInitDone();
                return;
            }
            
            foreach (_ABaseActivityInfo info in _m_lCommonActivityInfoList)
            {
                if (info != null) 
                    info.discard();
            }
            _m_lCommonActivityInfoList.Clear();
            
            //添加计步器默认1次，等活动数据处理完再调用一次，确保setInitDone在所有活动数据处理完后才调用
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setInitDone);
            
            List<Activity_Info> activityInfos = _msg.getActivityList();
            if (activityInfos != null)
            {
                stepCounter.chgTotalStepCount(activityInfos.Count);
                foreach (Activity_Info activityInfo in activityInfos)
                {
                    if(activityInfo == null)//活动数据不存在
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }
                    
                    GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(activityInfo.getActivityId());
                    if(null == activityMainRef)
                    {
                        Debug.LogError($"新增活动id:{activityInfo.getActivityId()}的活动数据，但未找到对应的活动主表数据，活动数据异常，无法创建活动实例");
                        stepCounter.addDoneStepCount();
                        continue;
                    }

                    _ABaseActivityInfo aBaseActivity = CommonActivityFactory.instance.createInstance(activityMainRef.type_id, activityInfo);
                    if (null == aBaseActivity)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }
                    aBaseActivity.subInit(stepCounter.addDoneStepCount);
                    _m_lCommonActivityInfoList.Add(aBaseActivity);
                }
            }
            
            //更新有效数据列表
            _updateValidActivityList();

            //初始化活动玩家信息
            if (_msg.getPlayerDataList() != null)
            {
                for (int i = 0; i < _msg.getPlayerDataList().Count; i++)
                {
                    Activity_PlayerData activityPlayerData = _msg.getPlayerDataList()[i];
                    if (activityPlayerData == null)
                        continue;
                    _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(activityPlayerData.getActivityInstanceId());
                    if (activityInfo != null)
                        activityInfo.upateActivityPlayerData(activityPlayerData);
                }
            }

            //初始化阶段奖励信息
            if (_msg.getStepRewardList() != null)
            {
                for (int i = 0; i < _msg.getStepRewardList().Count; i++)
                {
                    Activity_StepRewardInfo stepRewardInfo = _msg.getStepRewardList()[i];
                    if (stepRewardInfo == null)
                        continue;
                    _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(stepRewardInfo.getActivityInstanceId());
                    if (activityInfo != null)
                        activityInfo.updateStepRewardInfo(stepRewardInfo);
                }
            }

            //活动初始化完检查一次冲榜记录数据
            _m_rankRushRemarkInfo.regDelegate(_m_rankRushRemarkInfo.checkActivityIsRunnung);
            
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 初始化活动货币列表
        /// </summary>
        /// <param name="_msg"></param>
        public void retActivityCurrencyList(GS2GC_002_064_RetActivityCurrencyList _msg)
        {
            if (_msg == null)
                return;

            if (_m_lActivityCurrencyInfoDic == null)
                _m_lActivityCurrencyInfoDic = new Dictionary<long, long>();
            _m_lActivityCurrencyInfoDic.Clear();

            for (int i = 0; i < _msg.getCurrencyList().Count; i++)
            {
                Common_ActivityCurrencyInfo info = _msg.getCurrencyList()[i];
                if(info == null)
                    continue;

                _m_lActivityCurrencyInfoDic[info.getRefId()] = info.getCount();
            }
        }

        /// <summary>
        /// 新增活动推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityAdd(GS2GC_017_050_OnActivityAdd _msg)
        {
            if (_msg == null)
                return;

            _addOrUpdateActivityInfo(_msg.getActivity());
            GCommon.reloadCustomLoadPrefab();
        }
        
        /// <summary>
        /// 活动数据变化推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityChg(GS2GC_017_051_OnActivityChg _msg)
        {
            if (_msg == null)
                return;

            _addOrUpdateActivityInfo(_msg.getActivity());
            GCommon.reloadCustomLoadPrefab();
        }
        
        /// <summary>
        /// 移除活动推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityRemove(GS2GC_017_052_OnActivityRemove _msg)
        {
            if(_msg == null)
                return;

            _ABaseActivityInfo activityInfo = null;
            for (int i = _m_lCommonActivityInfoList.Count - 1; i >= 0; i--)
            {
                activityInfo = _m_lCommonActivityInfoList[i];
                if (activityInfo != null && activityInfo.instanceId == _msg.getInstanceId())
                {
                    activityInfo.discard();
                    _m_lCommonActivityInfoList.RemoveAt(i);
                    
                    //更新有效
                    _updateValidActivityList();
                    WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, activityInfo.activityId, activityInfo.instanceId);
                    return;
                }
            }
            
            GCommon.reloadCustomLoadPrefab();
        }

        /// <summary>
        /// 活动状态变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityStateChg(GS2GC_017_056_OnActivityStateChg _msg)
        {
            if (_msg == null)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
            if (activityInfo == null)
                return;

            bool oldIsEnable = activityInfo.isEnable;
            EActivityState oriState = activityInfo.activityState;
            activityInfo.updateState(_msg.getState());
            
            if (oriState != activityInfo.activityState)
            {
                switch (activityInfo.activityState)
                {
                    case EActivityState.PLAYING:
                        WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_START, activityInfo.activityId, activityInfo.instanceId);
                        break;
                    case EActivityState.SETTLING:
                        WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_SETTLING, activityInfo.activityId, activityInfo.instanceId);
                        break;
                    case EActivityState.REWARDING:
                        WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_END, activityInfo.activityId, activityInfo.instanceId);
                        break;
                    case EActivityState.CLOSED:
                        WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, activityInfo.activityId, activityInfo.instanceId);
                        break;
                }

                _updateValidActivityList();
                
                WinMsg.SendMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, activityInfo.activityId, activityInfo.instanceId, oriState, activityInfo.activityState);
                GCommon.reloadCustomLoadPrefab();
                refreshGemGiftPackRedTip();
                refreshGemGiftPackDialyRedTip();
            }
            
            //从未开启到已开启，通知热更配表刷新补丁
            if (!oldIsEnable && activityInfo.isEnable)
            {
                ALCommonTaskController.CommonActionAddMonoTask((() =>
                {
                    NPPlayer.instance.commonActivityHotRefComp.patchTableByActivityInstanceId(_msg.getInstanceId());
                }));
            }
        }

        /// <summary>
        /// 活动货币信息推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityCurrencyInfoChg(GS2GC_004_059_PushActivityCurrencyInfo _msg)
        {
            if (_msg == null || _msg.getCurrencyInfo() == null || _msg.getCurrencyInfo().getRefId() <= 0)
                return;

            if (_m_lActivityCurrencyInfoDic == null)
                _m_lActivityCurrencyInfoDic = new Dictionary<long, long>();

            _m_lActivityCurrencyInfoDic[_msg.getCurrencyInfo().getRefId()] = _msg.getCurrencyInfo().getCount();
            WinMsg.SendMsg(WinMsgType.ON_ACTIVITY_CURRENCY_CHG, _msg.getCurrencyInfo().getRefId());
        }


        #endregion

        #region C2S

        /// <summary>
        /// 请求活动数据列表(初始化数据)
        /// </summary>
        public void reqActivityList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_018_ReqActivityList());
        }

        /// <summary>
        /// 请求活动货币列表
        /// </summary>
        public void reqActivityCurrencyList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_064_ReqActivityCurrencyList());
        }

        #endregion
    }
}