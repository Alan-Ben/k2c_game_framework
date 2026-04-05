using CommonEnum;
using NPEnum;

namespace GOE
{
    public class WeekCardAssignItemShowInfo_College : _IWeekCardAssignItemShowInfo
    {
        private readonly GWeekCardSettingInfo_CollegeStudy _m_info;

        public WeekCardAssignItemShowInfo_College(GWeekCardSettingInfo_CollegeStudy _info)
        {
            _m_info = _info;
        }

        public long getUIPathId()
        {
            return 4505;
        }

        public EWeekCardSettleType getAssignType()
        {
            return _m_info.settingType;
        }

        public string getAssignName()
        {
            return TextTranslate.instance.getLanguage(string.Format(TransKeyConst.week_card_type_name, _m_info.settingType.ToString()));
        }

        public bool getIsClose()
        {
            return _m_info.isClose;
        }

        public string getAssignDesc()
        {
            return TextTranslate.instance.getLanguage(string.Format(TransKeyConst.week_card_type_desc, _m_info.settingType.ToString()));
        }

        public EGameCommonUnlockType getUnlockType()
        {
            return EGameCommonUnlockType.LOCK;
        }

        public GWeekCardSettingInfo_CollegeStudy settingInfo { get => _m_info; }
    }
}