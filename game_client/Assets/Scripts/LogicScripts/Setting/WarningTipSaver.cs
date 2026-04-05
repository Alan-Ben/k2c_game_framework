using ALPackage;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOE
{
    public class WarningTipSaver : _AALBasicSettingInfo
    {
        // key = 警告类型，value = 无视警告的日期
        private Dictionary<ENPWarningType, string> _m_dWarningDic;
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';

        public WarningTipSaver(long _accountCID)
            : base(string.Format("{0}_warning_tip", _accountCID))
        {
            _m_dWarningDic = new Dictionary<ENPWarningType, string>();
        }

        /*************
         * 构建需要保存的字符串
         **/
        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();
            int forCount = 0;
            foreach (KeyValuePair<ENPWarningType, string> kv in _m_dWarningDic)
            {
                forCount++;
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);
                if (forCount != _m_dWarningDic.Count)
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

            _m_dWarningDic.Clear();
            try
            {
                string[] dataStrs = _infoStr.Split(_m_dataSplit);
                for (int i = 0; i < dataStrs.Length; i++)
                {
                    string[] kvStrs = dataStrs[i].Split(_m_keyValueSplit);
                    ENPWarningType key = (ENPWarningType)Enum.Parse(typeof(ENPWarningType), kvStrs[0]);
                    _m_dWarningDic.Add(key, kvStrs[1]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("NPWarningTipSaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                _m_dWarningDic.Clear();
            }
        }

        /// <summary>
        /// 今日是否需要显示警告提示
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public bool needShowWarningTip(ENPWarningType _key)
        {
            if (!_m_dWarningDic.ContainsKey(_key))
                return true;

            return _m_dWarningDic[_key] != System.DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 设置今日不再提示
        /// </summary>
        /// <param name="_key"></param>
        public void setTodayIgnoreWarningTip(ENPWarningType _key)
        {
            _m_dWarningDic[_key] = System.DateTime.Now.ToString("yyyyMMdd");
            saveSetting();
        }
    }
}
