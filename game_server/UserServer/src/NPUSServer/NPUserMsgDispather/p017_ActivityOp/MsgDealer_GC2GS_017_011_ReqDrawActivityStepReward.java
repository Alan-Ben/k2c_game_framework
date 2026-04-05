package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_011_ReqDrawActivityStepReward;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.StepReward.ActivityStepRewardInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.USLog;
import USLOGDB.Bo.LogActivityStepRewardDrawBO;

public class MsgDealer_GC2GS_017_011_ReqDrawActivityStepReward extends NPUserMsgDealer<GC2GS_017_011_ReqDrawActivityStepReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_011_ReqDrawActivityStepReward _msg)
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
        Result result = stepRewardInfo.drawStepReward(userData, _msg.getStepId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        //返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_011_RetDrawActivityStepReward());

        //记录数据日志
        try
        {
            BM bmObj = userData.getUSServer().getBM();

            LogActivityStepRewardDrawBO logBo = new LogActivityStepRewardDrawBO();
            logBo.setCid(bmObj, userData.getCid());
            logBo.setInstanceId(bmObj, _msg.getInstanceId());
            logBo.setStepRewardId(bmObj, _msg.getStepRewardId());
            logBo.setStepId(bmObj, _msg.getStepId());
            logBo.setIsAKey(bmObj, false);
            CommLogDB.log(bmObj, logBo, context);
        } catch (Exception e)
        {
            USLog.error(userData.getUSServer(), "MsgDealer_GC2GS_017_011_ReqDrawActivityStepReward._dealMessage - log failed: exception occurred, cid={}, instanceId={}, stepRewardId={}, stepId={}, error={}",
                       userData.getCid(), _msg.getInstanceId(), _msg.getStepRewardId(), _msg.getStepId(), e.getMessage());
        }
    }
}
