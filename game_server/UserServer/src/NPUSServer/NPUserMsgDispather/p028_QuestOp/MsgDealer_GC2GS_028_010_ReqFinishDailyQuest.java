package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_010_ReqFinishDailyQuest;
import MJLog.MJEventLog;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028010DailyQuestFinishBO;

public class MsgDealer_GC2GS_028_010_ReqFinishDailyQuest extends NPUserMsgDealer<GC2GS_028_010_ReqFinishDailyQuest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_010_ReqFinishDailyQuest _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DAILY_QUEST_FINISH);

        //执行领取奖励逻辑
        Result result = userData.getDailyQuestComponent().drawQuestReward(_msg.getQuestId(), _msg.getRefreshSerial(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_010_RetFinishDailyQuest(context.getCollector()));

        //记录玩家完成日常任务次数
        _commiter.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.FINISH_DAILY_QUEST, 1, context);

        //日志数据
        Opt028010DailyQuestFinishBO optBo = new Opt028010DailyQuestFinishBO();
        optBo.setQuestId(getUSServer().getBM(), _msg.getQuestId());
        optBo.setRefreshSerial(getUSServer().getBM(), _msg.getRefreshSerial());
        _commiter.getUserData().logEvent(optBo, context);

        //运营日志
        MJEventLog.logTask(userData, 1, Long.toString(_msg.getQuestId()));
    }
}
