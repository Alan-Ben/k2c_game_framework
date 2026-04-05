using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerChildComponent
    {
        public class ReadEngageRequestSaver : _AALBasicSettingInfo
        {
            private const char _k_dataSplit = '|';
            private const char _k_valueSplit = ':';
            
            [NotNull] private readonly PlayerChildComponent _m_comp;
            [ItemNotNull, NotNull] private readonly List<AdultEngageRequestInfo> _m_readEngageRequestList;


            public ReadEngageRequestSaver([NotNull] PlayerChildComponent _comp)
                : base($"{NPPlayer.instance.playerInfo.CID}_read_engage_request_saver")
            {
                _m_comp = _comp;
                _m_readEngageRequestList = new List<AdultEngageRequestInfo>();
            }


            public bool isSaved(AdultEngageRequestInfo _info)
            {
                if (_info == null)
                    return false;
                
                return _m_readEngageRequestList.Contains(_info);
            }
            public void clearEngageRequest()
            {
                _m_readEngageRequestList.Clear();
                saveSetting();
            }
            public void removeEngageRequest(AdultEngageRequestInfo _info, bool _isSave = true)
            {
                if (!_m_readEngageRequestList.Remove(_info))
                    return;
                
                if (_isSave)
                    saveSetting();
            }
            public void readAllEngageRequest()
            {
                _m_readEngageRequestList.Clear();
                _m_readEngageRequestList.AddRange(_m_comp._m_engageRequestInfoList);
                saveSetting();
            }
            
            
            protected override void _initSettingStr(string _infoStr)
            {
                if (string.IsNullOrEmpty(_infoStr))
                    return;

                _m_readEngageRequestList.Clear();
                try
                {
                    string[] dataStrArray = _infoStr.Split(_k_dataSplit);
                    foreach (string dataStr in dataStrArray)
                    {
                        string[] valueStr = dataStr.Split(_k_valueSplit);
                        long adultId = long.Parse(valueStr[0]);
                        int expireTime = int.Parse(valueStr[1]);
                        AdultEngageRequestInfo info = _m_comp._m_engageRequestInfoList.Find(_info => _info.adultId == adultId && _info.expiredTimeS == expireTime);
                        if (info != null)
                            _m_readEngageRequestList.Add(info);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("ReadEngageRequestSaver init has Exception.   _infoStr:   [" + _infoStr + "]\t\tException:   " + e);
                    _m_readEngageRequestList.Clear();
                }
            }
            protected override string _makeSettingStr()
            {
                StringBuilder sb = new StringBuilder();
                int forCount = 0;
                foreach (AdultEngageRequestInfo info in _m_readEngageRequestList)
                {
                    forCount++;
                    sb.Append(info.adultId);
                    sb.Append(_k_valueSplit);
                    sb.Append(info.expiredTimeS);
                    if (forCount != _m_readEngageRequestList.Count)
                        sb.Append(_k_dataSplit);
                }
                return sb.ToString();
            }
        }
    }
}