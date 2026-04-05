using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using Common;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine.Serialization;


namespace GOE
{
    //存储的消息结构
    [Serializable]
    public class ConsortAIChatSimpleData
    {
        // 妃子id
        public long consortId;
        public bool hasUnRead = false; // 是否有未读消息
        public long lastMsgTime = 0;
        public string lastContent = string.Empty; // 最后消息内容
    }
    
    /// <summary>
    /// 朋友圈记录的本地保存
    /// </summary>
    public class ConsortAIChatSaverMgr : _AALBasicSettingInfo
    {
        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';

        private readonly long _m_accountCID;
        // 简化的聊天记录数据，初始化就会加载
        [NotNull] private List<ConsortAIChatSimpleData> _m_consortAIChatSimpleDataList;
        // 简化数据的字典索引，提高查找效率
        [NotNull] private Dictionary<long, ConsortAIChatSimpleData> _m_consortSimpleDataDic;
        // 详细聊天记录,需要时才会加载
        [NotNull] private readonly Dictionary<long, ConsortAIChatSaver> _m_aiChatDetailSaverDic;
        
        // AI首次发送消息的时间标签列表，用于统计今日AI首次消息数量
        [NotNull] private List<long> _m_consortAIFirstMsgTagList;
        
        public ConsortAIChatSaverMgr(long _accountCID)
            : base($"{_accountCID}_cache_account_consort_ai_chat_history_mgr")
        {
            _m_consortAIChatSimpleDataList = new List<ConsortAIChatSimpleData>();
            _m_consortSimpleDataDic = new Dictionary<long, ConsortAIChatSimpleData>();
            _m_aiChatDetailSaverDic = new Dictionary<long, ConsortAIChatSaver>();
            _m_consortAIFirstMsgTagList = new List<long>();
            _m_accountCID = _accountCID;
        }
        
        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int listCount = 0;

            // 保存简化的聊天记录数据
            foreach (ConsortAIChatSimpleData simpleData in _m_consortAIChatSimpleDataList)
            {
                listCount++;
                sb.Append(JsonUtility.ToJson(simpleData));
                if (listCount != _m_consortAIChatSimpleDataList.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);
            
            // 保存AI首次消息时间标签列表
            listCount = 0;
            foreach (long timeTag in _m_consortAIFirstMsgTagList)
            {
                listCount++;
                sb.Append(timeTag);
                if (listCount != _m_consortAIFirstMsgTagList.Count)
                    sb.Append(_m_dataSplit);
            }
           
            return sb.ToString();
        }

        /**************
        * 读取保存的字符串
        **/
        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            _m_consortAIChatSimpleDataList.Clear();
            _m_consortSimpleDataDic.Clear();
            _m_consortAIFirstMsgTagList.Clear();
            
            try
            {
                string[] fieldStrs = _infoStr.Split(_m_fieldSplit);
                
                // 加载简化的聊天记录数据
                if (fieldStrs.Length > 0)
                {
                    string[] dataStrs = fieldStrs[0].Split(_m_dataSplit);
                    ConsortAIChatSimpleData tmpData;
                    foreach (string value in dataStrs)
                    {
                        tmpData = JsonUtility.FromJson<ConsortAIChatSimpleData>(value);
                        _m_consortAIChatSimpleDataList.Add(tmpData);
                        _m_consortSimpleDataDic[tmpData.consortId] = tmpData; // 同时添加到字典中
                    }
                }
                
                // 加载AI首次消息时间标签列表
                if (fieldStrs.Length > 1 && !string.IsNullOrEmpty(fieldStrs[1]))
                {
                    string[] dataStrs = fieldStrs[1].Split(_m_dataSplit);
                    foreach (string value in dataStrs)
                    {
                        long timeTag = ALCommon.ParseLong(value);
                        if(timeTag == 0)
                            continue;
                        _m_consortAIFirstMsgTagList.Add(timeTag);
                    }
                }
                
                _clearAIFirstMsgNotTodayTag();
                saveSetting();
                // 更新红点状态
                _updateRedTip();

            }
            catch (Exception e)
            {
                Debug.LogError("HistorySaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_consortAIChatSimpleDataList.Clear();
                _m_consortSimpleDataDic.Clear();
                _m_consortAIFirstMsgTagList.Clear();
            }

        }

