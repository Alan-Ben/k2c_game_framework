package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_005_ReqSevenDayGoalsDrawReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

public class MsgDealer_GC2GS_033_005_ReqSevenDayGoalsDrawReward extends NPUserMsgDealer<GC2GS_033_005_ReqSevenDayGoalsDrawReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_005_ReqSevenDayGoalsDrawReward _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_SEVEN_DAY_GOALS_TASK_REWARD);

        //执行逻辑
        Result result = _committer.getUserData().getSevenDayGoalsComponent().drawTaskReward(_msg.getRefId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(context.getCollector().toProto());

        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_005_RetSevenDayGoalsDrawReward());
    }
}
