using Common.GuildEnum;
using Common.GuildObj;

namespace GOE
{
    public class GuildLimitInfoFactory
    {
        public static _AGuildJoinLimitInfo getLimitInfo(Guild_JoinLimitInfo _joinLimitInfo)
        {
            if (_joinLimitInfo == null)
                return null;

            switch (_joinLimitInfo.getType())
            {
                case EGuildJoinLimitType.LEVEL:
                    return new GuildJoinLevelLimit(_joinLimitInfo);

                case EGuildJoinLimitType.NATION_POWER:
                    return new GuildJoinNationPowerLimit(_joinLimitInfo);

                default:
                    Debug.LogError($"[GuildLimitInfoFactory.getLimitInfo] 没有类型:{_joinLimitInfo.getType()} 的处理");
                    return null;
            }
        }
    }
}