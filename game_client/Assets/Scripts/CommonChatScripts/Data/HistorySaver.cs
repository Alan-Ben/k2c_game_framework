using System;
using ALPackage;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using JetBrains.Annotations;


namespace ChatPackage.Internal
{
    /// <summary>
    /// 单个人私聊记录的本地保存
    /// </summary>
    public class HistorySaver : _AALBasicSettingInfo
    {
        //聊天记录列表
        [NotNull] private readonly List<HistorySaverData> _m_historyDataList;
        private HashSet<long> _m_historyMsgIdList;
        private const char _m_dataSplit = '|';
        private readonly string _m_target;
        
        public HistorySaver(string _target)
            : base($"{_target}_cache_account_chat_history")
        {
            _m_historyDataList = new List<HistorySaverData>();
            _m_historyMsgIdList = new HashSet<long>();
            _m_target = _target;
        }

        public string target { get { return _m_target; } }

        public List<HistorySaverData> historyDataList { get { return _m_historyDataList; } }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int listCount = 0;

            foreach (HistorySaverData listValue in _m_historyDataList)
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
            _m_historyMsgIdList.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                foreach (string value in dataStrs)
                {
                    HistorySaverData data = null;
                    try
                    {
                        data = JsonUtility.FromJson<HistorySaverData>(value);
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
                _m_historyMsgIdList.Clear();
            }

        }

        public void addData(HistorySaverData _data)
        {
            if (_data == null)
                return;
            if(_m_historyMsgIdList.Contains(_data.msgId))
                return;
            _m_historyMsgIdList.Add(_data.msgId);
            // 按msgId排序插入，保持列表从旧到新
            int insertIndex = _m_historyDataList.Count;
            for (int i = _m_historyDataList.Count - 1; i >= 0; i--)
            {
                if (_m_historyDataList[i].msgId <= _data.msgId)
                {
                    insertIndex = i + 1;
                    break;
                }
                insertIndex = i;
            }
            _m_historyDataList.Insert(insertIndex, _data);
        }
    }
}