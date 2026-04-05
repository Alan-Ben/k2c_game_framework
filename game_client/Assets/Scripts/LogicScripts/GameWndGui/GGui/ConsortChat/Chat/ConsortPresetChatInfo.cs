using System;
using System.Collections.Generic;
using ALPackage;
using Common;
using Common.ConsortObj;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class ConsortPresetChatInfo : _AConsortChatInfo
    {
        private long _m_consortId;
        private bool _m_hasAdd; // 是否已添加好友
        private long _m_addFriendTimeMs; // 添加好友的时间 只有添加好友回包那一下会记录，其他情况都为0，为了给添加好友马上解锁对话的时候只初始化首句用
        [NotNull]private List<ConsortPresetChatDialogue> _m_lDialogueList = new List<ConsortPresetChatDialogue>(); // 所有的对话
        private ConsortPresetChatDialogue _m_latestShowDialogue; // 最新的在处理的或者已经领取奖励的对话

        // -------- AI -------------
        
        private ConsortChatAIRefObj _m_consortChatAIRefObj;
        private bool _m_isWaitingAIResponse = false;

        public long aiLastMessageTime
        {
            get
            {
                var historyMsgList = AccountSettingMgr.instance.consortAIChatSaverMgr.getHistoryMsgList(_m_consortId);
                if (historyMsgList == null || historyMsgList.Count == 0)
                    return 0;
                
                long maxTime = 0;
                foreach (var msgData in historyMsgList)
                {
                    if (msgData != null && msgData.msgId > maxTime)
                        maxTime = msgData.msgId;
                }
                return maxTime;
            }
        }

        private long presetLastMessageTime
        {
            get
            {
                if (AccountSettingMgr.instance.consortPresetChatSaver != null)
                    return AccountSettingMgr.instance.consortPresetChatSaver.getConsortLatestMsgTime(_m_consortId);
                return 0;
            }
        }
        
        // -------- AI-----------
        public bool hasAdd => _m_hasAdd;
        public bool unread
        {
            get
            {
                return (_m_latestShowDialogue != null && !_m_latestShowDialogue.hadDraw) ||  AccountSettingMgr.instance.consortAIChatSaverMgr.hasUnRead(_m_consortId);
            }
        }

        public long consortId => _m_consortId;
        
        /// <summary>
        /// 获取最新对话添加时间，用于排序
        /// </summary>
        public long lastMsgTime 
        {
            get
            {
                return Math.Max(presetLastMessageTime, aiLastMessageTime);
            }
        }
        
          
        public bool hasUnlockAIChat 
        {
            get
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);
                return _m_consortChatAIRefObj != null && consortInfo != null && consortInfo.intimacy >= _m_consortChatAIRefObj.intimacy_count;
            }
        }

        public bool needShowAIChat
        {
            get
            {
                return hasUnlockAIChat && _m_latestShowDialogue.hadDraw;
            }
        }

        public string unlockAIChatTip
        {
            get
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortId);

                return TextTranslate.instance.getLanguage(TransKeyConst.consort_chat_unlock_ai_chat_tip,
                    consortInfo?.consortTransName, _m_consortChatAIRefObj?.intimacy_count);
            }
        }

        public ConsortPresetChatInfo([NotNull]Consort_ChatInfo _chatInfo)
        {
            updateInfo(_chatInfo);
        }

        public void updateInfo([NotNull]Consort_ChatInfo _chatInfo)
        {
            _m_consortId = _chatInfo.getConsortId();
            _m_consortChatAIRefObj = GRefdataCoreMgr.instance.consortChatAIRefCore.getRef(_m_consortId);
            _m_hasAdd = _chatInfo.getHasAdd();
            _m_lDialogueList.Clear();
            foreach (Consort_ChatDialogue dialogue in _chatInfo.getDialogueList())
            {
                if(dialogue == null) continue;
                _m_lDialogueList.Add(new ConsortPresetChatDialogue(_m_consortId, dialogue));
            }
            // 按触发时间先后排序，旧到新
            _m_lDialogueList.Sort(ConsortPresetChatDialogue.sortDialogue);
            _m_addFriendTimeMs = 0;
            _checkNextDealDialogue();
        }


        /// <summary>
        /// 每秒刷新一下对话，根据情况更新妃子消息
        /// </summary>
        public void tick(float _delayTime)
        {
            if (_m_latestShowDialogue != null && !_m_latestShowDialogue.hadDraw && _m_hasAdd)
            {
                _m_latestShowDialogue.tick(_delayTime);
            }
        }

        /// <summary>
        /// 是否需要显示对话选项
        /// </summary>
        /// <param name="_dialogueId"></param>
        /// <param name="_sentence"></param>
        /// <returns></returns>
        public bool needShowOption(out long _dialogueId, out ConsortChatDialogueSentenceRefObj _sentence)
        {
            if (_m_latestShowDialogue != null && _m_latestShowDialogue.isDealing &&
                _m_latestShowDialogue.needShowOption(out _sentence))
            {
                _dialogueId = _m_latestShowDialogue.dialogueId;
                return true;
            }

            _dialogueId = 0;
            _sentence = null;
            return false;

        } 
        
        /// <summary>
        /// 获取缩略显示文本
        /// </summary>
        /// <param name="_action"></param>
        public void getMiniContent(Action<string> _action)
        {
            ConsortAIChatSimpleData aiLastMsg = AccountSettingMgr.instance.consortAIChatSaverMgr.lastAISimpleData(_m_consortId);

            if (aiLastMsg != null && _m_latestShowDialogue != null)
            {
                if (_m_latestShowDialogue.triggerTimeMs > aiLastMsg.lastMsgTime)
                {
                    _m_latestShowDialogue.init(_m_addFriendTimeMs, false);
                    _action?.Invoke(_m_latestShowDialogue.getMiniContent());
                    return;
                }
                else
                {
                    _action?.Invoke(aiLastMsg.lastContent);
                    return;
                }

            }
            if (aiLastMsg != null)
            {
                _action?.Invoke(aiLastMsg.lastContent);
                return;
            }
            if (_m_latestShowDialogue != null)
            {
                _m_latestShowDialogue.init(_m_addFriendTimeMs, false);
                _action?.Invoke(_m_latestShowDialogue.getMiniContent());
                return;
            }
        }
        
        /// <summary>
        /// 检查赋值下一个要处理的对话
        /// </summary>
        private void _checkNextDealDialogue()
        {
            if (_m_latestShowDialogue != null)
            {
                // 最后一个对话还在处理中，则直接返回
                if (_m_latestShowDialogue.isDealing)
                    return;
                _m_latestShowDialogue.onShowTypingChange -= setShowTyping;
                _m_latestShowDialogue.onReceiveMsg -= _onReceiveMsg;
                _m_latestShowDialogue.setIsDealing(false);
                _m_latestShowDialogue.setIsLastDialogue(false);
                _m_latestShowDialogue = null;
            }
            foreach (ConsortPresetChatDialogue dialogue in _m_lDialogueList)
            {
                if(dialogue == null) continue;
                
                if(dialogue.hadDraw)
                    continue;
                
                // 取未领取奖励的最老对话作为最后一个对话，并开始处理对话
                _m_latestShowDialogue = dialogue;
                if (_m_hasAdd)
                {
                    _m_latestShowDialogue.onReceiveMsg += _onReceiveMsg;
                    _m_latestShowDialogue.onShowTypingChange += setShowTyping;
                    _m_latestShowDialogue.setIsDealing(true);
                    _m_latestShowDialogue.setIsLastDialogue(true);
                    _m_latestShowDialogue.init(_m_addFriendTimeMs, true);
                }
                break;
                
            }

            if (_m_latestShowDialogue == null)
            {
                _m_latestShowDialogue = _m_lDialogueList.GetLast();
                _m_latestShowDialogue?.setIsLastDialogue(true);
            }
        }

        #region AI Chat

        /// <summary>
        /// 检查是否要添加妃子AI聊天第一条消息
        /// </summary>
        public void checkAddFirstMsg()
        {
            if(_m_consortChatAIRefObj == null)
                return;
            if (hasUnlockAIChat && !AccountSettingMgr.instance.consortAIChatSaverMgr.hasAddedMsg(_m_consortId))
            {
                string content = TextTranslate.instance.getLanguage(_m_consortChatAIRefObj.ai_first_list.GetRandomItem());
                AccountSettingMgr.instance.consortAIChatSaverMgr.addAIFirstMsg(FpsAndPingMgr.instance.serverTimeTag);
                NPPlayer.instance.consortChatComp.onConsortAiChatMsgAdd(new GS2GC.p015_ConsortOp.GS2GC_015_073_OnConsortAiChatMsgAdd(_m_consortId, content, 0,2));
            }
        }
        
        /// <summary>
        /// 发送消息给妃子
        /// </summary>
        /// <param name="_content"></param>
        public void reqSendAIChatMsg(string _content, Action<bool> _action)
        {
            if (_m_isWaitingAIResponse)
                return;
            _m_isWaitingAIResponse = true;
            ConsortAISaverMsgData data = AccountSettingMgr.instance.consortAIChatSaverMgr.addPlayerSendNewMsg(_m_consortId, _content);
            ConsortChatAIPlayerMsgInfo msg = new ConsortChatAIPlayerMsgInfo(data.msgId, data.gameContent);
            _onReceiveMsg(msg);
            setShowTyping(true);
            NPPlayer.instance.consortChatComp.reqSendAIChatMsg(_m_consortId, AccountSettingMgr.instance.consortAIChatSaverMgr.getReqAIChatMsgList(_m_consortId),
                _suc =>
                {
                    if (!_suc)
                        _m_isWaitingAIResponse = false;
                    _action?.Invoke(_suc);
                });
        }

        public void reqSendAIFirstMsg()
        {
            if(!AccountSettingMgr.instance.consortAIChatSaverMgr.hasAddedMsg(_m_consortId))
                return;
            if (_m_isWaitingAIResponse)
                return;
            _m_isWaitingAIResponse = true;
            setShowTyping(true);
            AccountSettingMgr.instance.consortAIChatSaverMgr.addAIFirstMsg(FpsAndPingMgr.instance.serverTimeTag);
            List<Common_AiChatMessage> msgList = AccountSettingMgr.instance.consortAIChatSaverMgr.getReqAIChatMsgList(_m_consortId);
            var lastMsg = msgList.GetLast();
            if (lastMsg == null || lastMsg.getRoleType() != EAiChatRoleType.USER)
                msgList.Add(new Common_AiChatMessage(EAiChatRoleType.USER, ""));
            NPPlayer.instance.consortChatComp.reqConsortInitiateAiChat(_m_consortId, msgList,
                _suc =>
                {
                    if (!_suc)
                        _m_isWaitingAIResponse = false;
                });
        }
        
        /// <summary>
        /// 收到妃子回复消息
        /// </summary>
        /// <param name="_content"></param>
        public void onConsortAiChatMsgAdd(string _content, bool _isConsortInitiateAiChat,int _errorCode = 0)
        {
            _m_isWaitingAIResponse = false;
            // 如果错误码不为0，则表示发生了错误，消息内容替换为默认出错回复
            if(_errorCode != 0)
            {
                // 如果是妃子发起的，发生错误则不生成消息
                if(_isConsortInitiateAiChat)
                {
                    setShowTyping(false);
                    return;
                }
                _content = TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_error_reply_list.GetRandomItem());
            }
            ConsortAISaverMsgData data = AccountSettingMgr.instance.consortAIChatSaverMgr.addConsortSendNewMsg(FpsAndPingMgr.instance.serverTimeTag, _m_consortId, _content);
            ConsortChatAIMsgInfo msg = new ConsortChatAIMsgInfo(data.msgId, data.gameContent);
            setShowTyping(false);
            _onReceiveMsg(msg);
        }
        #endregion


        private void _onReceiveMsg(_AConsortChatMsgInfo _msg)
        {
            receiveMsg(_msg);
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_CHAT_PRESET_CHAT_CHG, _m_consortId);
        }

        #region 协议更新状态

        /// <summary>
        /// 对话选项发生改变
        /// </summary>
        /// <param name="_dialogueId"></param>
        /// <param name="_optionInfo"></param>
        public void onDialogueOptionChg(long _dialogueId, Common.ConsortObj.Consort_DialogueOption _optionInfo)
        {
            foreach (var dialogue in _m_lDialogueList)
            {
                if (dialogue != null && dialogue.dialogueId == _dialogueId)
                {
                    dialogue.updateOption(_optionInfo);
                    break;
                }
            }
        }
        
        /// <summary>
        /// 当有新的对话添加进来
        /// </summary>
        /// <param name="_dialogueId"></param>
        /// <param name="_triggerTimeMs"></param>
        public void onChatDialogueAdd(long _dialogueId, long _triggerTimeMs)
        {
            _m_lDialogueList.Add(new ConsortPresetChatDialogue(_m_consortId, _dialogueId, _triggerTimeMs));
            _checkNextDealDialogue();
        }

        /// <summary>
        /// 对话奖励领取回包
        /// </summary>
        public void onChatDialogueRewardDraw(long _dialogueId)
        {
            foreach (var dialogue in _m_lDialogueList)
            {
                if (dialogue != null && dialogue.dialogueId == _dialogueId)
                {
                    dialogue.setHadDrawReward();
                    break;
                }
            }
            _checkNextDealDialogue();
        }
        
        /// <summary>
        /// 设置妃子已添加好友
        /// </summary>
        /// <param name="_hasAdd"></param>
        public void setHasAdd(bool _hasAdd)
        {
            _m_hasAdd = _hasAdd;
            _m_addFriendTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            if(_hasAdd)
                AccountSettingMgr.instance.consortPresetChatSaver.addConsortLatestMsgTime(_m_consortId, FpsAndPingMgr.instance.serverTimeTag);
            _checkNextDealDialogue();
        }
        
        #endregion
        /// <summary>
        /// 给的是从0-n ->新到旧
        /// </summary>
        /// <param name="_msgId"></param>
        /// <param name="_msgCount"></param>
        /// <param name="_action"></param>
        protected override void _getHistoryList(long _msgId, int _msgCount, Action<List<_AConsortChatMsgInfo>> _action)
        {
            List<_AConsortChatMsgInfo> resultList = new List<_AConsortChatMsgInfo>();
            
            // 获取AI聊天消息列表
            List<ConsortAISaverMsgData> aiMsgInfoList = AccountSettingMgr.instance.consortAIChatSaverMgr.getHistoryMsgList(_m_consortId);
            if(aiMsgInfoList == null)
                aiMsgInfoList = new List<ConsortAISaverMsgData>();
            
            // 筛选可用的dialogue列表（已初始化且可获取消息的）
            List<ConsortPresetChatDialogue> validDialogueList = new List<ConsortPresetChatDialogue>();
            foreach (var dialogue in _m_lDialogueList)
            {
                if (dialogue != null && dialogue.canGetMsg && dialogue.msgList != null && dialogue.msgList.Count > 0)
                {
                    validDialogueList.Add(dialogue);
                }
            }
            
            // 双指针归并：从后往前（从新到旧）遍历
            int dialogueIdx = validDialogueList.Count - 1; // dialogue列表指针
            int aiMsgIdx = aiMsgInfoList.Count - 1; // AI消息列表指针
            int count = 0;

            bool lastMsgIsAIChat = true; // 当前已添加的最旧消息是否为AI消息
            while (count < _msgCount && (dialogueIdx >= 0 || aiMsgIdx >= 0))
            {
                long dialogueTime = long.MinValue;
                long aiMsgTime = long.MinValue;
                
                // 获取当前dialogue的时间（整个dialogue作为一个单位）
                if (dialogueIdx >= 0)
                {
                    dialogueTime = validDialogueList[dialogueIdx].triggerTimeMs;
                }
                
                // 获取当前AI消息的时间
                if (aiMsgIdx >= 0)
                {
                    aiMsgTime = aiMsgInfoList[aiMsgIdx].msgId;
                }
                
                // 比较时间，选择较新的消息
                if (dialogueTime >= aiMsgTime)
                {
                    // 处理dialogue - 一次性取出该dialogue的所有消息（作为整体）
                    ConsortPresetChatDialogue dialogue = validDialogueList[dialogueIdx];
                    dialogueIdx--; // 移动到下一个dialogue
                    
                    // 从dialogue中取所有消息（从后往前）
                    for (int j = dialogue.msgList.Count - 1; j >= 0 && count < _msgCount; j--)
                    {
                        var msg = dialogue.msgList[j];
                        
                        if (msg == null)
                            continue;
                        if (msg.msgId >= _msgId && _msgId > -1)
                        {
                            lastMsgIsAIChat = false;
                            continue;
                        }
                        
                        resultList.Insert(0, msg);
                        count++;
                        lastMsgIsAIChat = false;
                    }
                }
                else
                {
                    // 处理AI消息
                    ConsortAISaverMsgData aiMsg = aiMsgInfoList[aiMsgIdx];
                    aiMsgIdx--;
                    
                    if (aiMsg == null)
                        continue;
                    if (aiMsg.msgId >= _msgId && _msgId > -1)
                    {
                        lastMsgIsAIChat = true;
                        continue;
                    }
                    // 如果上一条消息是预设对话，则需要加时间分割消息
                    if (!lastMsgIsAIChat)
                    {
                        long timeMsgTimeTag = aiMsg.msgId + 1;
                        resultList.Insert(0, new ConsortChatMsgTimeInfo(timeMsgTimeTag, timeMsgTimeTag));
                    }
                    if (aiMsg.senderIsPlayer)
                    {
                        resultList.Insert(0, new ConsortChatAIPlayerMsgInfo(aiMsg.msgId, aiMsg.gameContent));
                    }
                    else
                    {
                        resultList.Insert(0, new ConsortChatAIMsgInfo(aiMsg.msgId, aiMsg.gameContent));
                    }
                    count++;
                    lastMsgIsAIChat = true;
                }
            }

            _action?.Invoke(resultList);
        }

        protected override void _doLoadOp(Action<List<_AConsortChatMsgInfo>> _action)
        {
            //逐个开启加载
            for (int i = 0; i < _m_lDialogueList.Count; i++)
            {
                ConsortPresetChatDialogue loadObj = _m_lDialogueList[i];
                if (null == loadObj)
                    continue;
                
                // 如果还不能获取消息，则跳过
                if (!loadObj.canGetMsg)
                    continue;
                loadObj.init(_m_addFriendTimeMs, false);
            }
            _action?.Invoke(null);

        }

    }
}