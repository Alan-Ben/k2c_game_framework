package GameLogicServer.GroupMgr.ActivityMgr.FirstTeamActivity;

import GLSDB.Bo.FirstTeamActivityPlayerBO;
import GameLogicServer.GameLogicServer;
import GameLogicServer.GroupMgr.PlayerGroupMgr._ATPlayerGroupPlayerMgr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;

import java.util.List;

/**
 * 首组队活动玩家管理器
 *
 * 主要功能：
 * 1. 负责玩家数据的创建与回调
 */
public class FirstTeamActivityPlayerMgr extends _ATPlayerGroupPlayerMgr<FirstTeamActivityPlayerInfo>
{
    /**
     * 构造玩家管理器
     * @param _activity 所属活动对象
     */
    public FirstTeamActivityPlayerMgr(FirstTeamActivity _activity)
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
     * 从预加载的全量BO列表中筛选本活动玩家数据并注入管理器
     * @param _allPlayerBoList 全量玩家BO列表（已一次性加载完毕）
     */
    public void sInit(List<FirstTeamActivityPlayerBO> _allPlayerBoList)
    {
        long groupId = getActivity().getGroupId();
        for (int i = 0; i < _allPlayerBoList.size(); i++)
        {
            FirstTeamActivityPlayerBO bo = _allPlayerBoList.get(i);
            if (null == bo || bo.getGroupId() != groupId)
                continue;

            FirstTeamActivityPlayerInfo player = new FirstTeamActivityPlayerInfo(this, bo);
            sInitAdd(player);
        }
    }

    /**
     * 构建玩家数据
     * @param _cid 玩家CID
     * @param _callback 构建结果回调
     */
    @Override
    protected void _buildPlayerInfo(long _cid, _ICallBackResultT<FirstTeamActivityPlayerInfo> _callback)
    {
        // 本地无缓存时创建一条玩家活动数据
        FirstTeamActivityPlayerBO bo = new FirstTeamActivityPlayerBO();
        bo.setGroupId(GameLogicServer.getInstance().getBM(), getActivity().getGroupId());
        bo.setCid(GameLogicServer.getInstance().getBM(), _cid);
        bo.insert(GameLogicServer.getInstance().getBM());

        FirstTeamActivityPlayerInfo player = new FirstTeamActivityPlayerInfo(this, bo);
        // 创建成功后回调上层流程
        if (null != _callback)
            _callback.onRunOver(Result.SUCC, player);
    }
}
