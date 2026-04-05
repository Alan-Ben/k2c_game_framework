package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import Common.QuestEnum.EQuestType;
import GC2GS.p028_QuestOp.GC2GS_028_002_ReqFinishQuest;
import MJLog.MJEventLog;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.ErrMain.QuestErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.QuestComp.PlayerQuestInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028002QuestFinishBO;

public class MsgDealer_GC2GS_028_002_ReqFinishQuest extends NPUserMsgDealer<GC2GS_028_002_ReqFinishQuest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_002_ReqFinishQuest _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.QUEST_FINISH);
        PlayerQuestInfo questInfo = _commiter.getUserData().getQuestComponent().lookupQuest(_msg.getQuestId());
        if (questInfo == null)
        {
            _commiter.commitFailRes(QuestErr.QUEST_FINISH_FAIL.getCode());
            return;
        }

        long questStep = questInfo.getQuestStep();

        NPItemCollector itemCollector = new NPItemCollector(0);
        if (!_commiter.getUserData().getQuestComponent().finishQuestStep(_msg.getQuestId(), context, itemCollector))
        {
            _commiter.commitFailRes(QuestErr.QUEST_FINISH_FAIL.getCode());
            return;
        }

        //返回协议
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_002_RetFinishQuest(_msg.getQuestId(), questStep, itemCollector));

        //操作日志
        Opt028002QuestFinishBO optBo = new Opt028002QuestFinishBO();
        optBo.setQuestId(getUSServer().getBM(), _msg.getQuestId());
        _commiter.getUserData().logEvent(optBo, context);

        //运营日志
        if (questInfo.getRef().quest_type == EQuestType.MAIN)
        {
            MJEventLog.logTask(_commiter.getUserData(), 2, Long.toString(_msg.getQuestId()));
        }
    }
}
