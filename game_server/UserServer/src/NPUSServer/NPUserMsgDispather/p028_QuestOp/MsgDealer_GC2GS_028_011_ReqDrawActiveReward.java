package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_011_ReqDrawActiveReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestActiveReward;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestFreshGroup;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028011DailyQuestDrawActiveRewardBO;

public class MsgDealer_GC2GS_028_011_ReqDrawActiveReward extends NPUserMsgDealer<GC2GS_028_011_ReqDrawActiveReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_011_ReqDrawActiveReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //查找活跃奖励配置
        RefDailyQuestActiveReward refActiveReward = RefDailyQuestActiveReward.getMgr().get(_msg.getActiveRewardId());
        if (refActiveReward == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DAILY_QUEST_ACTIVE_GAIN);

        DailyQuestFreshGroup questGroup = userData.getDailyQuestComponent().lookupGroup(refActiveReward.daily_quest_type);
        if (questGroup == null)
        {
            _commiter.commitFailRes(QuestErr.DAILY_QUEST_NOT_EXIST.getCode());
            return;
        }

        //执行领取奖励逻辑
        Result result = questGroup.drawActiveReward(_msg.getActiveRewardId(), _msg.getRefreshSerial(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //奖励通用推送
        userData.sendMsgToGC(context.getCollector().toProto());
        //回包
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_011_RetDrawActiveReward());

        //日志数据
        Opt028011DailyQuestDrawActiveRewardBO optBo = new Opt028011DailyQuestDrawActiveRewardBO();
        optBo.setRefreshSerial(getUSServer().getBM(), _msg.getRefreshSerial());
        optBo.setRewardId(getUSServer().getBM(), _msg.getActiveRewardId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
