using System;
using Common.WeekCardObj;
using CommonEnum;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    public abstract class _AGWeekCardSettingInfoBase
    {

        private EWeekCardSettleType _m_settingType;
    
        public _AGWeekCardSettingInfoBase(WeekCard_SingleSettingInfo _info)
        {
            _m_settingType = _info.getType();
            _initExInfo(_info.getExtraInfo());
        }

        public EWeekCardSettleType settingType { get => _m_settingType; }

        protected abstract void _initExInfo(byte[] _exInfo);


        public void reqWeekCardChgSetting(Action<GS2GC_004_016_RetWeekCardChgSetting> _backAction = null)
        {
            WeekCard_SingleSettingInfo info = new WeekCard_SingleSettingInfo();
            info.setType(_m_settingType);
            info.setExtraInfo(_makeExInfoPackage());

            NPPlayer.instance.weekCardComp.reqWeekCardChgSetting(info, _backAction);
        }

        protected abstract byte[] _makeExInfoPackage();
    }
}