using Common.GuildEnum;

namespace GOE
{
    public class GuildLogMsgDetailFactory
    {
        public static _IGuildLogMsgDetailInfo getGuildLogMsgDetailInfo(EGuildLogType _guildLogType, byte[] _bytes)
        {
            switch (_guildLogType)
            {
                case EGuildLogType.GUILD_CREATION:
                    return new GuildCreationLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.MEMBER_JOIN:
                    return new GuildMemberJoinLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.MEMBER_LEAVE:
                    return new GuildMemberLeaveLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.GUILD_CONSTRUCTION:
                    return new GuildConstructionLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.KICK_OUT_MEMBER:
                    return new GuildKickOutMemberLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.POSITION_CHANGE:
                    return new GuildPositionChangeLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.ANNOUNCEMENT_CHANGE:
                    return new GuildAnnouncementLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.GUILD_RENAME:
                    return new GuildRenameLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.GUILD_FLAG_CHANGE:
                    return new GuildFlagChangeLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.ENABLE_FREE_JOIN:
                    return new GuildEnableFreeJoinLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.DISABLE_FREE_JOIN:
                    return new GuildDisableFreeJoinLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.GUILD_UPGRADE:
                    return new GuildUpgradeLogMsgDetailInfo(_bytes);
                
                case EGuildLogType.LEADER_TRANSFER:
                    return new GuildLeaderTransferLogMsgDetailInfo(_bytes);
                
                default:
                    Debug.LogError($"[GuildLogMsgDetailFactory getGuildLogMsgDetailInfo] 类型_guildLogType没有对应的_IGuildLogMsgDetailInfo数据类");
                    return null;
            }
        }
    }
}