using System;
using ALPackage;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using Common;
using JetBrains.Annotations;


namespace GOE
{
    /// <summary>
    /// 朋友圈记录的本地保存
    /// </summary>
    public class ConsortPresetChatSaver : _AALBasicSettingInfo
    {
        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';

        private readonly long _m_accountCID;
        [NotNull] private Dictionary<long, long> _m_consortLastestMsgTimeDic;
        
        public ConsortPresetChatSaver(long _accountCID)
            : base($"{_accountCID}_cache_account_consort_preset_chat_mgr")
        {
            _m_consortLastestMsgTimeDic = new Dictionary<long, long>();
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

            foreach (var kvp in _m_consortLastestMsgTimeDic)
            {
                listCount++;
                sb.Append(kvp.Key).Append(':').Append(kvp.Value);
                if (listCount != _m_consortLastestMsgTimeDic.Count)
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

            _m_consortLastestMsgTimeDic.Clear();
            
            try
            {
                string[] fieldStrs = _infoStr.Split(_m_fieldSplit);
                if (fieldStrs.Length > 0 && !string.IsNullOrEmpty(fieldStrs[0]))
                {
                    string[] dataStrs = fieldStrs[0].Split(_m_dataSplit);
                    foreach (string value in dataStrs)
                    {
                        string[] keyValuePair = value.Split(':');
                        if (keyValuePair.Length == 2)
                        {
                            long consortId = ALCommon.ParseLong(keyValuePair[0]);
                            int count = ALCommon.ParseInt(keyValuePair[1]);
                            if (consortId != 0)
                            {
                                _m_consortLastestMsgTimeDic[consortId] = count;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("HistorySaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_consortLastestMsgTimeDic.Clear();
            }
        }

        public void addConsortLatestMsgTime(long _consort, long _timeMs)
        {
            _m_consortLastestMsgTimeDic[_consort] = _timeMs;
            NPPlayer.instance.consortChatComp?.sortPresetChatDataList();
        }
        
        public long getConsortLatestMsgTime(long _consort)
        {
            if (_m_consortLastestMsgTimeDic.ContainsKey(_consort))
                return _m_consortLastestMsgTimeDic[_consort];
            return 0;
        }
    }
}