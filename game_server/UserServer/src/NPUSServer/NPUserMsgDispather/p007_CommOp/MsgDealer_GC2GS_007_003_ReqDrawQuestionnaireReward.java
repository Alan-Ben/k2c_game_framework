package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_003_ReqDrawQuestionnaireReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_003_ReqDrawQuestionnaireReward extends NPUserMsgDealer<GC2GS_007_003_ReqDrawQuestionnaireReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_003_ReqDrawQuestionnaireReward _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_QUESTIONNAIRE_REWARD);

        Result result = getUSServer().getQuestionnaireMgr().drawReward(userData, _msg.getId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_003_RetDrawQuestionnaireReward());
    }
}
