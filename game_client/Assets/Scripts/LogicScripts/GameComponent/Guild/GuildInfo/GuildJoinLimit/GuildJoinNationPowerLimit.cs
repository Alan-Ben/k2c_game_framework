using Common.GuildObj;
using NPEnum;

namespace GOE
{
    public class GuildJoinNationPowerLimit : _AGuildJoinLimitInfo
    {
        public GuildJoinNationPowerLimit(Guild_JoinLimitInfo _joinLimitInfo) : base(_joinLimitInfo)
        {
        }

        protected override bool _meetLimitingCondition(long _value)
        {
            return _value >= _m_joinLimitInfo.getValue();
        }

        protected override bool _selfMeetLimitingCondition()
        {
            return NPPlayer.instance.getValue(ENPPlayerValueType.EARNINGS) >= _m_joinLimitInfo.getValue();
        }
        public override string desc
        {
            get
            {
                if (_m_JoinLimitRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.desc, _m_joinLimitInfo.getValue().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            }
        }

        public override string onTryJoinNotConformLimitTip
        {
            get
            {
                if (_m_JoinLimitRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.on_try_join_not_conform_limit_tip, _m_joinLimitInfo.getValue().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            }
        }
    }
}