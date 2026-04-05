using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class PlayerInfoSaver : _AALBasicSettingInfo
    {
        // key:rankId  value:cidList
        [NotNull]public Dictionary<long, NPCommonSimplePlayerInfo> _m_playerInfoDic;
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = '!';

        public PlayerInfoSaver()
            : base(string.Format("cache_player_info"))
        {
            _m_playerInfoDic = new Dictionary<long, NPCommonSimplePlayerInfo>();
        }

        /*************
         * 构建需要保存的字符串
         **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int forCount = 0;
            foreach (KeyValuePair<long, NPCommonSimplePlayerInfo> kv in _m_playerInfoDic)
            {
                forCount++;
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                sb.Append(JsonUtility.ToJson(kv.Value));
                if (forCount != _m_playerInfoDic.Count)
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

            _m_playerInfoDic.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                for (int i = 0; i < dataStrs.Length; i++)
                {
                    string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                    long key = long.Parse(kvStrs[0]);
                    NPCommonSimplePlayerInfo value = JsonUtility.FromJson<NPCommonSimplePlayerInfo>(kvStrs[1]);

                    _m_playerInfoDic.Add(key, value);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("NPPlayerInfoSaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_playerInfoDic.Clear();
            }
        }

        //保存用户信息
        public void savePlayerInfo(NPCommonSimplePlayerInfo _playerInfo)
        {
            if(null == _playerInfo)
                return;
            
            NPCommonSimplePlayerInfo info = null;
            if (_m_playerInfoDic.TryGetValue(_playerInfo.cid, out info))
            {
                info = _playerInfo;
            }
            else
            {
                //todo 
                if (_m_playerInfoDic.Count >= 100)
                {
                    return;
                }
                _m_playerInfoDic.TryAdd(_playerInfo.cid, _playerInfo);
            }

            saveSetting();
        }

        //获取玩家信息
        public NPCommonSimplePlayerInfo getPlayerInfo(long _cid)
        {
            NPCommonSimplePlayerInfo info = null;
            _m_playerInfoDic.TryGetValue(_cid, out info);
            return info;
        }
    }
}
