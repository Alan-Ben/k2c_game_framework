using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 私聊对象个数的本地保存
    /// </summary>
    public class HistoryAccountSaver : _AALBasicSettingInfo
    {
        //会话列表
        private Dictionary<string,string> _m_historyChatInfoTagDic;
        private Dictionary<string,long> _m_historyChatReadTimeDic;
        private Dictionary<long,long> _m_historyRoomChatReadTimeDic;
        private List<string> _m_historyUpToTopList;//置顶列表
        private Dictionary<string,long> _m_historyChatRemoveTimeDic;

        //分隔符
        private const char _m_typeSplit = '&';
        private const char _m_dataSplit = '|';

        private const char _m_keyValueSplit = ':';
        public HistoryAccountSaver(long _accountCid)
            : base(string.Format("{0}_cache_account_chat_history", _accountCid))
        {
            _m_historyChatInfoTagDic = new Dictionary<string, string>();
            _m_historyChatRemoveTimeDic = new Dictionary<string, long>();
            _m_historyChatReadTimeDic = new Dictionary<string, long>();
            _m_historyRoomChatReadTimeDic = new Dictionary<long, long>();
            _m_historyUpToTopList = new List<string>();
        }

        public Dictionary<string, string> historyChatInfoTagDic { get { return _m_historyChatInfoTagDic; } }
        public Dictionary<string, long> historyChatRemoveTimeDic { get { return _m_historyChatRemoveTimeDic; } }
        public Dictionary<string, long> historyChatReadTimeDic { get { return _m_historyChatReadTimeDic; } }
        public Dictionary<long, long> historyRoomChatReadTimeDic { get { return _m_historyRoomChatReadTimeDic; } }
        
        
        public List<string> historyUpToTopList { get { return _m_historyUpToTopList; } }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int forCount = 0;
            foreach (KeyValuePair<string, string> kv in _m_historyChatInfoTagDic)
            {
                forCount++;
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);
                if (forCount != _m_historyChatInfoTagDic.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_typeSplit);
            if(_m_historyChatReadTimeDic.Count > 0)
            {
                forCount = 0;
                foreach (KeyValuePair<string, long> kv in _m_historyChatReadTimeDic)
                {
                    forCount++;
                    sb.Append(kv.Key);
                    sb.Append(_m_keyValueSplit);
                    sb.Append(kv.Value);
                    if (forCount != _m_historyChatReadTimeDic.Count)
                        sb.Append(_m_dataSplit);
                }
            }

            sb.Append(_m_typeSplit);
            if (_m_historyUpToTopList.Count > 0)
            {
                forCount = 0;
                foreach (string _key in _m_historyUpToTopList)
                {
                    forCount++;
                    sb.Append(_key);
                    if (forCount != _m_historyUpToTopList.Count)
                        sb.Append(_m_dataSplit);
                }
            }
            
            sb.Append(_m_typeSplit);
            if(_m_historyChatRemoveTimeDic.Count > 0)
            {
                forCount = 0;
                foreach (KeyValuePair<string, long> kv in _m_historyChatRemoveTimeDic)
                {
                    forCount++;
                    sb.Append(kv.Key);
                    sb.Append(_m_keyValueSplit);
                    sb.Append(kv.Value);
                    if (forCount != _m_historyChatRemoveTimeDic.Count)
                        sb.Append(_m_dataSplit);
                }
            }
            
            sb.Append(_m_typeSplit);
            if(_m_historyRoomChatReadTimeDic.Count > 0)
            {
                forCount = 0;
                foreach (KeyValuePair<long, long> kv in _m_historyRoomChatReadTimeDic)
                {
                    forCount++;
                    sb.Append(kv.Key);
                    sb.Append(_m_keyValueSplit);
                    sb.Append(kv.Value);
                    if (forCount != _m_historyRoomChatReadTimeDic.Count)
                        sb.Append(_m_dataSplit);
                }
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

            _m_historyChatInfoTagDic.Clear();
            _m_historyChatReadTimeDic.Clear();
            _m_historyUpToTopList.Clear();
            _m_historyChatRemoveTimeDic.Clear();
            _m_historyRoomChatReadTimeDic.Clear();
            try
            {
                string[] typeStrs = _infoStr.Split(_m_typeSplit);
                string[] dataStrs = null;
                if (typeStrs.Length > 0)
                {
                    dataStrs = typeStrs[0].Split(_m_dataSplit);
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                        string key = kvStrs[0];
                        string value = kvStrs[1];

                        _m_historyChatInfoTagDic.Add(key, value);
                    }
                }
                
                if (typeStrs.Length > 1)
                {
                    dataStrs = typeStrs[1].Split(_m_dataSplit);
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                        string key = kvStrs[0];
                        long value = long.Parse(kvStrs[1]);

                        _m_historyChatReadTimeDic.Add(key, value);
                    }
                }
                if (typeStrs.Length > 2)
                {
                    dataStrs = typeStrs[2].Split(_m_dataSplit);
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        string key = dataStrs[i];

                        _m_historyUpToTopList.Add(key);
                    }
                }
                
                
                if (typeStrs.Length > 3)
                {
                    dataStrs = typeStrs[3].Split(_m_dataSplit);
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                        string key = kvStrs[0];
                        long value = long.Parse(kvStrs[1]);

                        _m_historyChatRemoveTimeDic.Add(key, value);
                    }
                }
                if (typeStrs.Length > 4)
                {
                    dataStrs = typeStrs[4].Split(_m_dataSplit);
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                        long key = long.Parse(kvStrs[0]);
                        long value = long.Parse(kvStrs[1]);

                        _m_historyRoomChatReadTimeDic.Add(key, value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("HistoryAccountSaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_historyChatInfoTagDic.Clear();
                _m_historyChatReadTimeDic.Clear();
                _m_historyUpToTopList.Clear();
                _m_historyChatRemoveTimeDic.Clear();
                _m_historyRoomChatReadTimeDic.Clear();
            }
        }
        
        /// <summary>
        /// 添加置顶
        /// </summary>
        /// <param name="_chatInfoTag"></param>
        public bool addUpToTop(string _chatInfoTag)
        {
            
            if (_m_historyUpToTopList.Contains(_chatInfoTag))
            {
                return false;
            }

            if (_m_historyUpToTopList.Count >= GRefdataCoreMgr.instance.npGeneral.chat_private_up_to_top_limit)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_private_up_to_top_max));
                return false;
            }
            
            _m_historyUpToTopList.Add(_chatInfoTag);

            saveSetting();
            return true;
        }

        /// <summary>
        /// 移除置顶
        /// </summary>
        /// <param name="_chatInfoTag"></param>
        public void removeUpToTop(string _chatInfoTag)
        {
            if (_m_historyUpToTopList.Contains(_chatInfoTag))
            {
                _m_historyUpToTopList.Remove(_chatInfoTag);
            }
            saveSetting();
        }

        public void addChatInfoTag(string _chatInfoTag,string _otherUserTag)
        {
            if (!_m_historyChatInfoTagDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatInfoTagDic.Add(_chatInfoTag, _otherUserTag);
            }

            saveSetting();
        }

        public void removeChatInfoTag(string _chatInfoTag)
        {
            if (_m_historyChatInfoTagDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatInfoTagDic.Remove(_chatInfoTag);
            }
            saveSetting();
        }
        
        public void addChatReadTimeTag(string _chatInfoTag,long _readTime)
        {
            if (!_m_historyChatReadTimeDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatReadTimeDic.Add(_chatInfoTag, _readTime);
            }
            else
            {
                _m_historyChatReadTimeDic[_chatInfoTag] = _readTime;
            }

            saveSetting();
        }

        public void removeChatReadTimeTag(string _chatInfoTag)
        {
            if (_m_historyChatReadTimeDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatReadTimeDic.Remove(_chatInfoTag);
            }
            saveSetting();
        }
        public void addRoomChatReadTimeTag(long _chatId,long _readTime)
        {
            if (!_m_historyRoomChatReadTimeDic.ContainsKey(_chatId))
            {
                _m_historyRoomChatReadTimeDic.Add(_chatId, _readTime);
            }
            else
            {
                _m_historyRoomChatReadTimeDic[_chatId] = _readTime;
            }

            saveSetting();
        }

        public void removeRoomChatReadTimeTag(long _chatId)
        {
            if (_m_historyRoomChatReadTimeDic.ContainsKey(_chatId))
            {
                _m_historyRoomChatReadTimeDic.Remove(_chatId);
            }
            saveSetting();
        }

        /// <summary>
        /// 添加聊天频道删除时间戳
        /// </summary>
        /// <param name="_chatInfoTag"></param>
        /// <param name="_removeTime"></param>
        public void addChatRemoveTimeTag(string _chatInfoTag,long _removeTime)
        {
            if (!_m_historyChatRemoveTimeDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatRemoveTimeDic.Add(_chatInfoTag, _removeTime);
            }
            else
            {
                _m_historyChatRemoveTimeDic[_chatInfoTag] = _removeTime;
            }

            saveSetting();
        }

        /// <summary>
        /// 移除聊天频道删除时间戳
        /// </summary>
        /// <param name="_chatInfoTag"></param>
        public void removeChatRemoveTimeTag(string _chatInfoTag)//
        {
            if (_m_historyChatRemoveTimeDic.ContainsKey(_chatInfoTag))
            {
                _m_historyChatRemoveTimeDic.Remove(_chatInfoTag);
            }
            saveSetting();
        }
    }
}