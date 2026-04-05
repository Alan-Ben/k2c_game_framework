package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import GC2GS.p012_ActivityTeamOp.GC2GS_012_004_ReqSetActivityTeamApplyCond;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

public class MsgDealer_GC2GS_012_004_ReqSetActivityTeamApplyCond extends NPUserMsgDealer<GC2GS_012_004_ReqSetActivityTeamApplyCond>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_004_ReqSetActivityTeamApplyCond _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查对应的组队活动
        long groupId = CommonFunc.parseActivityTeamGroupId(_msg.getTeamId());
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivityByGroupId(groupId);
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

        dealer.updateJoinCond(_msg.getTeamId(), userData.getCid(), _msg.getJoinCond(), result ->
        {
            if (!result.isSucc())
            {
                _commiter.commitFailRes(result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_004_RetSetActivityTeamApplyCond());
        });
    }
}
