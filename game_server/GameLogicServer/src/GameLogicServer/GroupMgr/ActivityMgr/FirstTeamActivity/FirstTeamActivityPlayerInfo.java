package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import GLSDB.Bo.FirstTeamActivityPlayerBO;
import GameLogicServer.GroupMgr.PlayerGroupMgr._APlayerGroupPlayerInfo;

/**
 * 首组队活动玩家信息对象
 */
public class FirstTeamActivityPlayerInfo extends _APlayerGroupPlayerInfo
{
    /**
     * 构造玩家信息
     * @param _playerMgr 所属玩家管理器
     * @param _bo 玩家BO数据
     */
    public FirstTeamActivityPlayerInfo(FirstTeamActivityPlayerMgr _playerMgr, FirstTeamActivityPlayerBO _bo)
    {
        super(_playerMgr, _bo.getCid());
    }
}
