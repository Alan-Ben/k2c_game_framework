package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_FirstTeam_Team;
import Common.ServerObj.ServerObj_GLSTeamMember;
import GLSDB.Bo.FirstTeamActivityTeamBO;
import GLSDB.Bo.FirstTeamActivityTeamMemberBO;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity.TeamGroup._APlayerGroupTeamMgr;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 首组队活动队伍管理器
 *
 * 主要功能：
 * 1. 构建活动队伍对象
 * 2. 同步队伍成员CID列表
 */
public class FirstTeamActivityTeamMgr extends _APlayerGroupTeamMgr<FirstTeamActivityPlayerInfo, FirstTeamActivityTeamInfo>
{
    /**
     * 构造队伍管理器
     * @param _activity 所属活动对象
     */
    public FirstTeamActivityTeamMgr(FirstTeamActivity _activity)
    {
        super(_activity);
    }

    /**
     * 获取类型化活动对象
     * @return 首组队活动对象
     */
    public FirstTeamActivity getFirstTeamActivity()
    {
        return (FirstTeamActivity) getActivity();
    }

    /**
     * 从预加载的全量BO列表中筛选本活动队伍数据并注入管理器
     * @param _allTeamBoList 全量队伍BO列表（已一次性加载完毕）
     * @param _memberBoMap   成员BO数据（key=teamId，已在初始化器中按teamId预分组）
     */
    public void sInit(List<FirstTeamActivityTeamBO> _allTeamBoList,
                      HashMap<Long, List<FirstTeamActivityTeamMemberBO>> _memberBoMap)
    {
        long groupId = getFirstTeamActivity().getGroupId();
        for (int i = 0; i < _allTeamBoList.size(); i++)
        {
            FirstTeamActivityTeamBO teamBo = _allTeamBoList.get(i);
            if (null == teamBo || teamBo.getGroupId() != groupId)
                continue;

            FirstTeamActivityTeamInfo teamInfo = new FirstTeamActivityTeamInfo(this, teamBo);

            // 从预分组的成员数据中还原该队伍的成员CID列表
            List<FirstTeamActivityTeamMemberBO> memberBos = _memberBoMap.get(teamBo.getTeamId());
            if (null != memberBos)
            {
                ArrayList<Long> cidList = new ArrayList<>();
                for (int j = 0; j < memberBos.size(); j++)
                {
                    FirstTeamActivityTeamMemberBO memberBo = memberBos.get(j);
                    if (null != memberBo)
                        cidList.add(memberBo.getCid());
                }
                teamInfo.setMemberCidList(cidList);
            }

            sInitAdd(teamInfo);
        }
    }

    /**
     * 根据远端数据构建本地队伍对象
     * @param _teamId 队伍ID
     * @param _obj 远端队伍结构
     * @return 本地队伍对象
     */
    @Override
    protected FirstTeamActivityTeamInfo _buildPlayerGroup(long _teamId, _IALProtocolStructure _obj)
    {
        ServerObj_FirstTeam_Team teamData = (ServerObj_FirstTeam_Team) _obj;

        // 落地队伍基础记录
        FirstTeamActivityTeamBO bo = new FirstTeamActivityTeamBO();
        bo.setGroupId(GameLogicServer.getInstance().getBM(), getFirstTeamActivity().getGroupId());
        bo.setTeamId(GameLogicServer.getInstance().getBM(), _teamId);
        bo.insert(GameLogicServer.getInstance().getBM());

        FirstTeamActivityTeamInfo team = new FirstTeamActivityTeamInfo(this, bo);

        // 复制远端成员CID，作为本地队伍成员快照
        ArrayList<Long> cidList = new ArrayList<>();
        for (int i = 0; i < teamData.getMemberList().size(); i++)
        {
            ServerObj_GLSTeamMember member = teamData.getMemberList().get(i);
            if (null == member)
                continue;
            cidList.add(member.getCid());
        }
        team.setMemberCidList(cidList);

        return team;
    }
}
