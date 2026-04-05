using Common.GuildObj;
using NPEnum;

namespace GOE
{
    public class GuildJoinLevelLimit : _AGuildJoinLimitInfo
    {
        private PlayerLvlRefObj _m_rLevelRefObj;
        public GuildJoinLevelLimit(Guild_JoinLimitInfo _joinLimitInfo) : base(_joinLimitInfo)
        {
            _m_rLevelRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_m_joinLimitInfo.getValue());
        }

        protected override bool _meetLimitingCondition(long _value)
        {
            return _value >= _m_joinLimitInfo.getValue();
        }

        protected override bool _selfMeetLimitingCondition()
        {
            return NPPlayer.instance.getValue(ENPPlayerValueType.LVL) >= _m_joinLimitInfo.getValue();
        }

        public override string desc {
            get
            {
                if (_m_JoinLimitRefObj == null || _m_rLevelRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.desc, _m_rLevelRefObj.name);
            }
        }

        public override string onTryJoinNotConformLimitTip
        {
            get
            {
                if (_m_JoinLimitRefObj == null || _m_rLevelRefObj == null)
                    return string.Empty;

                return TextTranslate.instance.getLanguage(_m_JoinLimitRefObj.on_try_join_not_conform_limit_tip, _m_rLevelRefObj.name);
            }
        }
    }
}