using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortObj;
using GC2GS.p015_ConsortOp;
using GS2GC.p002_InitOp;
using GS2GC.p015_ConsortOp;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace GOE
{
    public class ConsortChatComponent : _ANPBasicPlayerComponent
    {
        [NotNull] private List<ConsortPresetChatInfo> _m_presetChatDataList = new List<ConsortPresetChatInfo>();
        
        [NotNull] private ConsortMomentsInfo _m_consortMomentsInfo ;
        
        [NotNull] private List<ConsortAIMomentReqData> _m_consortAIMomentReqDataList = new List<ConsortAIMomentReqData>();
        private bool _m_openConsortAIMomentReq = false;
        
        private long _m_nextMomentTimeMs = -1;
        private bool _m_bHasAddOfflineMoments = false;
        
        private long _m_nextAIFirstTimeMs = -1;
        private bool _m_bHasAddOfflineAIFirst = false;

        private ALCommonEnableTaskController _m_consortChatTick;

        public ConsortMomentsInfo consortMomentsInfo => _m_consortMomentsInfo;

        public ConsortChatComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.CONSORT_CHAT; } }
        public override ENPPlayerCompType[] dependCompList { get { return new[] {ENPPlayerCompType.BASIC_INFO, ENPPlayerCompType.CONSORT}; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqConsortChatInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            List<ConsortPresetChatInfo> chatList = new List<ConsortPresetChatInfo>(_m_presetChatDataList);
            for (int i = 0; i < chatList.Count; i++)
            {
                var chatInfo = chatList[i];
                if (chatInfo != null && chatInfo.needShowAIChat)
                {
                    chatInfo.checkAddFirstMsg();
                }
            }

            _m_consortMomentsInfo = new ConsortMomentsInfo();
            
            sortPresetChatDataList();
            refreshPresetChatRed();

            _m_bHasAddOfflineMoments = false;
            
            _m_consortChatTick.setDisable();
            _m_consortChatTick = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_consortChatTick, 1f);
            
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);

        }

        protected override void _onInitFail()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);

        }

        protected override void _discard()
        {
            _m_presetChatDataList.Clear();
            _m_nextMomentTimeMs = -1;
            _m_consortMomentsInfo?.discard();
            _m_consortChatTick.setDisable();
        }
        /// <summary>
        /// 每秒更新一下妃子聊天
        /// </summary>
        private void _consortChatTick()
        {
            for (int i = 0; i < _m_presetChatDataList.Count; i++)
            {
                ConsortPresetChatInfo presetChatData = _m_presetChatDataList[i];
                presetChatData?.tick(1); // 每秒钟tick一次
            }

            _consortAIFirstMsgTick();
            // 刷新朋友圈
            _consortMomentTick();
        }

        private void _consortAIFirstMsgTick()
        {
            if(AccountSettingMgr.instance.consortAIChatSaverMgr == null || !NPPlayer.instance.consortComp.isInitDone)
                return;
            // 每天的朋友圈数量超过上限则不再添加
            if(AccountSettingMgr.instance.consortAIChatSaverMgr.todayAIFirstCount() > GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_consort_initiate_msg_daily_max_count)
                return;
            // 随机一个下次出现朋友圈的时间
            if (_m_nextAIFirstTimeMs == -1)
            {
                // 如果已经添加过离线朋友圈，则不再添加
                if (!_m_bHasAddOfflineAIFirst)
                {
                    _m_bHasAddOfflineAIFirst = true;
                    long lastAIFirstMsgTag = AccountSettingMgr.instance.consortAIChatSaverMgr.lastAIFirstMsgTag();
                    long curTimeTag = FpsAndPingMgr.instance.serverTimeTag;
                    long interval = curTimeTag - lastAIFirstMsgTag;
                    // 已经有朋友圈的才生成离线朋友圈
                    if (lastAIFirstMsgTag > 0 && interval > GRefdataCoreMgr.instance.npGeneral.consort_chat_consort_initiate_msg_offline_appear_random_time.min  * 1000)
                    {
                        int maxTriggers = GRefdataCoreMgr.instance.npGeneral.consort_chat_consort_initiate_msg_offline_max_count;
                        List<ConsortPresetChatInfo> chatConsortList = new List<ConsortPresetChatInfo>();
                        foreach (ConsortPresetChatInfo chatInfo in _m_presetChatDataList)
                        {
                            if (chatInfo != null && chatInfo.hasAdd && chatInfo.needShowAIChat)
                            {
                                chatConsortList.Add(chatInfo);
                            }
                        }
                        if(chatConsortList.Count < maxTriggers)
                            maxTriggers = chatConsortList.Count;
                        
                        if (interval < GRefdataCoreMgr.instance.npGeneral.consort_chat_consort_initiate_msg_offline_appear_random_time.max  * 1000 * maxTriggers)
                        {
                            int count = 0;
                            long triggerTime = lastAIFirstMsgTag;
                            while (triggerTime < curTimeTag && count < maxTriggers)
                            {
                                triggerTime += GRefdataCoreMgr.instance.npGeneral.consort_chat_consort_initiate_msg_offline_appear_random_time.getRandomValue()  * 1000;
                                if (triggerTime < curTimeTag)
                                {
                                    ConsortPresetChatInfo sendChatInfo = chatConsortList.GetRandomItemAndRemove();
                                    sendChatInfo?.reqSendAIFirstMsg();
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            long baseIntervalHours = interval / (maxTriggers + 1);
                            long halfRange = (long)(baseIntervalHours * 0.8);
                            
                            for (int i = 1; i <= maxTriggers; i++)
                            {
                                long triggerTime = lastAIFirstMsgTag + baseIntervalHours * i  + (long)((Random.value - 0.5f) * halfRange);
                                if (triggerTime < curTimeTag)
                                {
                                    ConsortPresetChatInfo sendChatInfo = chatConsortList.GetRandomItemAndRemove();
                                    sendChatInfo?.reqSendAIFirstMsg();
                                }
                            }
                        }
                    }
                }
              
                _m_nextAIFirstTimeMs = AccountSettingMgr.instance.consortAIChatSaverMgr.lastAIFirstMsgTag() 
                                       + GRefdataCoreMgr.instance.npGeneral.consort_chat_consort_initiate_msg_appear_random_time.getRandomValue() * 1000;
            }
            // 如果当前时间大于下次出现朋友圈的时间，则添加一个朋友圈
            if (FpsAndPingMgr.instance.serverTimeTag > _m_nextAIFirstTimeMs)
            {
                _m_nextAIFirstTimeMs = -1;
                List<ConsortPresetChatInfo> chatConsortList = new List<ConsortPresetChatInfo>();
                foreach (ConsortPresetChatInfo chatInfo in _m_presetChatDataList)
                {
                    if (chatInfo != null && chatInfo.hasAdd && chatInfo.needShowAIChat)
                    {
                        chatConsortList.Add(chatInfo);
                    }
                }
                ConsortPresetChatInfo sendChatInfo = chatConsortList.GetRandomItem();
                sendChatInfo?.reqSendAIFirstMsg();
            }
        }
        

        /// <summary>
        /// 刷新朋友圈
        /// </summary>
        private void _consortMomentTick()
        {
            if(AccountSettingMgr.instance.consortMomentSaverMgr == null || !NPPlayer.instance.consortComp.isInitDone)
                return;
            AccountSettingMgr.instance.consortMomentSaverMgr.checkCommentReply();
            // 每天的朋友圈数量超过上限则不再添加
            if(AccountSettingMgr.instance.consortMomentSaverMgr.todayMomentCount() > GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_daily_max_count)
                return;
            // 随机一个下次出现朋友圈的时间
            if (_m_nextMomentTimeMs == -1)
            {
                // 如果已经添加过离线朋友圈，则不再添加
                if (!_m_bHasAddOfflineMoments)
                {
                    _m_bHasAddOfflineMoments = true;
                    long lastMomentTime = AccountSettingMgr.instance.consortMomentSaverMgr.lastMomentInstanceId();
                    long curTimeTag = FpsAndPingMgr.instance.serverTimeTag;
                    long interval = curTimeTag - lastMomentTime;
                    // 已经有朋友圈的才生成离线朋友圈
                    if (lastMomentTime > 0 && interval > GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_offline_appear_random_time.min  * 1000)
                    {
                        int maxTriggers = GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_offline_max_count;
                        
                        if (interval < GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_offline_appear_random_time.max  * 1000 * maxTriggers)
                        {
                            int count = 0;
                            long triggerTime = lastMomentTime;
                            while (triggerTime < curTimeTag && count < maxTriggers)
                            {
                                triggerTime += GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_offline_appear_random_time.getRandomValue()  * 1000;
                                if (triggerTime < curTimeTag)
                                {
                                    if (AccountSettingMgr.instance.consortMomentSaverMgr.targetMomentInstanceDayMomentCount(triggerTime) 
                                        < GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_daily_max_count)
                                    {
                                        _m_consortMomentsInfo.addMoment(triggerTime, getChatConsortList());
                                        count++;
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            long baseIntervalHours = interval / (maxTriggers + 1);
                            long halfRange = (long)(baseIntervalHours * 0.8);
                            
                            for (int i = 1; i <= maxTriggers; i++)
                            {
                                long triggerTime = lastMomentTime + baseIntervalHours * i  + (long)((Random.value - 0.5f) * halfRange);
                                if (triggerTime < curTimeTag)
                                {
                                    if(AccountSettingMgr.instance.consortMomentSaverMgr.targetMomentInstanceDayMomentCount(triggerTime) < GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_daily_max_count) 
                                        _m_consortMomentsInfo.addMoment(triggerTime, getChatConsortList());
                                }
                            }
                        }
                    }
                }
              
                _m_nextMomentTimeMs = AccountSettingMgr.instance.consortMomentSaverMgr.lastMomentInstanceId() 
                                      + GRefdataCoreMgr.instance.npGeneral.consort_chat_moment_appear_random_time.getRandomValue() * 1000;
            }
            // 如果当前时间大于下次出现朋友圈的时间，则添加一个朋友圈
            if (FpsAndPingMgr.instance.serverTimeTag > _m_nextMomentTimeMs)
            {
                _m_nextMomentTimeMs = -1;
                // 根据已经添加好友的妃子，生成一个朋友圈
                _m_consortMomentsInfo.addMoment(FpsAndPingMgr.instance.serverTimeTag, getChatConsortList());
            }
        }
        public void testMomentsAdd(List<long> _consortList)
        {
#if UNITY_EDITOR
            // 根据已经添加好友的妃子，生成一个朋友圈
            _m_consortMomentsInfo.addMoment(FpsAndPingMgr.instance.serverTimeTag, _consortList);
#endif
        }
        public void testAIFirstMsgAdd(List<long> _consortList)
        {
#if UNITY_EDITOR
            List<ConsortPresetChatInfo> chatConsortList = new List<ConsortPresetChatInfo>();
            foreach (ConsortPresetChatInfo chatInfo in _m_presetChatDataList)
            {
                if (chatInfo != null && chatInfo.hasAdd && chatInfo.needShowAIChat && (_consortList == null || _consortList.Contains(chatInfo.consortId)))
                {
                    chatConsortList.Add(chatInfo);
                }
            }
            ConsortPresetChatInfo sendChatInfo = chatConsortList.GetRandomItem();
            sendChatInfo?.reqSendAIFirstMsg();
#endif
        }
        /// <summary>
        /// 是否已添加妃子好友
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public bool hasAddConsortFriend(long _consortId)
        {
            var chatInfo = getConsortPresetChatInfo(_consortId);
            return chatInfo != null && chatInfo.hasAdd;
        }

        /// <summary>
        /// 获取待添加好友的妃子列表
        /// </summary>
        /// <returns></returns>
        public List<long> getChatNewFriendList()
        {
            List<long> newFriendList = new List<long>();
            foreach (var chatInfo in _m_presetChatDataList)
            {
                if (chatInfo != null && !chatInfo.hasAdd)
                {
                    newFriendList.Add(chatInfo.consortId);
                }
            }
            return newFriendList;
        }

        /// <summary>
        /// 获取预设妃子聊天信息
        /// </summary>
        /// <returns></returns>
        public List<long> getChatConsortList()
        {

            List<long> chatConsortList = new List<long>();
            foreach (var chatInfo in _m_presetChatDataList)
            {
                if (chatInfo != null && chatInfo.hasAdd)
                {
                    chatConsortList.Add(chatInfo.consortId);
                }
            }
            return chatConsortList;
        }
        
        /// <summary>
        /// 获取妃子预设聊天信息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public ConsortPresetChatInfo getConsortPresetChatInfo(long _consortId)
        {
            foreach (var presetChatInfo in _m_presetChatDataList)
            {
                if (presetChatInfo != null && _consortId == presetChatInfo.consortId)
                    return presetChatInfo;
            }
            return null;
        }
        

        /// <summary>
        /// 添加妃子预设聊天信息
        /// </summary>
        /// <param name="_chatInfo"></param>
        private void _addConsortPresetChatInfo(Consort_ChatInfo _chatInfo)
        {
            if(_chatInfo == null)
                return;
            ConsortPresetChatInfo presetChatInfo = null;

            foreach (ConsortPresetChatInfo chatInfo in _m_presetChatDataList)
            {
                if (chatInfo != null && _chatInfo.getConsortId() == chatInfo.consortId)
                {
                    presetChatInfo = chatInfo;
                    break;
                }
            }
            if (presetChatInfo != null)
            {
                presetChatInfo.updateInfo(_chatInfo);
            }
            else
            {
                presetChatInfo = new ConsortPresetChatInfo(_chatInfo);
                _m_presetChatDataList.Add(presetChatInfo);
            }
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND);
            sortPresetChatDataList();
            refreshPresetChatRed();
        }

        #region 红点 red

        /// <summary>
        /// 对预设聊天数据列表进行排序：未读优先，然后按最新对话添加时间排序
        /// </summary>
        public void sortPresetChatDataList()
        {
            _m_presetChatDataList.Sort((a, b) =>
            {
                if (a == null && b == null) return 0;
                if (a == null) return 1;
                if (b == null) return -1;
                
                // 如果未读状态相同，按最新对话添加时间排序（时间越新越靠前）
                return b.lastMsgTime.CompareTo(a.lastMsgTime);
            });
        }

        public void refreshPresetChatRed()
        {
            int count = 0;
            foreach (ConsortPresetChatInfo chatInfo in _m_presetChatDataList)
            {
                if (chatInfo != null && (chatInfo.unread || !chatInfo.hasAdd))
                    count++;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CONSORT_CHAT_PRESET_PAGE, count);
        }

        #endregion
        #region 消息

        private void _onConsortIntimacyChg(params object[] __objs)
        {
            if (__objs.Length == 0)
                return;
            long consortId = (long)__objs[0];
            
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(consortId);

            // 延迟3秒，避免预设回包还没回来，导致判断错误
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (chatInfo != null && chatInfo.needShowAIChat)
                {
                    chatInfo.checkAddFirstMsg();
                    sortPresetChatDataList();
                }
            }, 2);
        }
        
        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqConsortChatInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_055_ReqConsortChatInit());
        }
        
        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retConsortChatInit(GS2GC_002_055_RetConsortChatInit _info)
        {
            _m_presetChatDataList.Clear();
            if (_info != null)
                foreach (Consort_ChatInfo chatInfo in _info.getChatList())
                {
                    if(chatInfo == null)
                        continue;
                    ConsortPresetChatInfo presetChatInfo = null;

                    foreach (var tInfo in _m_presetChatDataList)
                    {
                        if (tInfo != null && chatInfo.getConsortId() == tInfo.consortId)
                        {
                            presetChatInfo = tInfo;
                            break;
                        }
                    }
                    if(presetChatInfo == null)
                    {
                        presetChatInfo = new ConsortPresetChatInfo(chatInfo);
                        _m_presetChatDataList.Add(presetChatInfo);
                    }
                    else
                    {
                        presetChatInfo.updateInfo(chatInfo);
                    }
                }

            sortPresetChatDataList();
            setInitDone();
        }

        /// <summary>
        /// 家人-选择对话选项
        /// </summary>
        public void reqSendChooseDialogueOption(long _consortId, long _dialogueId, long _sentenceId, long _optionId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_015_020_ReqConsortChooseDialogueOption(_consortId, _dialogueId, _sentenceId, _optionId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_020_RetConsortChooseDialogueOption>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        /// <summary>
        /// 家人-领取对话奖励
        /// </summary>
        public void reqConsortDrawDialogueReward(long _consortId, long _dialogueId , Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_015_021_ReqConsortDrawDialogueReward(_consortId, _dialogueId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_021_RetConsortDrawDialogueReward>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }

        /// <summary>
        /// 添加家人好友
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_callback"></param>
        public void reqConsortAddChatFriend(long _consortId, Action<bool> _callback = null)
        {
            // 不是很重要的逻辑，可以直接改
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_consortId);
            chatInfo?.setHasAdd(true);
            NPGSClientListener.sendRequestByLog(new GC2GS_015_023_ReqConsortAddChatFriend(_consortId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_023_RetConsortAddChatFriend>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 请求发送家人AI聊天内容,isMomentContent 是否为朋友圈内容请求
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_momentInstanceId"></param>
        /// <param name="_msgList"></param>
        /// <param name="_isMomentContent"></param>
        /// <param name="_callback"></param>
        public void reqConsortAiChatMoment(long _consortId, long _momentInstanceId, List<Common.Common_AiChatMessage> _msgList, bool _isMomentContent)
        {
            _m_consortAIMomentReqDataList.Add(new ConsortAIMomentReqData(_consortId, _momentInstanceId, _isMomentContent, _msgList));
            _openAIMomentReqTick();
        }
        public void reqConsortAIMomentContent(long _consortId,  long _momentInstanceId, List<Common.Common_AiChatMessage> _msgList)
        {
            reqConsortAiChatMoment(_consortId, _momentInstanceId, _msgList, true);
        }

        private void _openAIMomentReqTick()
        {
            if(_m_openConsortAIMomentReq)
                return;
            _m_openConsortAIMomentReq = true;
            _updateAIMomentReqTick();
        }

        /// <summary>
        /// 朋友圈内容请求改为先存到列表中，然后每2s发送一条请求
        /// </summary>
        private void _updateAIMomentReqTick()
        {
            if(!_m_openConsortAIMomentReq)
                return;
            if(_m_consortAIMomentReqDataList.Count <= 0)
            {
                _m_openConsortAIMomentReq = false;
                return;
            }
            ConsortAIMomentReqData reqData = _m_consortAIMomentReqDataList.GetFirstAndRemove();

            if (reqData != null)
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_015_024_ReqConsortAiChatMoment(reqData.momentInstanceId, reqData.msgList, reqData.consortId, reqData.isMomentContent),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_024_RetConsortAiChatMoment>(
                        (_isSuc, _msg) => { }, null, false));
                ALCommonTaskController.CommonActionAddMonoTask(_updateAIMomentReqTick, 2);
            }
            else
            {
                _updateAIMomentReqTick();
            }
        }
        


        /// <summary>
        /// 请求增加AI妃子朋友圈评论
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_momentInstanceId"></param>
        /// <param name="_msgList"></param>
        /// <param name="_callback"></param>
        public void reqConsortEvaluateReply(long _consortId, long _momentInstanceId, List<Common.Common_AiChatMessage> _msgList, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_015_026_ReqConsortEvaluateReply(_momentInstanceId, _consortId, _msgList),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_026_RetConsortEvaluateReply>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 新增家人对话推送
        /// </summary>
        public void onConsortChatDialogueAdd(GS2GC_015_070_OnConsortChatDialogueAdd _msg)
        {    
            if (_msg == null)
                return;
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_msg.getConsortId());
            chatInfo?.onChatDialogueAdd(_msg.getDialogueId(), _msg.getTriggerTimeMs());
            sortPresetChatDataList();
            refreshPresetChatRed();
        }
        
        /// <summary>
        /// 家人对话奖励领取推送
        /// </summary>
        public void onConsortChatDialogueRewardDraw(GS2GC_015_071_OnConsortChatDialogueRewardDraw _msg)
        {
            if (_msg == null)
                return;
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_msg.getConsortId());
            chatInfo?.onChatDialogueRewardDraw(_msg.getDialogueId());
            sortPresetChatDataList();
            refreshPresetChatRed();
        }
        /// <summary>
        /// 家人对话选择变更推送
        /// </summary>
        public void onConsortChatDialogueOptionChg(GS2GC_015_072_OnConsortChatDialogueOptionChg _msg)
        {
            if (_msg == null)
                return;
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_msg.getConsortId());
            chatInfo?.onDialogueOptionChg(_msg.getDialogueId(), _msg.getOptionInfo());
            sortPresetChatDataList();
            refreshPresetChatRed();
        }

        /// <summary>
        /// 家人对话是否添加好友标志位变更
        /// </summary>
        public void onConsortChatHasAddChg(GS2GC_015_074_OnConsortChatHasAddChg _msg)
        {
            if (_msg == null)
                return;
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_msg.getConsortId());
            chatInfo?.setHasAdd(_msg.getHasAdd());

            if (chatInfo != null && chatInfo.needShowAIChat)
            {
                chatInfo.checkAddFirstMsg();
            }
            sortPresetChatDataList();


            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_ADD_FRIEND);
        }
        /// <summary>
        /// 家人对话信息新增
        /// </summary>
        public void onConsortChatInfoAdd(GS2GC_015_075_OnConsortChatInfoAdd _msg)
        {
            if (_msg == null)
                return;
            _addConsortPresetChatInfo(_msg.getChatInfo());
        }
        
        public void onConsortAiChatMomentMsgAdd(GS2GC_015_076_OnConsortAiChatMomentMsgAdd _msg)
        {
            if (_msg == null)
                return;
            long instanceId = _msg.getInstanceId();
            ConsortMomentSaver momentSaver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(instanceId);
            if (momentSaver == null)
                return;
            if (!momentSaver.hasGetContent)
            {
                _m_consortMomentsInfo.updateMomentContent(_msg.getInstanceId(), _msg.getMsg(), _msg.getErrCode());
                return;
            }

            _m_consortMomentsInfo.addConsortAIComment(_msg.getInstanceId(), _msg.getConsortId(), _msg.getMsg(), _msg.getErrCode());
        }

        #region AI 聊天
        
        /// <summary>
        /// 家人-Ai对话 _msgList 从旧到新
        /// </summary>
        public void reqSendAIChatMsg(long _consortId, List<Common.Common_AiChatMessage> _msgList, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_015_022_ReqConsortAiChat(_consortId, _msgList, 1),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_022_RetConsortAiChat>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        
        /// <summary>
        /// 家人-Ai对话 _msgList 从旧到新
        /// </summary>
        public void reqConsortInitiateAiChat(long _consortId, List<Common.Common_AiChatMessage> _msgList, Action<bool> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_015_025_ReqConsortInitiateAiChat(_consortId, _msgList, 2),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_015_025_RetConsortInitiateAiChat>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc);
                }, null, false));
        }
        /// <summary>
        /// 家人AI对话新增消息
        /// </summary>
        public void onConsortAiChatMsgAdd(GS2GC_015_073_OnConsortAiChatMsgAdd _msg)
        {
            if (_msg == null)
                return;
            ConsortPresetChatInfo chatInfo = getConsortPresetChatInfo(_msg.getConsortId());
            if (chatInfo == null)
                return;
            chatInfo.onConsortAiChatMsgAdd(_msg.getMsg(), _msg.getClientDataId() == 2, _msg.getErrCode());
            sortPresetChatDataList();
        }
        
        
        #endregion
        
        #endregion
    }
}