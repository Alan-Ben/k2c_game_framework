package NPUSServer.QuestionnaireMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.Common_QuestionnaireRewardInfo;
import Common.MailObj.Mail_Data;
import NPCommon.NPCommon_ItemInfo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import USDB.Bo.QuestionnairePlayerRecordBO;

import java.util.List;

public class QuestionnairePlayerRecordInfo
{
    private QuestionnaireInfo _m_info;
    private long _m_dbId;
    private long _m_cid;
    private boolean _m_hasDraw;

    public QuestionnairePlayerRecordInfo(QuestionnaireInfo _info, QuestionnairePlayerRecordBO _bo)
    {
        _m_info = _info;
        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_hasDraw = _bo.getHasDraw();
    }

    public long getCid()
    {
        return _m_cid;
    }

    /**
     * 是否已领取
     * @return
     */
    public boolean hasDraw()
    {
        return _m_hasDraw;
    }

    /**
     * 标记领取
     */
    public void markDraw(NPPlayerContext _context)
    {
        _m_hasDraw = true;

        //保存数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("has_draw", 1);
        _m_info.getMgr().getUserServer().getBM().getBM(QuestionnairePlayerRecordBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 检查补发邮件
     */
    public void checkSendMail(List<NPCommon_ItemInfo> _rewardList, NPPlayerContext _context)
    {
        if (_m_hasDraw)
            return;

        markDraw(_context);

        //发送邮件
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(RefGeneral.Ref().questionnaire_mail_id);
        mailData.getItemList().getItemList().addAll(_rewardList);
        ALSynTaskManager.getInstance().regTask(() -> MailSystem.addMail(_m_info.getMgr().getUserServer(), _m_cid, mailData, _context));
    }

    public Common_QuestionnaireRewardInfo makeProto()
    {
        Common_QuestionnaireRewardInfo proto = new Common_QuestionnaireRewardInfo();
        proto.setQuestionnaireId(_m_info.getQuestionnaireId());
        proto.setHasDraw(_m_hasDraw);
        return proto;
    }
}
