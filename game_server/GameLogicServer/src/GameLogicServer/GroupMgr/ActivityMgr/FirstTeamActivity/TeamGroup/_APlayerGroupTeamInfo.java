package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.TeamGroup;

import GameLogicServer.GroupMgr.PlayerGroupMgr._APlayerGroupPlayerInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupInfo;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATGroupMgr;

/**
 * 队伍型归属主体抽象基类
 * @param <T> 队伍成员类型
 */
public abstract class _APlayerGroupTeamInfo<T extends _APlayerGroupPlayerInfo> extends _ATGroupInfo<T>
{
    /**
     * 构造队伍主体对象
     * @param _groupMgr 所属队伍管理器
     * @param _groupId 队伍ID
     */
    public _APlayerGroupTeamInfo(_ATGroupMgr<T, ?> _groupMgr, long _groupId)
    {
        super(_groupMgr, _groupId);
    }
}
