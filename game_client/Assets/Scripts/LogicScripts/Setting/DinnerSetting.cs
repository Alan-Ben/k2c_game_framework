using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 宴会相关本地保存
    /// </summary>
    public class DinnerSetting : _AALBasicSettingInfo
    {
        private long _m_quickJoinCostId;// 保存的快速赴宴消耗id
        
        private List<long> _m_hasInviteList;//<cid,timeS>
        private long _m_dinnerInviteInstanceId;//保存的宴会邀请的实例id
        private long _m_HasReqServerInviteListInstanceId;//保存的宴会邀请的实例id
        private List<long> _m_serverInviteCidList;// 宴会推荐邀请玩家列表
        private long _m_lastServerShareTimeMs;
        private long _m_lastGuildShareTimeMs;

        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';

        public long lastServerShareTimeMs
        {
            get
            {
                return _m_lastServerShareTimeMs;
            }
            set
            {
                _m_lastServerShareTimeMs = value;
                saveSetting();
            }
        }
        public long lastGuildShareTimeMs
        {
            get
            {
                return _m_lastGuildShareTimeMs;
            }
            set
            {
                _m_lastGuildShareTimeMs = value;
                saveSetting();
            }
        }

        public DinnerSetting(long _accountCID)
            : base(string.Format("{0}_cache_dinner_setting_gob_v2", _accountCID))
        {
            _m_hasInviteList = new List<long>();
            _m_serverInviteCidList = new List<long>();
        }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(_m_quickJoinCostId);
            sb.Append(_m_fieldSplit);
            
            sb.Append(_m_dinnerInviteInstanceId);
            sb.Append(_m_fieldSplit);
            
            int forCount = 0;
            foreach (long kv in _m_hasInviteList)
            {
                forCount++;
                sb.Append(kv);
                if (forCount != _m_hasInviteList.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);
            sb.Append(_m_HasReqServerInviteListInstanceId);
            
            sb.Append(_m_fieldSplit);
            forCount = 0;
            foreach (long cid in _m_serverInviteCidList)
            {
                forCount++;
                sb.Append(cid);
                if (forCount != _m_serverInviteCidList.Count)
                    sb.Append(_m_dataSplit);
            }
            sb.Append(_m_fieldSplit);
            sb.Append(_m_lastServerShareTimeMs);
            sb.Append(_m_fieldSplit);
            sb.Append(_m_lastGuildShareTimeMs);

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
                int strIndex = 0;
                if (fieldStrs.Length > strIndex)
                {
                    _m_quickJoinCostId = ALCommon.ParseLong(fieldStrs[strIndex]); 
                }

                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    _m_dinnerInviteInstanceId = ALCommon.ParseLong(fieldStrs[strIndex]); 
                }
                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    string[] dataStrs = fieldStrs[strIndex].Split(_m_dataSplit);
                    _m_hasInviteList.Clear();
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        long key = ALCommon.ParseLong(dataStrs[i]);
                        _m_hasInviteList.Add(key);
                    }
                } 
                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    _m_HasReqServerInviteListInstanceId = ALCommon.ParseLong(fieldStrs[strIndex]); 
                }
                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    string[] dataStrs = fieldStrs[strIndex].Split(_m_dataSplit);
                    _m_serverInviteCidList.Clear();
                    for (int i = 0; i < dataStrs.Length; i++)
                    {
                        if(string.IsNullOrEmpty(dataStrs[i]))
                            continue;
                        long cid = ALCommon.ParseLong(dataStrs[i]);
                        _m_serverInviteCidList.Add(cid);
                    }
                } 
                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    _m_lastServerShareTimeMs = ALCommon.ParseLong(fieldStrs[strIndex]); 
                }
                strIndex++;
                if (fieldStrs.Length > strIndex)
                {
                    _m_lastGuildShareTimeMs = ALCommon.ParseLong(fieldStrs[strIndex]); 
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
        public bool hasInvite(long _dinnerInstanceId ,long _key)
        {
            if (_m_dinnerInviteInstanceId != _dinnerInstanceId)
                return false;
            foreach (long cid in _m_hasInviteList)
            {
                if (cid == _key)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 设置邀请时间
        /// </summary>
        /// <param name="_key">NPDailyTagConst常量</param>
        public void setInvite(long _dinnerInstanceId, long _key)
        {
            if (_m_dinnerInviteInstanceId != _dinnerInstanceId)
            {
                _m_hasInviteList.Clear();
                _m_dinnerInviteInstanceId = _dinnerInstanceId;
            }
            if (!_m_hasInviteList.Contains(_key))
            {
                _m_hasInviteList.Add(_key);
            }
            saveSetting();
        }
        
        /// <summary>
        /// 是否开启快速赴宴
        /// </summary>
        public bool isQuickJoin()
        {
            return _m_quickJoinCostId > 0;
        }
        
        /// <summary>
        /// 获取快速赴宴id
        /// </summary>
        public long getQuickJointCostId()
        {
            return _m_quickJoinCostId;
        }
        /// <summary>
        /// 设置快速赴宴id
        /// </summary>
        public void setQuickJointCostId(long _costId)
        {
            _m_quickJoinCostId = _costId;
            saveSetting();
        }

        public void setReqServerInviteListInstanceId(long _instanceId, List<long> _cidList)
        {
            if(_cidList == null)
                return;

            _m_HasReqServerInviteListInstanceId = _instanceId;
            _m_serverInviteCidList = _cidList;

            saveSetting();
        }

        public bool isHasReqServerInviteList(long _instanceId)
        {
            return _m_HasReqServerInviteListInstanceId == _instanceId;
        }

        public List<long> getServerInviteCidList(long _instanceId)
        {
            return _m_serverInviteCidList;
        }
        
    }
}