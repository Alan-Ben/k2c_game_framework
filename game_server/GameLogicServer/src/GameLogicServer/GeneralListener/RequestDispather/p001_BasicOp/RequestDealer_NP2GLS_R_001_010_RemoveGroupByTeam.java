package GameLogicServer.GeneralListener.RequestDispather.p001_BasicOp;

import GameLogicServer.GroupMgr.GroupInstanceInfo;
import GameLogicServer.GroupMgr.GroupInstanceMgr;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_010_RemoveGroupByTeam;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_010_RemoveGroupByTeam;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.CommErr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 处理 US 通知 GLS 移除指定 group 请求（队伍解散时触发）
 */
public class RequestDealer_NP2GLS_R_001_010_RemoveGroupByTeam extends NPRequestDealer<NP2GLS_R_001_010_RemoveGroupByTeam>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GLS_R_001_010_RemoveGroupByTeam _msg)
    {
        GroupInstanceInfo instance = GroupInstanceMgr.getInstance().lookup(_msg.getInstanceId());
        if (null == instance)
        {
            _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        // 委托活动对象移除指定 group
        instance.getActivity().removeGroup(_msg.getGroupId());
        _committer.commitSucRes(new NP2GLS_RB_001_010_RemoveGroupByTeam());
    }
}

