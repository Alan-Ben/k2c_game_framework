using Common.GuildEnum;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        public GuildJoinLimitRefObj getGuildJoinLimitInfo(EGuildJoinLimitType _joinLimitType)
        {
            foreach (var _joinLimitInfo in GRefdataCoreMgr.instance.guildJoinLimitRefCore.refList)
            {
                if (_joinLimitInfo != null && _joinLimitInfo.type == _joinLimitType)
                {
                    return _joinLimitInfo;
                }
            }

            return null;
        }
    }
}