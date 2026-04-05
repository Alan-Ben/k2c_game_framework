package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_004_ReqQuestionnaireInfo;
import NPCommon.ErrMain.HttpErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.QuestionnaireMgr.QuestionnaireInfo;
import NPUSServer.QuestionnaireMgr.QuestionnairePlayerRecordInfo;

public class MsgDealer_GC2GS_007_004_ReqQuestionnaireInfo extends NPUserMsgDealer<GC2GS_007_004_ReqQuestionnaireInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_004_ReqQuestionnaireInfo _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        QuestionnaireInfo questionnaireInfo = getUSServer().getQuestionnaireMgr().lookupByQuestionnaireId(_msg.getId());
        if (questionnaireInfo == null)
        {
            _commiter.commitFailRes(HttpErr.QUESTIONNAIRE_ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        QuestionnairePlayerRecordInfo recordInfo = questionnaireInfo.lookup(userData.getCid());
        if (recordInfo == null)
        {
            _commiter.commitSucRes(US2GCWriter_007_CommOp.make_004_RetQuestionnaireInfo(null));
        }else
        {
            _commiter.commitSucRes(US2GCWriter_007_CommOp.make_004_RetQuestionnaireInfo(recordInfo.makeProto()));
        }
    }
}
