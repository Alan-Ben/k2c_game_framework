package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_025_ReqDoneAchieveStep;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USLOGDB.OptBo.Opt021025AchieveDoneStepBO;

/*************
 * 设置称号信息已查看
 * @author mj
 *
 */
public class MsgDealer_GC2GS_021_025_ReqDoneAchieveStep extends NPUserMsgDealer<GC2GS_021_025_ReqDoneAchieveStep>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_025_ReqDoneAchieveStep _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DONE_ACHIEVE_STEP);

        //领取成就步骤奖励
        Result result = userData.getAchieveComponent().drawAchieveStepReward(_msg.getAchieveId(), _msg.getStep(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        // 增加接待次数计数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.DRAW_ACHIEVE_REWARD_TIMES, 1, context);

        //回包协议
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_025_RetDoneAchieveStep());
        
        //操作日志
        Opt021025AchieveDoneStepBO optBo = new Opt021025AchieveDoneStepBO();
        optBo.setAchieveId(userData.getUSServer().getBM(), _msg.getAchieveId());
        optBo.setStep(userData.getUSServer().getBM(), _msg.getStep());
        userData.logEvent(optBo, context);
    }
}