        private void _updateRedTip()
        {
            int count = 0;
            foreach (var simpleData in _m_consortAIChatSimpleDataList)
            {
                if (simpleData != null && simpleData.hasUnRead)
                {
                    count++;
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CONSORT_CHAT_AI_PAGE, count);
        }
        
        private ConsortAIChatSaver _getAiChatSaver(long _consortId)
        {
            _m_aiChatDetailSaverDic.TryGetValue(_consortId, out ConsortAIChatSaver saver);

            if(saver == null)
            {
                saver = new ConsortAIChatSaver(NPPlayer.instance.playerInfo.CID, _consortId);
                saver.init();
                _m_aiChatDetailSaverDic.Add(_consortId, saver);
            }

            return saver;
        }

        /// <summary>
        /// 获取或创建简化数据
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        private ConsortAIChatSimpleData _getOrCreateSimpleData(long _consortId)
        {
            if (_m_consortSimpleDataDic.TryGetValue(_consortId, out ConsortAIChatSimpleData simpleData))
            {
                return simpleData;
            }

            // 创建新的简化数据
            simpleData = new ConsortAIChatSimpleData();
            simpleData.consortId = _consortId;
            _m_consortAIChatSimpleDataList.Add(simpleData);
            _m_consortSimpleDataDic[_consortId] = simpleData;
            return simpleData;
        }
        /// <summary>
        /// 是否有未读消息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public bool hasUnRead(long _consortId)
        {
            return _m_consortSimpleDataDic.TryGetValue(_consortId, out ConsortAIChatSimpleData simpleData) && simpleData.hasUnRead;
        }
        /// <summary>
        /// 设置为已读
        /// </summary>
        public void setRead(long _consortId)
        {
            if (_m_consortSimpleDataDic.TryGetValue(_consortId, out ConsortAIChatSimpleData simpleData))
            {
                simpleData.hasUnRead = false;
                // 更新红点状态
                _updateRedTip();
                saveSetting();
                NPPlayer.instance.consortChatComp.refreshPresetChatRed();
            }
        }
       
        /// <summary>
        /// 获取妃子AI聊天的最后一条消息内容
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public ConsortAIChatSimpleData lastAISimpleData(long _consortId)
        {
            return _m_consortSimpleDataDic.TryGetValue(_consortId, out ConsortAIChatSimpleData simpleData) ? simpleData : null;
        }
        
        /// <summary>
        /// 获取妃子AI聊天的最后一条消息内容
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public string lastContent(long _consortId)
        {
            return _m_consortSimpleDataDic.TryGetValue(_consortId, out ConsortAIChatSimpleData simpleData) ? simpleData.lastContent : "";
        }
        
        /// <summary>
        /// 是否已经添加过消息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public bool hasAddedMsg(long _consortId)
        {
            return _m_consortSimpleDataDic.ContainsKey(_consortId);
        }

        public long lastAIFirstMsgTag()
        {
            _m_consortAIFirstMsgTagList.Sort();
            return _m_consortAIFirstMsgTagList.GetLast();
        }

        public int todayAIFirstCount()
        {
            return _m_consortAIFirstMsgTagList.Count;
        }

        private void _clearAIFirstMsgNotTodayTag()
        {
            DateTime dateTime1 = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);
            DateTime todayDayTime = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 0, 0, 0, DateTimeKind.Utc);
            long today = TimeUtil.dateTime2Milliseconds(todayDayTime);
            for (int i = _m_consortAIFirstMsgTagList.Count - 1; i >= 0; i--)
            {
                if (_m_consortAIFirstMsgTagList[i] < today)
                {
                    _m_consortAIFirstMsgTagList.RemoveAt(i);
                }
            }
        }

