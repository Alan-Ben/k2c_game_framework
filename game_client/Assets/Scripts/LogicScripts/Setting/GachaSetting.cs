using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 抽卡相关本地保存
    /// </summary>
    public class GachaSetting : _AALBasicSettingInfo
    {
        [NotNull] private Dictionary<long, bool> _m_dGachaSkipShowDic = new Dictionary<long, bool>();//<gachaId,是否显示跳过>
        
        private const char _m_fieldSplit = '&';
        private const char _m_dataSplit = '|';
        private const char _m_keyValueSplit = ':';

        public GachaSetting(long _accountCID)
            : base(string.Format("{0}_cache_gacha_setting", _accountCID))
        {
        }

        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();

            // 序列化_m_dGachaSkipShowDic
            foreach (var kv in _m_dGachaSkipShowDic)
            {
                sb.Append(kv.Key);
                sb.Append(_m_keyValueSplit);
                sb.Append(kv.Value);

                sb.Append(_m_dataSplit);
            }
            
            return sb.ToString();
        }

        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;

            string[] fieldStrArray = _infoStr.Split(_m_fieldSplit);
            if(fieldStrArray == null)
                return;

            // 解析_m_dGachaSkipShowDic
            _m_dGachaSkipShowDic.Clear();
            if (fieldStrArray.Length >= 1 && !string.IsNullOrEmpty(fieldStrArray[0]))
            {
                string[] gachaSkipShowDataStrArray = fieldStrArray[0].Split(_m_dataSplit, StringSplitOptions.RemoveEmptyEntries);
                if (gachaSkipShowDataStrArray != null)
                {
                    foreach (var gachaSkipShowDataStr in gachaSkipShowDataStrArray)
                    {
                        if(string.IsNullOrEmpty(gachaSkipShowDataStr))
                            continue;

                        string[] kvStr = gachaSkipShowDataStr.Split(_m_keyValueSplit, StringSplitOptions.RemoveEmptyEntries);
                        if (kvStr == null || kvStr.Length < 2 || string.IsNullOrEmpty(kvStr[0]) || string.IsNullOrEmpty(kvStr[1]))
                        {
                            Debug.LogError_EditorOnly($"[GachaSetting] 解析_gachaSkipShowDataStr某数据存在错误, gachaSkipShowDataStr:{gachaSkipShowDataStr}");
                            continue;
                        }

                        try
                        {
                            long gachaId = long.Parse(kvStr[0]);
                            bool needSkipShow = bool.Parse(kvStr[1]);
                            
                            _m_dGachaSkipShowDic.Add(gachaId, needSkipShow);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError_EditorOnly($"[GachaSetting] 解析_gachaSkipShowDataStr某数据存在错误, gachaSkipShowDataStr:{gachaSkipShowDataStr} e:{e}");
                        }
                    }
                }
            }
        }

        #region _m_dGachaSkipShowDic

        /// <summary>
        /// 设置是否跳过抽卡展示
        /// </summary>
        /// <param name="_gachaId"></param>
        /// <param name="_needSkipShow"></param>
        public void setGachaSkipShow(long _gachaId, bool _needSkipShow)
        {
            _m_dGachaSkipShowDic[_gachaId] = _needSkipShow;
            
            saveSetting();
        }

        /// <summary>
        /// 获取是否跳过抽卡展示
        /// </summary>
        /// <param name="_gachaId"></param>
        /// <returns></returns>
        public bool getGachaSkipShow(long _gachaId)
        {
            _m_dGachaSkipShowDic.TryGetValue(_gachaId, out bool _needSkipShow);
            return _needSkipShow;
        }

        #endregion
    }
}