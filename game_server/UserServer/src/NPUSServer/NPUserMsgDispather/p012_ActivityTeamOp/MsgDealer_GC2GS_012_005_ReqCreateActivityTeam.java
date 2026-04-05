package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import GC2GS.p012_ActivityTeamOp.GC2GS_012_005_ReqCreateActivityTeam;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPGameRes.Refs.Activity.RefActivityTeam;
import NPUSServer.CommonActivityMgr.Core.GameLogicDealer.ActivityGameLogicDealer;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

public class MsgDealer_GC2GS_012_005_ReqCreateActivityTeam extends NPUserMsgDealer<GC2GS_012_005_ReqCreateActivityTeam>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_005_ReqCreateActivityTeam _msg)
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

        //检查设置数据
        RefActivityTeam ref = dealer.getRef();
        if(null == ref)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        if(_msg.getJoinCond().getCond() != ref.apply_cond_type)
        {
            _commiter.commitFailRes(CommErr.REF_ERROR.getCode());
            return;
        }

        dealer.createTeam(userData.getCid(), dealer.getMemberLimit(), _msg.getJoinType(), _msg.getJoinCond(), _msg.getTeamName(), _msg.getTeamDec(), (_result, _team) ->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            //推送数据
            userData.sendMsgToGC(US2GCWriter_012_ActivityTeamOp.make_051_OnJoinActivityTeam(_team));

            //返回结果
            _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_005_RetCreateActivityTeam());
        });
    }
}
