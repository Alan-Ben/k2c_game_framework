package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_003_ReqDropQuest;
import NPCommon.ErrMain.QuestErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028003QuestDropBO;

public class MsgDealer_GC2GS_028_003_ReqDropQuest extends NPUserMsgDealer<GC2GS_028_003_ReqDropQuest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_003_ReqDropQuest _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.QUEST_DROP);
        //放弃任务
        if (!_commiter.getUserData().getQuestComponent().dropQuest(_msg.getQuestId(), context))
        {
            _commiter.commitFailRes(QuestErr.QUEST_DROP_FAIL.getCode());
            return;
        }

        //返回协议
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_003_RetDropQuest());

        //操作日志
        Opt028003QuestDropBO optBo = new Opt028003QuestDropBO();
        optBo.setQuestId(getUSServer().getBM(), _msg.getQuestId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
