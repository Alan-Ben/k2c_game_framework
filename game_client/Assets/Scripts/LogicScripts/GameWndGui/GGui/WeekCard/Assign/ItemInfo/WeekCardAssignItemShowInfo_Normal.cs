using CommonEnum;
using NPEnum;

namespace GOE
{
    public class WeekCardAssignItemShowInfo_Normal : _IWeekCardAssignItemShowInfo
    {
        private readonly GWeekCardSettingInfo_DealPolicy _m_info;
        private readonly ENPFunctionType _m_functionType;

        public WeekCardAssignItemShowInfo_Normal(GWeekCardSettingInfo_DealPolicy _info, ENPFunctionType _functionType)
        {
            _m_info = _info;
            _m_functionType = _functionType;
        }

        public long getUIPathId()
        {
            return 4504;
        }

        public EWeekCardSettleType getAssignType()
        {
            return _m_info.settingType;
        }

        public string getAssignName()
        {
            return TextTranslate.instance.getLanguage(string.Format(TransKeyConst.week_card_type_name, _m_info.settingType.ToString()));
        }

        public string getAssignDesc()
        {
            return TextTranslate.instance.getLanguage(string.Format(TransKeyConst.week_card_type_desc, _m_info.settingType.ToString()));
        }

        public EGameCommonUnlockType getUnlockType()
        {
            return GCommon.isFuncUnlock(_m_functionType) ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK;
        }

        public GWeekCardSettingInfo_DealPolicy settingInfo { get => _m_info; }
    }
}