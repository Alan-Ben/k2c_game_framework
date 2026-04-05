package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import GC2GS.p012_ActivityTeamOp.GC2GS_012_003_ReqSelfActivityTeam;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

public class MsgDealer_GC2GS_012_003_ReqSelfActivityTeam extends NPUserMsgDealer<GC2GS_012_003_ReqSelfActivityTeam>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_003_ReqSelfActivityTeam _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查对应的组队活动
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if(null == activity)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //检查活动的组队接口
        ActivityTeamDealer dealer = activity.getTeamDealer();
        if(null == dealer)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_TYPE_ERROR.getCode());
            return;
        }

        dealer.getPlayerTeam(userData.getCid(), (_result, _team)->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_003_RetSelfActivityTeam(_team));
        });
    }
}
