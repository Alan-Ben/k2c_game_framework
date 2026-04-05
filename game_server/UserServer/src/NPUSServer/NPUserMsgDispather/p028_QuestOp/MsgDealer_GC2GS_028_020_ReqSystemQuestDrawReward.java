package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_020_ReqSystemQuestDrawReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp.SystemQuestGroupInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;

public class MsgDealer_GC2GS_028_020_ReqSystemQuestDrawReward extends NPUserMsgDealer<GC2GS_028_020_ReqSystemQuestDrawReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_020_ReqSystemQuestDrawReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        SystemQuestGroupInfo groupInfo = userData.getSystemQuestComponent().lookupGroup(_msg.getGroupId());
        if (groupInfo == null)
        {
            _commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_SYSTEM_QUEST_STEP_REWARD);
        Result result = groupInfo.drawStepReward(_msg.getStep(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_020_RetSystemQuestDrawReward(context.getCollector()));

    }
}
