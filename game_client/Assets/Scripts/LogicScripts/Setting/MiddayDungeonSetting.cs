using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 午间宴会相关本地保存
    /// </summary>
    public class MiddayDungeonSetting : _AALBasicSettingInfo
    {
        private long _m_roundInstanceId;//保存的宴会邀请的实例id

        private Dictionary<long,long> _m_hasOpenList;//<cid,timeS>
        private Dictionary<long,long> _m_hasInvalidList;//<cid,timeS>

        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';

        public MiddayDungeonSetting(long _accountCID)
            : base(string.Format("{0}_cache_midday_dungeon_setting_gob", _accountCID))
        {
            _m_hasOpenList = new Dictionary<long,long>();
            _m_hasInvalidList = new Dictionary<long, long>();
        }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();

            // 清理已过期id
            List<long> removeList = new List<long>();
            foreach (var kv in _m_hasOpenList)
            {
                if (kv.Value < FpsAndPingMgr.instance.serverTimeTag)
                    removeList.Add(kv.Key);
            }

            foreach (var dbid in removeList)
            {
                _m_hasOpenList.Remove(dbid);
            }
            // 清理已过期id
            removeList.Clear();
            foreach (var kv in _m_hasInvalidList)
            {
                if (kv.Value < FpsAndPingMgr.instance.serverTimeTag)
                    removeList.Add(kv.Key);
            }
            foreach (var dbid in removeList)
            {
                _m_hasInvalidList.Remove(dbid);
            }
            
            sb.Append(_m_roundInstanceId);
            sb.Append(_m_fieldSplit);
            
            int forCount = 0;
            foreach (var kv in _m_hasOpenList)
            {
                forCount++;
                sb.Append(kv.Key);         
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);
                if (forCount != _m_hasOpenList.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);
            forCount = 0;
            foreach (var kv in _m_hasInvalidList)
            {
                forCount++;
                sb.Append(kv.Key);         
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);
                if (forCount != _m_hasInvalidList.Count)
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

            try
            {
                string[] fieldStrs = _infoStr.Split(_m_fieldSplit);
                if (fieldStrs.Length > 0)
                {
                    _m_roundInstanceId = ALCommon.ParseLong(fieldStrs[0]); 
                }
                if (fieldStrs.Length > 1)
                {
                    string[] dataStrs = fieldStrs[1].Split(_m_dataSplit);
                    _m_hasOpenList.Clear();
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        string[] keyStrs = dataStrs[i].Split(_m_keyValueSplit);
                        if(keyStrs.Length != 2)
                            continue;
                        long key = ALCommon.ParseLong(keyStrs[0]);
                        long value = ALCommon.ParseLong(keyStrs[1]);
                        _m_hasOpenList[key] = value;
                    }
                }
                if (fieldStrs.Length > 2)
                {
                    string[] dataStrs = fieldStrs[2].Split(_m_dataSplit);
                    _m_hasInvalidList.Clear();
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        string[] keyStrs = dataStrs[i].Split(_m_keyValueSplit);
                        if(keyStrs.Length != 2)
                            continue;
                        long key = ALCommon.ParseLong(keyStrs[0]);
                        long value = ALCommon.ParseLong(keyStrs[1]);
                        _m_hasInvalidList[key] = value;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"NPDailyTagSetting init has Exception:{e}, _infoStr:{_infoStr}");
            }
        }
        /// <summary>
        /// 获取保存的邀请时间
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public bool hasOpen(long _dbid)
        {
            if(_m_hasOpenList.ContainsKey(_dbid))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 设置邀请时间
        /// </summary>
        public void setHasOpen(long _dbid, long _expireTs)
        {
            // if (_m_roundInstanceId != _instanceId)
            // {
            //     _m_hasOpenList.Clear();
            //     _m_roundInstanceId = _instanceId;
            // }
            if(_m_hasOpenList.TryGetValue(_dbid, out long _timeS))
            {
                return ;
            }
            else
            {
                _m_hasOpenList[_dbid] = _expireTs;
            }

            saveSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public bool hasInValid(long _dbid)
        {
            if(_m_hasInvalidList.ContainsKey(_dbid))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void setHasInValid(long _dbid, long _expireTs)
        {
            // if (_m_roundInstanceId != _instanceId)
            // {
            //     _m_hasOpenList.Clear();
            //     _m_roundInstanceId = _instanceId;
            // }
            if(_m_hasInvalidList.TryGetValue(_dbid, out long _timeS))
            {
                return ;
            }
            else
            {
                _m_hasInvalidList[_dbid] = _expireTs;
            }

            saveSetting();
        }
    }
}