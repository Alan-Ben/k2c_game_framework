package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_013_ReqAKeyDrawActivityStepReward;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

public class MsgDealer_GC2GS_017_013_ReqAKeyDrawActivityStepReward extends NPUserMsgDealer<GC2GS_017_013_ReqAKeyDrawActivityStepReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_013_ReqAKeyDrawActivityStepReward _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //获取阶段奖励信息
        ActivityStepRewardInfo stepRewardInfo = activity.lookupStepReward(_msg.getStepRewardId());
        if (stepRewardInfo == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_STEP_REWARD_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_ACTIVITY_STEP_REWARD);

        //领取阶段奖励奖励
        Result result = stepRewardInfo.aKeyDrawStepReward(userData, context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        //返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_013_RetAKeyDrawActivityStepReward());

        //注意：数据日志记录已在ActivityStepRewardInfo.aKeyDrawStepReward()方法中实现，记录每个成功领取的阶段ID
    }
}
