package NPUSServer.Guild.Msg;

import Common.GuildEnum.EGuildLogType;
import Common.GuildEnum.EGuildPositionType;
import Common.GuildObj.*;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.Guild.RefGuildLog;
import NPUSServer.Guild.GuildInfo;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * GUILD_CREATION, //1 ==== 联盟创建
 * MEMBER_JOIN, //2 ==== 新成员加入
 * MEMBER_LEAVE, //3 ==== 老成员离开
 * GUILD_CONSTRUCTION, //4 ==== 联盟建设
 * KICK_OUT_MEMBER, //5 ==== 踢出成员
 * POSITION_CHANGE, //6 ==== 职位变更
 * ANNOUNCEMENT_CHANGE, //7 ==== 公告变更
 * GUILD_RENAME, //8 ==== 联盟改名
 * GUILD_FLAG_CHANGE, //9 ==== 联盟旗帜变更
 * ENABLE_FREE_JOIN, //10 ==== 开启自由加入
 * DISABLE_FREE_JOIN, //11 ==== 关闭自由加入
 * GUILD_UPGRADE, //12 ==== 联盟升级
 */
public class GuildLogFunc
{
    public static void sendGuildCreationLog(long _cid, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.GUILD_CREATION;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        _guildInfo.getGuildMgr().getServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _cid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
            {
                if (!_isSuc)
                    return;

                GuildLog_MemberName proto = new GuildLog_MemberName(_info.getPlayerName());
                _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
            }
        });
    }

    public static void sendMemberJoinLog(long _cid, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.MEMBER_JOIN;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        _guildInfo.getGuildMgr().getServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _cid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
            {
                if (!_isSuc)
                    return;

                GuildLog_MemberName proto = new GuildLog_MemberName(_info.getPlayerName());
                _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
            }
        });
    }

    public static void sendMemberLeaveLog(String _name, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.MEMBER_LEAVE;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_MemberName proto = new GuildLog_MemberName(_name);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendGuildConstructionLog(String _name, long _refId, int _guildExp, int _guildWealth, int _guildCoin, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.GUILD_CONSTRUCTION;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_GuildConstruction proto = new GuildLog_GuildConstruction();
        proto.setPlayerName(_name);
        proto.setRefId(_refId);
        proto.setGuildExp(_guildExp);
        proto.setGuildWealth(_guildWealth);
        proto.setGuildCoin(_guildCoin);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendKickOutMemberLog(String _name, EGuildPositionType _position, long _kickedCid, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.KICK_OUT_MEMBER;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        _guildInfo.getGuildMgr().getServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _kickedCid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
            {
                if (!_isSuc)
                    return;

                GuildLog_KickOutMember proto = new GuildLog_KickOutMember();
                GuildLog_OperatorInfo operatorInfo = new GuildLog_OperatorInfo(_name, _position);
                proto.setOperatorInfo(operatorInfo);
                proto.setTargetPlayerName(_info.getPlayerName());
                _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
            }
        });
    }

    public static void sendPositionChangeLog(String _name, EGuildPositionType _position, long _targetCid, EGuildPositionType _targetPosition, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.POSITION_CHANGE;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        _guildInfo.getGuildMgr().getServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _targetCid, new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _info)
            {
                if (!_isSuc)
                    return;

                GuildLog_PositionChange proto = new GuildLog_PositionChange();
                GuildLog_OperatorInfo operatorInfo = new GuildLog_OperatorInfo(_name, _position);
                proto.setOperatorInfo(operatorInfo);
                proto.setTargetPlayerName(_info.getPlayerName());
                proto.setTargetPosition(_targetPosition);

                _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
            }
        });
    }

    public static void sendAnnouncementChangeLog(String _name, EGuildPositionType _position, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.ANNOUNCEMENT_CHANGE;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_OperatorInfo proto = new GuildLog_OperatorInfo(_name, _position);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendGuildRenameLog(String _name, EGuildPositionType _position, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.GUILD_RENAME;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_GuildRename proto = new GuildLog_GuildRename();
        proto.setNewName(_guildInfo.getName());
        proto.setOperatorInfo(new GuildLog_OperatorInfo(_name, _position));
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendGuildFlagChangeLog(String _name, EGuildPositionType _position, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.GUILD_FLAG_CHANGE;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_OperatorInfo proto = new GuildLog_OperatorInfo(_name, _position);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendEnableFreeJoinLog(String _name, EGuildPositionType _position, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.ENABLE_FREE_JOIN;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_OperatorInfo proto = new GuildLog_OperatorInfo(_name, _position);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendDisableFreeJoinLog(String _name, EGuildPositionType _position, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.DISABLE_FREE_JOIN;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_OperatorInfo proto = new GuildLog_OperatorInfo(_name, _position);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendGuildUpgradeLog(int _level, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.GUILD_UPGRADE;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        GuildLog_GuildUpgrade proto = new GuildLog_GuildUpgrade(_level);
        _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
    }

    public static void sendLeaderTransferLog(long _oriLeaderCid, long _newLeaderCid, GuildInfo _guildInfo)
    {
        EGuildLogType logType = EGuildLogType.LEADER_TRANSFER;
        RefGuildLog refGuildLog = RefGuildLog.getMgr().get(logType.ordinal());
        if (refGuildLog == null)
            return;

        List<Long> cidList = new ArrayList<>();
        cidList.add(_oriLeaderCid);
        cidList.add(_newLeaderCid);

        _guildInfo.getGuildMgr().getServer().getPlayerCacheGetter().getInfoListA(PlayerInfo_IconShow.class, cidList, new HandlerOne<Map<Long, PlayerInfo_IconShow>>()
        {
            @Override
            public void handle(Map<Long, PlayerInfo_IconShow> _infoMap)
            {
                PlayerInfo_IconShow oriLeader = _infoMap.get(_oriLeaderCid);
                PlayerInfo_IconShow newLeader = _infoMap.get(_newLeaderCid);
                if (oriLeader == null || newLeader == null)
                    return;

                GuildLog_LeaderTransfer proto = new GuildLog_LeaderTransfer(oriLeader.getPlayerName(), newLeader.getPlayerName());
                _guildInfo.sendGuildLog(logType, proto, refGuildLog.show_type_list);
            }
        });
    }
}