        public void addAIFirstMsg(long _msgTimeTag)
        {
            _m_consortAIFirstMsgTagList.Add(_msgTimeTag);
            _clearAIFirstMsgNotTodayTag();
            saveSetting(); // 保存到本地
        }

        /// <summary>
        /// 添加妃子发送的新消息，标记有新未读，并且更新最后一条消息内容
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_content"></param>
        public ConsortAISaverMsgData addConsortSendNewMsg(long _msgServerTimeTag, long _consortId, string _content)
        {
            ConsortAISaverMsgData data = new ConsortAISaverMsgData(_msgServerTimeTag, false, _content);

            // 使用优化后的方法获取或创建简化数据
            ConsortAIChatSimpleData consortSimpleData = _getOrCreateSimpleData(_consortId);
            consortSimpleData.lastContent = _content;
            consortSimpleData.hasUnRead = true;
            consortSimpleData.lastMsgTime = _msgServerTimeTag;
            
            ConsortAIChatSaver chatSaver = _getAiChatSaver(_consortId);
            if (chatSaver != null)
            {
                chatSaver.addData(data);
                chatSaver.saveSetting();
            }
            // 更新红点状态
            _updateRedTip();
            saveSetting();
            return data;
        }
        
        /// <summary>
        /// 添加玩家发送给妃子的新消息
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_content"></param>
        public ConsortAISaverMsgData addPlayerSendNewMsg(long _consortId, string _content)
        {
            ConsortAISaverMsgData data = new ConsortAISaverMsgData(FpsAndPingMgr.instance.serverTimeTag, true, _content);
            ConsortAIChatSaver chatSaver = _getAiChatSaver(_consortId);
            if (chatSaver != null)
            {
                chatSaver.addData(data);
                chatSaver.saveSetting();
            }
            return data;
        }
        
        /// <summary>
        /// 获取妃子AI聊天的历史消息列表
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public List<ConsortAISaverMsgData> getHistoryMsgList(long _consortId)
        {
            ConsortAIChatSaver chatSaver = _getAiChatSaver(_consortId);
            if (chatSaver != null)
            {
                return chatSaver.historyDataList;
            }
            return new List<ConsortAISaverMsgData>();
        }

        /// <summary>
        /// 获取请求ai回复的消息列表，从旧到新
        /// </summary>
        /// <param name="consortId"></param>
        /// <returns></returns>
        public List<Common_AiChatMessage> getReqAIChatMsgList(long consortId)
        {
            List<Common_AiChatMessage> msgList = new List<Common_AiChatMessage>();
            ConsortAIChatSaver chatSaver = _getAiChatSaver(consortId);
            if (chatSaver != null)
            {
                int msgCount = GRefdataCoreMgr.instance.npGeneral.consort_chat_ai_send_max_msg_count;
                for (int i = chatSaver.historyDataList.Count - 1; i >= 0; i--)
                {
                    if(msgCount <= 0)
                        break;
                    ConsortAISaverMsgData msgData = chatSaver.historyDataList[i];
                    if (msgData == null) continue;


                    var headMsg = msgList.Count > 0 ? msgList[0] : null;
                    if (headMsg != null && headMsg.getRoleType() == EAiChatRoleType.SYSTEM  && !msgData.senderIsPlayer)
                    {
                        // 如果是连续妃子的消息，则只用最后一条作为上下文
                        headMsg.setMsg(msgData.gameContent);
                    }
                    else if (headMsg != null && headMsg.getRoleType() == EAiChatRoleType.USER &&
                             msgData.senderIsPlayer)
                    {
                        // 如果是玩家发多条消息，则合并消息
                        headMsg.setMsg(msgData.gameContent + "\n" + headMsg.getMsg());
                    }
                    else
                    {
                        msgList.Insert(0, new Common_AiChatMessage(
                            msgData.senderIsPlayer ? CommonEnum.EAiChatRoleType.USER : CommonEnum.EAiChatRoleType.SYSTEM,
                            msgData.gameContent));
                    }
                    
              
                    
                    msgCount --;
                }
            }

            return msgList;
        }
    }
}