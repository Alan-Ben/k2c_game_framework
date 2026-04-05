package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import GLSDB.Bo.FirstTeamActivityPlayerBO;
import GLSDB.Bo.FirstTeamActivityTeamBO;
import GLSDB.Bo.FirstTeamActivityTeamMemberBO;
import GameLogicServer.GroupMgr.ActivityFactory._IActivityBoInitializer;
import GameLogicServer.GroupMgr.GroupInstanceInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * FirstTeamActivity BO数据批量初始化器
 *
 * 主要功能：
 * 1. 一次性加载三张BO表的全量数据到内存
 * 2. 成员数据按teamId预分组，提升分配效率
 * 3. 遍历活跃实例，将数据注入每个 FirstTeamActivity 对象
 */
public class FirstTeamActivityBoInitializer implements _IActivityBoInitializer
{
    @Override
    public boolean sInitAll(BM _bm, HashMap<Long, GroupInstanceInfo> _activeInstances)
    {
        // 一次性加载全量玩家BO数据
        List<FirstTeamActivityPlayerBO> playerBoList = _bm.getBM(FirstTeamActivityPlayerBO.class).s_findAll();
        if (null == playerBoList)
        {
            CommLog.error("FirstTeamActivityBoInitializer.sInitAll - load FirstTeamActivityPlayerBO fail.");
            return false;
        }

        // 一次性加载全量队伍BO数据
        List<FirstTeamActivityTeamBO> teamBoList = _bm.getBM(FirstTeamActivityTeamBO.class).s_findAll();
        if (null == teamBoList)
        {
            CommLog.error("FirstTeamActivityBoInitializer.sInitAll - load FirstTeamActivityTeamBO fail.");
            return false;
        }

        // 一次性加载全量队伍成员BO数据
        List<FirstTeamActivityTeamMemberBO> memberBoList = _bm.getBM(FirstTeamActivityTeamMemberBO.class).s_findAll();
        if (null == memberBoList)
        {
            CommLog.error("FirstTeamActivityBoInitializer.sInitAll - load FirstTeamActivityTeamMemberBO fail.");
            return false;
        }

        // 成员数据按 teamId 内存分组，减少后续每个队伍的遍历开销
        HashMap<Long, List<FirstTeamActivityTeamMemberBO>> memberBoMap = new HashMap<>();
        for (int i = 0; i < memberBoList.size(); i++)
        {
            FirstTeamActivityTeamMemberBO memberBo = memberBoList.get(i);
            if (null == memberBo)
                continue;
            memberBoMap.computeIfAbsent(memberBo.getTeamId(), k -> new ArrayList<>()).add(memberBo);
        }

        // 遍历活跃实例，找到 FirstTeamActivity 类型并注入对应数据
        for (GroupInstanceInfo info : _activeInstances.values())
        {
            if (!(info.getActivity() instanceof FirstTeamActivity))
                continue;

            FirstTeamActivity activity = (FirstTeamActivity) info.getActivity();
            activity.sInit(playerBoList, teamBoList, memberBoMap);
        }

        return true;
    }
}

