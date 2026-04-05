package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import GLSDB.Bo.FirstTeamActivityTeamBO;
import GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.TeamGroup._APlayerGroupTeamInfo;

/**
 * 首组队活动队伍信息对象
 */
public class FirstTeamActivityTeamInfo extends _APlayerGroupTeamInfo<FirstTeamActivityPlayerInfo>
{
    /**
     * 构造队伍信息
     * @param _groupMgr 所属队伍管理器
     * @param _bo 队伍BO数据
     */
    public FirstTeamActivityTeamInfo(FirstTeamActivityTeamMgr _groupMgr, FirstTeamActivityTeamBO _bo)
    {
        super(_groupMgr, _bo.getTeamId());
    }
}
