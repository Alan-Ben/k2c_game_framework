package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_009_SyncGroupTeamData;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_009_SyncGroupTeamData;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 处理 US 通知 GLS 同步队伍成员数据请求
 */
public class RequestDealer_NP2GLS_R_001_009_SyncGroupTeamData extends NPRequestDealer<NP2GLS_R_001_009_SyncGroupTeamData>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_009_SyncGroupTeamData _msg)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().lookup(_msg.getInstanceId());
        if (null == instance)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        // 委托活动对象同步指定 group 的队伍成员
        instance.getActivity().syncGroupFromTeam(_msg.getGroupId(), _msg.getTeamId());
        _committer.commitSucRes(new NP2GLS_RB_001_009_SyncGroupTeamData());
    }
}

