package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_001_ReqStartQuest;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028001QuestStartBO;

public class MsgDealer_GC2GS_028_001_ReqStartQuest extends NPUserMsgDealer<GC2GS_028_001_ReqStartQuest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_001_ReqStartQuest _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.QUEST_START);
        Result result = _commiter.getUserData().getQuestComponent().startQuestByClient(_msg.getQuestId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //返回协议
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_001_RetStartQuest());

        //操作日志
        Opt028001QuestStartBO optBo = new Opt028001QuestStartBO();
        optBo.setQuestId(getUSServer().getBM(), _msg.getQuestId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
