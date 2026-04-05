using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConsortObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class ConsortPresetChatDialogue
    {
        private long _m_consortId;
        private long _m_dialogueId;
        private bool _m_isInited = false; // 是否已初始化
        private long _m_triggerTimeMs;// 触发时间戳
        private long _m_getRewardTimeMs;// 领奖时间戳
        [NotNull]private Dictionary<long, long> _m_options;
        private bool _m_hadDraw;// 是否领奖
        private bool _m_isDealing = false;
        private bool _m_isTyping = false; // 是否正在输入
        private long _m_curMsgId;
        private bool _m_reqDrawRewarding = false; // 是否正在请求领取奖励
        private bool _m_hadAddRewardMsg = false; // 是否已添加奖励消息
        private bool _m_isLastDialogue = false; // 是否已获得对话里的最后一条

        private float _m_curTickAddMsgTime = 0f; // 刷新下一条消息的计时器

        private ConsortChatDialogueRefObj _m_dialogueRefObj;
        private List<ConsortChatDialogueSentenceRefObj> _m_sentenceList;
        private List<_AConsortChatMsgInfo> _m_msgList;

        public Action<bool> onShowTypingChange;
        public Action<_AConsortChatMsgInfo> onReceiveMsg;

        public long dialogueId { get { return _m_dialogueId; } }
        public long triggerTimeMs { get { return _m_triggerTimeMs; } }
        public long getRewardTimeMs { get { return _m_getRewardTimeMs; } }
        public List<_AConsortChatMsgInfo> msgList { get { return _m_msgList; } }
        public bool hadDraw => _m_hadDraw;        /// <summary>
        /// 是否可以获取消息，已领取奖励和再处理中的可以获取消息
        /// </summary>
        public bool canGetMsg => _m_hadDraw || _m_isDealing;
        public bool isDealing => _m_isDealing;
        public bool isLastDialogue => _m_isLastDialogue;


        public ConsortPresetChatDialogue(long _consortId, [NotNull]Consort_ChatDialogue _dialogue)
        {
            _m_consortId = _consortId;
            _m_dialogueId = _dialogue.getDialogueId();
            _m_dialogueRefObj = GRefdataCoreMgr.instance.consortChatDialogueRefCore.getRef(_m_dialogueId);
            _m_triggerTimeMs = _dialogue.getTriggerTimeMs();
            _m_getRewardTimeMs = _dialogue.getDrawRewardTimeMs();
            _m_hadDraw = _dialogue.getHadDraw();
            _m_isDealing = false;
            _m_options = new Dictionary<long, long>();
            _m_options.Clear();
            foreach (var option in _dialogue.getOptionList())
            {
                if (option == null) continue;
                _m_options[option.getSentenceId()] = option.getOptionId();
            }
        }

        public ConsortPresetChatDialogue(long _consortId, long _dialogueId, long _triggerTimeMs)
        {
            _m_consortId = _consortId;
            _m_dialogueId = _dialogueId;            
            _m_dialogueRefObj = GRefdataCoreMgr.instance.consortChatDialogueRefCore.getRef(_m_dialogueId);
            _m_triggerTimeMs = _triggerTimeMs;
            _m_hadDraw = false;
            _m_isDealing = false;
            _m_options = new Dictionary<long, long>();
        }
        

        public void tick(float _delayTime)
        {
            if(_m_hadDraw)
                return;
            if(!_m_isInited)
                return;

            if (_m_sentenceList != null && _m_msgList != null)
            {
                ConsortChatDialogueSentenceRefObj lastSentence = _m_sentenceList.GetLast();

                if (lastSentence != null)
                {
                    long nextSentenceId = lastSentence.next_id;
                    
                    if (nextSentenceId == -1)
                    {
                        if (!_m_hadAddRewardMsg)
                        {
                            _m_hadAddRewardMsg = true;
                            _addMsg(null, new ConsortChatMsgRewardInfo(this, _m_dialogueRefObj, _m_curMsgId++, false));
                            AccountSettingMgr.instance.consortPresetChatSaver.addConsortLatestMsgTime(_m_consortId, FpsAndPingMgr.instance.serverTimeTag);
                        }
                    }
                    else if (nextSentenceId == 0)
                    {
                        
                    }
                    else
                    {
                        ConsortChatDialogueSentenceRefObj nextSentence = GRefdataCoreMgr.instance.consortChatDialogueSentenceRefCore.getRef(nextSentenceId);
                        if (nextSentence != null)
                        {
                            if (!_m_isTyping)
                            {
                                _m_curTickAddMsgTime = 0f;
                                _setShowTyping(true);
                            }
                            else
                            {
                                _m_curTickAddMsgTime += _delayTime;
                                if (_m_curTickAddMsgTime < GRefdataCoreMgr.instance.npGeneral.consort_chat_preset_reply_typing_time)
                                    return;
                                _setShowTyping(false);
                                _m_curTickAddMsgTime = 0f;
                                ConsortChatMsgSentenceInfo msg = new ConsortChatMsgSentenceInfo(nextSentence, _m_curMsgId++);
                                _addMsg(nextSentence, msg);
                                AccountSettingMgr.instance.consortPresetChatSaver.addConsortLatestMsgTime(_m_consortId, FpsAndPingMgr.instance.serverTimeTag);
                            }
                         
                        }
                    }
                }
            }
        }
        
        public void updateOption(Consort_DialogueOption _optionInfo)
        {
            if (_optionInfo == null) return ;
            _m_options[_optionInfo.getSentenceId()] = _optionInfo.getOptionId();
            
            ConsortChatDialogueSentenceRefObj lastSentence = _m_sentenceList.GetLast();

            if (lastSentence != null && _m_isInited)
            {   
                if(_m_options.TryGetValue(lastSentence.id, out long _optionId))
                {
                    ConsortChatDialogueSentenceRefObj optionRef =  GRefdataCoreMgr.instance.consortChatDialogueSentenceRefCore.getRef(_optionId);

                    if (optionRef != null)
                    {
                        var msg = new ConsortChatMsgResponseInfo(optionRef, _m_curMsgId++);
                        _addMsg(optionRef, msg);
                        return ;
                    }
                }
            }

            return ;
        }

        private void _addMsg(ConsortChatDialogueSentenceRefObj _sentenceRefObj, _AConsortChatMsgInfo _msg)
        {
            if(_m_sentenceList == null || _m_msgList == null)
                return;
            
            if (_sentenceRefObj != null) 
                _m_sentenceList.Add(_sentenceRefObj);
            _m_msgList.Add(_msg);
            onReceiveMsg?.Invoke(_msg);
        }

        private void _setShowTyping(bool _showTyping)
        {
            _m_isTyping = _showTyping;
            onShowTypingChange?.Invoke(_showTyping);
        }

        /// <summary>
        /// ui打开时尝试请求领取奖励
        /// </summary>
        public void tryReqDrawReward()
        {
            if(_m_hadDraw)
                return;
            if(_m_reqDrawRewarding)
                return;
            _m_reqDrawRewarding = true;
            NPPlayer.instance.consortChatComp.reqConsortDrawDialogueReward(_m_consortId, _m_dialogueId, null);
        }


        /// <summary>
        /// 设置为已领取奖励
        /// </summary>
        public void setHadDrawReward()
        {
            _m_hadDraw = true;
            _m_isDealing = false;
            _m_reqDrawRewarding = false;
            _m_getRewardTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            if (_m_dialogueRefObj != null && _m_dialogueRefObj.reward_item_list != null)
            {
                List<_IItem> rewardItems = new List<_IItem>(_m_dialogueRefObj.reward_item_list);
                NPCommonCostItem intimacyCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.consort_intimacy_item,
                    _m_dialogueRefObj.reward_intimacy);
                rewardItems.Add(intimacyCostItem);
                GCommon.showGainRewardTip(rewardItems);
            }
        }
        
        /// <summary>
        /// 设置是否正在处理对话
        /// </summary>
        /// <param name="_isDealing"></param>
        public void setIsDealing(bool _isDealing)
        {
            _m_isDealing = _isDealing;
        }

        public void setIsLastDialogue(bool _isLastDialogue)
        {
            _m_isLastDialogue = _isLastDialogue;
        }

        
        public bool needShowOption(out ConsortChatDialogueSentenceRefObj _sentenceRef)
        {
            _sentenceRef = _m_sentenceList.GetLast();
        
            if (_sentenceRef != null && _sentenceRef.response_sentence_list != null && _sentenceRef.response_sentence_list.Count > 0)
            {
                var lastMsg = _m_msgList.GetLast();
                if(lastMsg != null && lastMsg.showTyping)
                {
                    return false;
                }
                return true;
            }
        
            return false;
        }
        
        public string getMiniContent()
        {
            for (var i = _m_msgList.Count - 1; i >= 0; i--)
            {
                var msg = _m_msgList[i];
                if(msg == null || msg.msgType == EConsortChatMsgType.Reward)
                    continue;
                return msg.getMiniContent();
            }

            return "";
        }

        /// <summary>
        /// 初始化消息，如果_isDealing为true，则不初始化，直接添加第一句消息
        /// </summary>
        /// <param name="_addFriendTime"></param>
        /// <param name="_isDealing"></param>
        public void init(long _addFriendTime, bool _isDealing)
        {
            if (_m_isInited)
                return;
            _initConsortChatDialogueSentences(_addFriendTime, _isDealing);
            _m_isInited = true;
        }

        private void _initConsortChatDialogueSentences(long _addFriendTime, bool _isDealing)
        {
            var consortChatDialogueSentenceRefCore = GRefdataCoreMgr.instance.consortChatDialogueSentenceRefCore;
            
            _m_msgList = new List<_AConsortChatMsgInfo>();
            _m_sentenceList = new List<ConsortChatDialogueSentenceRefObj>();
            
            if (_m_dialogueRefObj == null || consortChatDialogueSentenceRefCore == null)
            {
                return; // 没有对话或句子
            }
            _m_curMsgId = _m_triggerTimeMs + 1; // 对话的消息id从对话id开始 

            ConsortChatDialogueSentenceRefObj sentenceRefObj = consortChatDialogueSentenceRefCore.getRef(_m_dialogueRefObj.start_sentence_id);
            // 如果一个选项都还没有, 并且时间小于5条信息发送延迟，则认为是只收到了首条信息
            if (_m_options.Count <= 0 && _isDealing)
            {
                ConsortChatMsgSentenceInfo msg = new ConsortChatMsgSentenceInfo(sentenceRefObj, _m_curMsgId++);
                _addMsg(sentenceRefObj, msg);
                return;
            }
          
            while (sentenceRefObj != null)
            {
                _m_sentenceList.Add(sentenceRefObj);
                _m_msgList.Add(new ConsortChatMsgSentenceInfo(sentenceRefObj, _m_curMsgId ++));
                long nextSentenceId = sentenceRefObj.next_id;
                
                if (sentenceRefObj.response_sentence_list != null && sentenceRefObj.response_sentence_list.Count > 0)
                {
                    if (!_m_options.TryGetValue(sentenceRefObj.id, out long optionId))
                    {
                        if(_m_hadDraw)
                            optionId = sentenceRefObj.response_sentence_list[0]; // 如果没有选项，则默认选择第一个
                        else
                            break;
                    }
                    ConsortChatDialogueSentenceRefObj optionRef = consortChatDialogueSentenceRefCore.getRef(optionId);
                    if (optionRef == null)
                        break;
                    _m_sentenceList.Add(sentenceRefObj);
                    _m_msgList.Add(new ConsortChatMsgResponseInfo(optionRef, _m_curMsgId ++));
                    nextSentenceId = optionRef.next_id;
                }

                if (nextSentenceId == -1 && _m_hadDraw)
                {
                    _m_msgList.Add(new ConsortChatMsgRewardInfo(this, _m_dialogueRefObj, _m_curMsgId ++, false));
                }
                // 获取下一句
                if (nextSentenceId <= 0)
                    break; // 没有下一句了
                
                sentenceRefObj = consortChatDialogueSentenceRefCore.getRef(nextSentenceId);
            }

        }
        
        
        /// <summary>
        /// 按照触发事件排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int sortDialogue(ConsortPresetChatDialogue a, ConsortPresetChatDialogue b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            return a._m_triggerTimeMs.CompareTo(b._m_triggerTimeMs);
        }
    }
}