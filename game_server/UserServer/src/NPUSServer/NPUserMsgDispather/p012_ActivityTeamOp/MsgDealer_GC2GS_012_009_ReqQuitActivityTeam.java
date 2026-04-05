package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import GC2GS.p012_ActivityTeamOp.GC2GS_012_009_ReqQuitActivityTeam;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core.GameLogicDealer.ActivityGameLogicDealer;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

public class MsgDealer_GC2GS_012_009_ReqQuitActivityTeam extends NPUserMsgDealer<GC2GS_012_009_ReqQuitActivityTeam>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_009_ReqQuitActivityTeam _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 检查对应的组队活动
        long groupId = CommonFunc.parseActivityTeamGroupId(_msg.getTeamId());
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivityByGroupId(groupId);
        if (null == activity)
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

        // 检查活动是否接入GameLogic
        ActivityGameLogicDealer gameLogicDealer = activity.getGameLogicDealer();
        if (null == gameLogicDealer)
        {
            _commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }

        dealer.quitTeam(_msg.getTeamId(), userData.getCid(), (_result) ->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            //返回结果
            _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_009_RetQuitActivityTeam());
        });
    }
}
