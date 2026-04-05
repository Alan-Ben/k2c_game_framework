using System;
using System.Collections.Generic;
using ALPackage;
using Common.MarsObj;
using GC2GS.p040_MarsPeopleOp;
using GS2GC.p002_InitOp;
using GS2GC.p040_MarsPeopleOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// Mars居民子组件
    /// </summary>
    public class MarsPeopleSubComponent : _AMarsSubComponent
    {
        private long _m_lIdlePeopleNum;//空闲居民数量
        // private int _m_lWorkingPeopleNum;//工作居民数量, 这边移除工作中居民数量, 直接通过建筑中派遣人数获取
        private long _m_lSickPeopleNum;//生病居民数量
        
        private int _m_iSatisfactionDregree;//满意度万分比
        
        private List<MarsIntelligentControlInfo> _m_lIntelligentControlInfoList = new List<MarsIntelligentControlInfo>();
        private List<MarsPeopleWillLetter> _m_lLetterList = new List<MarsPeopleWillLetter>();//信件数据列表
        private List<_IMarsPeopleWillHelp> _m_lHelpList = new List<_IMarsPeopleWillHelp>();//求助数据列表
        
        private List<MarsEventInfo> _m_lEventInfoList;//事件数据列表
        private List<Mars_Event> _m_lTriggerEventTipInfoList = new List<Mars_Event>();//触发事件Tip数据列表

        private Common.MarsObj.Mars_PeopleImmigrant _m_immigrantInfo;//移民数据
        private Common.MarsObj.Mars_PeopleImmigrantCount _m_todayImmigrantCount;//今日移民次数
        private MarsPeopleImmigrantCountResetLocalPushDealer _m_immigrantCountResetLocalPushDealer;//火星招募居民次数重置本地推送
        
        public MarsPeopleSubComponent([NotNull] MarsComponent _parentComp) 
            : base(_parentComp)
        {
        }

        /// <summary>
        /// 休息中居民数量
        /// </summary>
        public long idlePeopleNum { get { return _m_lIdlePeopleNum; } }

        /// <summary>
        /// 工作中居民数量
        /// </summary>
        public long workingPeopleNum { get { return _m_parentComponent.dispatchedPeopleNum; } }
        
        /// <summary>
        /// 生病中居民数量
        /// </summary>
        public long sickPeopleNum { get { return _m_lSickPeopleNum; } }
        /// <summary>
        /// 居民总人数
        /// </summary>
        public int satisfactionDregree { get { return _m_iSatisfactionDregree; } }
        public IReadOnlyList<MarsIntelligentControlInfo> intelligentControlInfoList { get { return _m_lIntelligentControlInfoList; } }
        public List<MarsPeopleWillLetter> letterList { get { return _m_lLetterList; } }
        public List<_IMarsPeopleWillHelp> helpList { get { return _m_lHelpList; } }
        public List<MarsEventInfo> eventInfoList { get { return _m_lEventInfoList; } }
        public List<Mars_Event> triggerEventTipInfoList { get { return _m_lTriggerEventTipInfoList; } }
        public int todayImmigrantCount { get { return _m_todayImmigrantCount?.getUsedCount() ?? 0; } }

        public override void init(Action<bool> _complete)
        {
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onPlayerBuffChg;
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);

            if(_m_lEventInfoList == null)
                _m_lEventInfoList = new List<MarsEventInfo>();
            _m_lEventInfoList.Clear();
            foreach (var marsEventRefObj in GRefdataCoreMgr.instance.marsEventRefCore.refList)
            {
                if(marsEventRefObj == null)
                    continue;
                
                _m_lEventInfoList.Add(new MarsEventInfo(marsEventRefObj));
            }
            
            reqMarsPeopleInit(((_isSucc, _msg) =>
                {
                    _m_parentComponent.dealPreInitFunc(() =>
                    {
                        if (_isSucc && _msg != null)
                        {
                            Common.MarsObj.Mars_PeopleNum serverPeopleNumInfo = _msg.getPeopleNum();
                            if (serverPeopleNumInfo != null)
                            {
                                _m_lIdlePeopleNum = serverPeopleNumInfo.getIdle();
                                // _m_lWorkingPeopleNum = serverPeopleNumInfo.getWorking();
                                _m_lSickPeopleNum = serverPeopleNumInfo.getSick();
                            }

                            _m_lIntelligentControlInfoList = new List<MarsIntelligentControlInfo>();
                            if (_msg.getIntelligentList() != null)
                            {
                                foreach (var serverIntelligentInfo in _msg.getIntelligentList())
                                {
                                    if(serverIntelligentInfo != null)
                                        _m_lIntelligentControlInfoList.Add(new MarsIntelligentControlInfo(serverIntelligentInfo));
                                }
                            }

                            _m_lLetterList = new List<MarsPeopleWillLetter>();
                            if (_msg.getLetterList() != null)
                            {
                                foreach (var serverLetterInfo in _msg.getLetterList())
                                {
                                    if(serverLetterInfo != null)
                                        _m_lLetterList.Add(new MarsPeopleWillLetter(serverLetterInfo));
                                }
                            }
                            
                            _m_lHelpList = new List<_IMarsPeopleWillHelp>();
                            if (_msg.getHelpList() != null)
                            {
                                foreach (var serverHelpInfo in _msg.getHelpList())
                                {
                                    if(serverHelpInfo != null)
                                        _m_lHelpList.Add(_ABaseMarsPeopleWillHelp.getNewInstance(serverHelpInfo));
                                }
                            }

                            _setSatisfactionDregree(_msg.getSatisfaction());
                            
                            _m_immigrantInfo = _msg.getImmigrant();
                            _updateTodayImmigrantCount(_msg.getImmigrantCount());
                            
                            _m_parentComponent.needUpdateTotalPeopleNum();
                        }

                        if (_isSucc)
                        {
                            _m_immigrantCountResetLocalPushDealer = new MarsPeopleImmigrantCountResetLocalPushDealer();
                            LocalPushMgr.instance.regDealer(_m_immigrantCountResetLocalPushDealer);
                        }

                        _complete?.Invoke(_isSucc);
                    });
                }));
        }
        public override void discard()
        {
            LocalPushMgr.instance.unRegDealer(_m_immigrantCountResetLocalPushDealer);
            _m_immigrantCountResetLocalPushDealer = null;

            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onPlayerBuffChg;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            
            _m_lIntelligentControlInfoList?.Clear();
            _m_lIntelligentControlInfoList = null;

            _m_lLetterList?.Clear();
            _m_lLetterList = null;
            
            _m_lHelpList?.Clear();
            _m_lHelpList = null;
            
            _m_lEventInfoList?.Clear();
            _m_lEventInfoList = null;
            _m_lTriggerEventTipInfoList?.Clear();
            _m_lTriggerEventTipInfoList = null;

            _m_immigrantInfo = null;
            _m_todayImmigrantCount = null;
        }

        #region 智能控制信息

        public MarsIntelligentControlInfo getIntelligentControlInfo(long _id)
        {
            if (_m_lIntelligentControlInfoList != null)
            {
                foreach (var intelligentControlInfo in _m_lIntelligentControlInfoList)
                {
                    if (intelligentControlInfo != null && intelligentControlInfo.id == _id)
                        return intelligentControlInfo;
                }
            }

            if (_m_lIntelligentControlInfoList == null)
                _m_lIntelligentControlInfoList = new List<MarsIntelligentControlInfo>();

            MarsIntelligentControlRefObj intelligentControlRefObj = GRefdataCoreMgr.instance.marsIntelligentControlRefCore.getRef(_id);
            if (intelligentControlRefObj != null)
            {
                MarsIntelligentControlInfo intelligentControlInfo = new MarsIntelligentControlInfo(_id, 0);
                _m_lIntelligentControlInfoList.Add(intelligentControlInfo);
                return intelligentControlInfo;
            }

            return null;
        }
        
        public MarsIntelligentControlInfo getIntelligentControlInfoByBuffId(long _buffId)
        {
            if (_m_lIntelligentControlInfoList != null)
            {
                foreach (var intelligentControlInfo in _m_lIntelligentControlInfoList)
                {
                    if (intelligentControlInfo != null && intelligentControlInfo.buffId == _buffId)
                        return intelligentControlInfo;
                }
            }

            return null;
        }

        #endregion

        #region 火星居民 - 民意信件

        public MarsPeopleWillLetter getLetterByInstanceId(long _instanceId)
        {
            if (_m_lLetterList != null)
            {
                foreach (var letter in _m_lLetterList)
                {
                    if (letter != null && letter.instanceId == _instanceId)
                        return letter;
                }
            }

            return null;
        }
        
        /// <summary>
        /// 更新信件完成状态
        /// </summary>
        public void updateLetterDealedState()
        {
            if(_m_lLetterList == null)
                return;

            int prevSatisfactionDregree = _m_iSatisfactionDregree;
            foreach (var letter in _m_lLetterList)
            {
                if(letter == null || letter.refObj == null)
                    continue;

                MarsPeopleLetterRefObj refObj = letter.refObj;
                // 若当前信件标识为未处理 但是 实际上信件已经满足处理完成条件, 则自动设置为已处理
                if (!letter.isDealed && letter.resolveCondEnable)
                {
                    letter.setDealed();//设置为已处理
                    _setSatisfactionDregree(_m_iSatisfactionDregree + refObj.deal_satisfaction_change);//满意度进行变化
                    
                    WinMsg.SendMsg(WinMsgType.ON_MARS_LETTER_UPDATE, letter);
                }
            }
            
            if(prevSatisfactionDregree != _m_iSatisfactionDregree)
                WinMsg.SendMsg(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG);
        }
        
        /// <summary>
        /// 是否有未处理信件
        /// </summary>
        /// <returns></returns>
        public bool hasUnDealLetter()
        {
            if (_m_lLetterList == null)
                return false;
            
            foreach (var letter in _m_lLetterList)
            {
                if(letter == null)
                    continue;
                
                if (!letter.isDealed && !letter.resolveCondEnable)
                    return true;
            }

            return false;
        }

        private void _setSatisfactionDregree(int _satisfaction)
        {
            _m_iSatisfactionDregree = Math.Clamp(_satisfaction, 0, 10000);//限制在0~10000之间
        }
        
        #endregion

        #region 火星居民 - 民意求助

        public _IMarsPeopleWillHelp getHelpByInstanceId(long _instanceId)
        {
            if (_m_lHelpList != null)
            {
                foreach (var help in _m_lHelpList)
                {
                    if (help != null && help.instanceId == _instanceId)
                        return help;
                }
            }

            return null;
        }
        
        /// <summary>
        /// 是否有未处理的求助
        /// </summary>
        /// <returns></returns>
        public bool hasUnHandleHelp()
        {
            if (_m_lHelpList != null)
            {
                foreach (var help in _m_lHelpList)
                {
                    if (help != null && help.state == EMarsPopularWillHelpState.WAIT_HANDLE)
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region 火星事件
        
        public MarsEventInfo getEventInfo(long _eventId)
        {
            if (_m_lEventInfoList != null)
            {
                foreach (var eventInfo in _m_lEventInfoList)
                {
                    if (eventInfo != null && eventInfo.eventRefId == _eventId)
                        return eventInfo;
                }
            }

            // 到这里说明列表中没有该事件，新增一个
            if (_m_lEventInfoList == null)
                _m_lEventInfoList = new List<MarsEventInfo>();
            MarsEventRefObj eventRefObj = GRefdataCoreMgr.instance.marsEventRefCore.getRef(_eventId);
            if (eventRefObj == null)
            {
                Debug.LogError($"MarsPeopleSubComponent.getEventInfo: 未找到事件配置, eventId={_eventId}");
                return null;
            }
            
            MarsEventInfo marsEventInfo = new MarsEventInfo(eventRefObj);
            _m_lEventInfoList.Add(marsEventInfo);
            return marsEventInfo;
        }

        public MarsEventInfo getEventInfo(MarsEventRefObj _marsEventRefObj)
        {
            if (_marsEventRefObj == null)
                return null;
            
            if (_m_lEventInfoList != null)
            {
                foreach (var eventInfo in _m_lEventInfoList)
                {
                    if (eventInfo != null && eventInfo.eventRefId == _marsEventRefObj.id)
                        return eventInfo;
                }
            }

            // 到这里说明列表中没有该事件，新增一个
            if (_m_lEventInfoList == null)
                _m_lEventInfoList = new List<MarsEventInfo>();
            MarsEventInfo marsEventInfo = new MarsEventInfo(_marsEventRefObj);
            _m_lEventInfoList.Add(marsEventInfo);
            return marsEventInfo;
        }
        
        /// <summary>
        /// 添加触发事件提示
        /// </summary>
        /// <param name="_serverEvent"></param>
        private bool _addTriggerEventTip(Common.MarsObj.Mars_Event _serverEvent)
        {
            if (_serverEvent == null)
                return false;

            MarsEventRefObj eventRefObj = GRefdataCoreMgr.instance.marsEventRefCore.getRef(_serverEvent.getEventId());
            if(eventRefObj == null)
                return false;
            
            if (_m_lTriggerEventTipInfoList == null)
                _m_lTriggerEventTipInfoList = new List<Mars_Event>();
            
            if (eventRefObj.is_singleton_tip)//若该事件为单例提示事件，则移除之前的该事件
            {
                _m_lTriggerEventTipInfoList.RemoveAll((_item) =>
                {
                    return _item != null && _item.getEventId() == _serverEvent.getEventId();
                });
            }
            
            // 当触发事件达到上限时，移除最早的事件
            if(_m_lTriggerEventTipInfoList.Count >= GRefdataCoreMgr.instance.npGeneral.mars_event_tip_show_num_limit)
                _m_lTriggerEventTipInfoList.RemoveAt(0);//移除最早的事件
            
            _m_lTriggerEventTipInfoList.Add(_serverEvent);
            return true;
        }

        /// <summary>
        /// 移除触发事件提示
        /// </summary>
        /// <param name="_eventInfo"></param>
        public void removeTriggerEventTip(Mars_Event _eventInfo)
        {
            if (_m_lTriggerEventTipInfoList == null)
                return;
            
            _m_lTriggerEventTipInfoList.Remove(_eventInfo);
        }

        #endregion

        #region 移民

        /// <summary>
        /// 获取移民数据
        /// </summary>
        /// <returns></returns>
        public Common.MarsObj.Mars_PeopleImmigrant getImmigrantInfo()
        {
            if (_m_immigrantInfo == null)
                return null;

            if (_m_immigrantInfo.getStartMs() <= 0 && _m_immigrantInfo.getEndMs() <= 0)
                _m_immigrantInfo = null;

            return _m_immigrantInfo;
        }
        
        /// <summary>
        /// 更新今日移民次数
        /// </summary>
        private void _updateTodayImmigrantCount(Common.MarsObj.Mars_PeopleImmigrantCount _immigrantCount)
        {
            if(_immigrantCount == null)
                return;

            int dayTag = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);//今日标识
            if(_immigrantCount.getDayTag() < dayTag)//服务器传来的日期标识 小于 当前客户端计算的服务器日期标识, 不进行变化
                return;

            _m_todayImmigrantCount = _immigrantCount;
            WinMsg.SendMsg(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG);
        }

        #endregion
        
        #region C2S

        /// <summary>
        /// 请求火星居民初始化
        /// </summary>
        private void reqMarsPeopleInit(Action<bool, GS2GC_002_074_RetMarsPeopleInit> _callBack)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_002_074_ReqMarsPeopleInit(), new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_074_RetMarsPeopleInit>((_isSucc, _msg) =>
            {
                _callBack?.Invoke(_isSucc, _msg);
            }));
        }

        /// <summary>
        /// 火星居民-移民
        /// </summary>
        /// <param name="_callBack">回调</param>
        public void reqPeopleImmigrant(Action<bool, GS2GC_040_001_RetPeopleImmigrant> _callBack)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_040_001_ReqPeopleImmigrant(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_040_001_RetPeopleImmigrant>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        /// <summary>
        /// 火星居民-确认移民数量
        /// </summary>
        /// <param name="_callBack">回调</param>
        public void reqPeopleConfirmImmigrant(Action<bool, GS2GC_040_002_RetPeopleConfirmImmigrant> _callBack)
        {
            // 直接使用new方法创建GC2GS_040_002_ReqPeopleConfirmImmigrant对象
            GC2GS_040_002_ReqPeopleConfirmImmigrant req = new GC2GS_040_002_ReqPeopleConfirmImmigrant();
            NPGSClientListener.sendRequestByLog(req,
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_040_002_RetPeopleConfirmImmigrant>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        /// <summary>
        /// 火星居民-执行决策
        /// </summary>
        /// <param name="_id">决策ID</param>
        /// <param name="_callBack">回调</param>
        public void reqDealIntelligent(long _id, Action<bool, GS2GC_040_003_RetDealIntelligent> _callBack)
        {
            // 直接使用new方法创建GC2GS_040_003_ReqDealIntelligent对象
            GC2GS_040_003_ReqDealIntelligent req = new GC2GS_040_003_ReqDealIntelligent(_id);
            NPGSClientListener.sendRequestByLog(req,
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_040_003_RetDealIntelligent>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        /// <summary>
        /// 火星居民-处理奖励帮助
        /// </summary>
        /// <param name="_id">实例ID</param>
        /// <param name="_callBack">回调</param>
        public void reqDealRewardHelp(long _id, Action<bool, GS2GC_040_004_RetDealRewardHelp> _callBack)
        {
            // 直接使用new方法创建GC2GS_040_004_ReqDealRewardHelp对象
            GC2GS_040_004_ReqDealRewardHelp req = new GC2GS_040_004_ReqDealRewardHelp(_id);
            NPGSClientListener.sendRequestByLog(req,
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_040_004_RetDealRewardHelp>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        /// <summary>
        /// 火星居民-处理选择帮助
        /// </summary>
        /// <param name="_id">实例ID</param>
        /// <param name="_choice">选择</param>
        /// <param name="_callBack">回调</param>
        public void reqDealChoiceHelp(long _id, int _choice, Action<bool, GS2GC_040_005_RetDealChoiceHelp> _callBack)
        {
            // 直接使用new方法创建GC2GS_040_005_ReqDealChoiceHelp对象
            GC2GS_040_005_ReqDealChoiceHelp req = new GC2GS_040_005_ReqDealChoiceHelp(_id, _choice);
            NPGSClientListener.sendRequestByLog(req,
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_040_005_RetDealChoiceHelp>((_isSucc, _msg) =>
                {
                    _callBack?.Invoke(_isSucc, _msg);
                }));
        }

        #endregion

        #region S2C

        /// <summary>
        /// 人口数量变化推送
        /// </summary>
        public void onMarsPeopleNumChg(GS2GC_040_050_OnMarsPeopleNumChg _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_PeopleNum serverPeopleNumInfo = _msg.getPeopleNum();
            long preIdlePeopleNum = _m_lIdlePeopleNum;
            long preSickPeopleNum = _m_lSickPeopleNum;
            if (serverPeopleNumInfo != null)
            {
                _m_lIdlePeopleNum = serverPeopleNumInfo.getIdle();
                // _m_lWorkingPeopleNum = serverPeopleNumInfo.getWorking();//这边移除工作中居民数量, 直接通过建筑中派遣人数获取
                _m_lSickPeopleNum = serverPeopleNumInfo.getSick();
            }
            if(preIdlePeopleNum != _m_lIdlePeopleNum)
                WinMsg.SendMsg(WinMsgType.ON_MARS_IDLE_PEOPLE_NUM_CHG);
            if(preSickPeopleNum != _m_lSickPeopleNum)
                WinMsg.SendMsg(WinMsgType.ON_MARS_SICK_PEOPLE_NUM_CHG);
            
            // 若空闲或生病居民数量变化，则需要更新总人数
            if(preIdlePeopleNum != _m_lIdlePeopleNum || preSickPeopleNum != _m_lSickPeopleNum)
                parentComponent.needUpdateTotalPeopleNum();
        }

        /// <summary>
        /// 决策数据变化推送
        /// </summary>
        public void onMarsIntelligentChg(GS2GC_040_051_OnIntelligentChg _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_Intelligent serverIntelligent = _msg.getIntelligent();
            if (serverIntelligent == null)
                return;

            MarsIntelligentControlInfo controlInfo = getIntelligentControlInfo(serverIntelligent.getId());
            if(controlInfo == null)
                return;
            
            controlInfo.updateData(serverIntelligent);

            WinMsg.SendMsg(WinMsgType.ON_MARS_INTELLIGENT_CHG, controlInfo);
        }

        /// <summary>
        /// 满意度变化推送
        /// </summary>
        public void onMarsSatisfactionChg(GS2GC_040_052_OnSatisfactionChg _msg)
        {
            if (_msg == null)
                return;

            _setSatisfactionDregree(_msg.getSatisfaction());
            WinMsg.SendMsg(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG);
        }

        /// <summary>
        /// 新增或变化信件推送
        /// </summary>
        public void onMarsLetterChg(GS2GC_040_053_OnLetterChg _msg)
        {
            if (_msg == null || _msg.getLetter() == null)
                return;

            MarsPeopleWillLetter letter = getLetterByInstanceId(_msg.getLetter().getId());
            if (letter == null)
            {
                letter = new MarsPeopleWillLetter(_msg.getLetter());
                
                if (_m_lLetterList == null)
                    _m_lLetterList = new List<MarsPeopleWillLetter>();
                _m_lLetterList.Add(letter);
                
                WinMsg.SendMsg(WinMsgType.ON_MARS_LETTER_ADD, letter);
            }
            else
            {
                letter.update(_msg.getLetter());
                
                WinMsg.SendMsg(WinMsgType.ON_MARS_LETTER_UPDATE, letter);
            }
        }

        /// <summary>
        /// 删除信件推送
        /// </summary>
        public void onMarsLetterDel(GS2GC_040_054_OnLetterDel _msg)
        {
            if (_msg == null || _m_lLetterList == null)
                return;

            for(int i = _m_lLetterList.Count - 1; i >= 0; i--)
            {
                MarsPeopleWillLetter letterInfo = _m_lLetterList[i];
                if(letterInfo != null && letterInfo.instanceId == _msg.getId())
                {
                    _m_lLetterList.RemoveAt(i);
                    break;
                }
            }

            WinMsg.SendMsg(WinMsgType.ON_MARS_LETTER_DEL, _msg.getId());
        }

        /// <summary>
        /// 求助变更推送（新增/更新）
        /// </summary>
        public void onMarsHelpChg(GS2GC_040_055_OnHelpChg _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_Help serverHelp = _msg.getHelp();
            if (serverHelp == null)
                return;

            _IMarsPeopleWillHelp targetHelpInfo = getHelpByInstanceId(serverHelp.getId());
            if (targetHelpInfo == null)
            {
                targetHelpInfo = _ABaseMarsPeopleWillHelp.getNewInstance(serverHelp);

                if (_m_lHelpList == null)
                    _m_lHelpList = new List<_IMarsPeopleWillHelp>();
                _m_lHelpList.Add(targetHelpInfo);
                
                WinMsg.SendMsg(WinMsgType.ON_MARS_HELP_ADD, targetHelpInfo);
            }
            else
            {
                targetHelpInfo.update(serverHelp);
                
                WinMsg.SendMsg(WinMsgType.ON_MARS_HELP_UPDATE, targetHelpInfo);
            }
        }

        /// <summary>
        /// 删除求助推送
        /// </summary>
        public void onMarsHelpDel(GS2GC_040_056_OnHelpDel _msg)
        {
            if (_msg == null || _m_lHelpList == null)
                return;

            long id = _msg.getId();
            for (int i = _m_lHelpList.Count - 1; i >= 0; i--)
            {
                _IMarsPeopleWillHelp item = _m_lHelpList[i];
                if (item != null && item.instanceId == id)
                {
                    _m_lHelpList.RemoveAt(i);
                    break;
                }
            }

            WinMsg.SendMsg(WinMsgType.ON_MARS_HELP_DEL, id);
        }

        /// <summary>
        /// 火星事件触发推送
        /// </summary>
        public void onMarsEventTrigger(GS2GC_040_057_OnMarsEventTrigger _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_Event serverEvent = _msg.getMarsEvent();
            if (serverEvent == null)
                return;

            if(_addTriggerEventTip(serverEvent))
                WinMsg.SendMsg(WinMsgType.ON_MARS_EVENT_TRIGGER);
        }

        /// <summary>
        /// 移民数据增加推送
        /// </summary>
        public void onPeopleImmigrantAdd(GS2GC_040_058_OnPeopleImmigrantAdd _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_PeopleImmigrant serverImmigrantInfo = _msg.getInfo();
            if (serverImmigrantInfo == null)
                return;

            // 更新移民数据
            _m_immigrantInfo = serverImmigrantInfo;
            _m_parentComponent.redTipDealer.refreshPeopleReplenishRedTip();

            WinMsg.SendMsg(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_ADD, serverImmigrantInfo);
        }

        /// <summary>
        /// 移民数据删除推送
        /// </summary>
        public void onPeopleImmigrantDel(GS2GC_040_059_OnPeopleImmigrantDel _msg)
        {
            if (_msg == null)
                return;

            // 清空移民数据
            _m_immigrantInfo = null;
            _m_parentComponent.redTipDealer.refreshPeopleReplenishRedTip();

            WinMsg.SendMsg(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_DEL);
        }

        /// <summary>
        /// 移民次数变化推送
        /// </summary>
        public void onPeopleImmigrantCountChg(GS2GC_040_060_OnPeopleImmigrantCountChg _msg)
        {
            if (_msg == null)
                return;

            Common.MarsObj.Mars_PeopleImmigrantCount serverImmigrantCount = _msg.getInfo();
            if (serverImmigrantCount == null)
                return;

            // 更新移民次数数据
            _updateTodayImmigrantCount(serverImmigrantCount);
        }

        #endregion

        #region 消息监听

        /// <summary>
        /// 当buff数据发生变化
        /// </summary>
        /// <param name="_buffInfo"></param>
        /// <param name="_layer"></param>
        /// <param name="_curLeftTimeMS"></param>
        private void _onPlayerBuffChg(NPPlayerBuffInfo _buffInfo, int _layer, long _curLeftTimeMS)
        {
            if(_buffInfo == null)
                return;

            MarsIntelligentControlInfo intelligentControlInfo = getIntelligentControlInfoByBuffId(_buffInfo.buffId);
            if (intelligentControlInfo != null)
            {
                intelligentControlInfo.updateBuffInfo();
                WinMsg.SendMsg(WinMsgType.ON_MARS_INTELLIGENT_CHG, intelligentControlInfo);
            }
        }

        /// <summary>
        /// 当跨天时
        /// </summary>
        private void _onCrossDay()
        {
            int dayTag = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);//今日标识
            if (_m_todayImmigrantCount != null && _m_todayImmigrantCount.getDayTag() < dayTag)
            {
                _m_todayImmigrantCount.setDayTag(dayTag);
                _m_todayImmigrantCount.setUsedCount(0);
                
                WinMsg.SendMsg(WinMsgType.ON_MARS_PEOPLE_IMMIGRANT_COUNT_CHG);
            }
        }
        
        #endregion
    }
}