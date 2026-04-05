using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using JetBrains.Annotations;


namespace GOE
{
    //存储的消息结构
    [Serializable]
    public class ConsortAISaverMsgData
    {
        // 私聊消息唯一id
        public long msgId;

        // 发送者信息
        public bool senderIsPlayer; // 是否是玩家发送的消息

        // 消息内容
        public string gameContent;

        public ConsortAISaverMsgData(long _msgId, bool _senderIsPlayer, string _content) {
            
            msgId = _msgId;
            senderIsPlayer = _senderIsPlayer;
            gameContent = _content;
            
        }
    }
    /// <summary>
    /// 单个人私聊记录的本地保存
    /// </summary>
    public class ConsortAIChatSaver : _AALBasicSettingInfo
    {
        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';
        private readonly long _m_accountCID;
        private readonly long _m_consortID;
        //聊天记录列表
        [NotNull] private readonly List<ConsortAISaverMsgData> _m_historyDataList;
        
        public ConsortAIChatSaver(long _accountCID, long _consortID)
            : base($"{_accountCID}_cache_account_consort_ai_chat_history_{_consortID}")
        {
            _m_historyDataList = new List<ConsortAISaverMsgData>();
            _m_accountCID = _accountCID;
            _m_consortID = _consortID;
        }

        public List<ConsortAISaverMsgData> historyDataList { get { return _m_historyDataList; } }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int listCount = 0;
            _checkMsgCount();
            foreach (ConsortAISaverMsgData listValue in _m_historyDataList)
            {
                listCount++;
                sb.Append(JsonUtility.ToJson(listValue));
                if (listCount != _m_historyDataList.Count)
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

            _m_historyDataList.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                foreach (string value in dataStrs)
                {
                    ConsortAISaverMsgData data = null;
                    try
                    {
                        data = JsonUtility.FromJson<ConsortAISaverMsgData>(value);
                    }
                    catch (Exception e)
                    {
#if UNITY_EDITOR
                        Debug.LogError($"私聊相关本地保存反序列化失败:{_infoStr};/n Exception:{e}");
#endif
                    }
                    addData(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("HistorySaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_historyDataList.Clear();
            }

        }

        /// <summary>
        /// 只保存200条消息
        /// </summary>
        private void _checkMsgCount()
        {
            int removeCount = _m_historyDataList.Count - 200;
            if(removeCount > 0)
                _m_historyDataList.RemoveRange(0, removeCount);
        }



        /// <summary>
        /// 增加消息
        /// </summary>
        /// <param name="_data"></param>
        public void addData(ConsortAISaverMsgData _data)
        {
            if(_data == null)
                return;
            _m_historyDataList.Add(_data);
            saveSetting();
        }
        
        /// <summary>
        /// 获取最后一条消息文本
        /// </summary>
        public string getLastMsgContent()
        {
            var lastMsg = _m_historyDataList.GetLast();
            if (lastMsg != null)
                return lastMsg.gameContent;
            return "";
        }
    }
}