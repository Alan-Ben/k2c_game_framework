package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_012_ReqDailyQuestTryFresh;
import NPCommon.ErrMain.QuestErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestComponent;
import NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp.DailyQuestFreshGroup;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;

public class MsgDealer_GC2GS_028_012_ReqDailyQuestTryFresh extends NPUserMsgDealer<GC2GS_028_012_ReqDailyQuestTryFresh>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_012_ReqDailyQuestTryFresh _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        DailyQuestComponent dailyQuestComponent = userData.getDailyQuestComponent();

        DailyQuestFreshGroup group = dailyQuestComponent.clientTryFreshGroup(_msg.getType(), _msg.getRefreshSerial());
        if (group == null)
        {
            //序列号不匹配，无法操作
            _commiter.commitFailRes(QuestErr.DAILY_QUEST_SERIAL_NOT_EQUAL.getCode());
            return;
        }

        //回包
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_012_RetDailyQuestTryFresh(group.makeProto()));
    }
}
