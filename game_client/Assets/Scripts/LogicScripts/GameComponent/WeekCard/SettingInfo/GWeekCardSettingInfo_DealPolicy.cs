using System;
using Common.WeekCardObj;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 周卡设置-懒汉策略
    /// </summary>
    public class GWeekCardSettingInfo_DealPolicy:_AGWeekCardSettingInfoBase
    {
        private WeekCard_ExtraInfo_DealPolicy _m_exInfo;
        
        public GWeekCardSettingInfo_DealPolicy(WeekCard_SingleSettingInfo _info) : base(_info)
        {
        }

        public WeekCard_ExtraInfo_DealPolicy exInfo { get => _m_exInfo; }

        protected override void _initExInfo(byte[] _exInfo)
        {
            _m_exInfo = new WeekCard_ExtraInfo_DealPolicy();
            if(null == _exInfo || _exInfo.Length == 0)
                return;
            _m_exInfo.readPackage(_exInfo);
        }

        public void setExInfo(bool _isLazy, Action<GS2GC_004_016_RetWeekCardChgSetting> _backAction = null)
        {
            _m_exInfo.setIsLazy(_isLazy);
            reqWeekCardChgSetting(_backAction);
        }

        protected override byte[] _makeExInfoPackage()
        {
            return _m_exInfo.makePackage();
        }
    }
}