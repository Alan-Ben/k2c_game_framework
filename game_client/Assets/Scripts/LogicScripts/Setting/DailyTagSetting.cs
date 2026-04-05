using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 每日标记本地保存
    /// </summary>
    public class DailyTagSetting : _AALBasicSettingInfo
    {
        private Dictionary<long, string> _m_dDataDic;//<NPDailyTagConst,日期>
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';

        public DailyTagSetting(long _accountCID)
            : base(string.Format("{0}_cache_daily_tag_setting", _accountCID))
        {
            _m_dDataDic = new Dictionary<long, string>();
        }


        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int forCount = 0;
            foreach (KeyValuePair<long, string> kv in _m_dDataDic)
            {
                forCount++;
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);
                if (forCount != _m_dDataDic.Count)
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

            _m_dDataDic.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                for (int i = 0; i < dataStrs.Length; i++)
                {
                    string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                    long key = ALCommon.ParseLong(kvStrs[0]);
                    _m_dDataDic[key] = kvStrs[1];
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"NPDailyTagSetting init has Exception:{e}, _infoStr:{_infoStr}");
                _m_dDataDic.Clear();
            }
        }

        /// <summary>
        /// 获取保存的标记时间
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string getSaveTag(long _key)
        {
            _m_dDataDic.TryGetValue(_key, out string value);
            return value;
        }

        /// <summary>
        /// 是否是新的一天
        /// </summary>
        /// <param name="_key">NPDailyTagConst常量</param>
        /// <returns></returns>
        public bool isNewDay(long _key)
        {
            if (!_m_dDataDic.ContainsKey(_key))
                return true;

            DateTime serverDateTime = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);
            return _m_dDataDic[_key] != serverDateTime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 设置记录为今天
        /// </summary>
        /// <param name="_key">NPDailyTagConst常量</param>
        public void setSaveToday(long _key)
        {
            DateTime serverDateTime = TimeUtil.FromUTCMilliseconds(FpsAndPingMgr.instance.serverTimeTag);
            if (_m_dDataDic.ContainsKey(_key) && _m_dDataDic[_key] == serverDateTime.ToString("yyyyMMdd"))
                return;
            
            _m_dDataDic[_key] = serverDateTime.ToString("yyyyMMdd");
            saveSetting();
        }

        /// <summary>
        /// 设置记录为目标时间
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_tag"></param>
        public void setSaveTag(long _key, string _tag)
        {
            if (_m_dDataDic.ContainsKey(_key) && _m_dDataDic[_key] == _tag)
                return;

            _m_dDataDic[_key] = _tag;
            saveSetting();
        }

    }
}