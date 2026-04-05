package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import Common.CrossTeamObj.CrossTeam_BaseInfo;
import GC2GS.p012_ActivityTeamOp.GC2GS_012_001_ReqActivityTeamList;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

import java.util.ArrayList;

public class MsgDealer_GC2GS_012_001_ReqActivityTeamList extends NPUserMsgDealer<GC2GS_012_001_ReqActivityTeamList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_001_ReqActivityTeamList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 检查对应的组队活动
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (null == activity)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 检查活动的组队接口
        ActivityTeamDealer dealer = activity.getTeamDealer();
        if (null == dealer)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_TYPE_ERROR.getCode());
            return;
        }

        // 请求 CTS 获取指定页的队伍列表
        dealer.getGroupTeamList(_msg.getPage(), (_result, _rpc) ->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            ArrayList<CrossTeam_BaseInfo> teamBaseList = _rpc.retObj().getTeamBaseList();
            int totalCount = _rpc.retObj().getTotalCount();
            _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_001_RetActivityTeamList(teamBaseList, totalCount));
        });
    }
}
