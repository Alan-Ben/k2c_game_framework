package NPUSServer.NPUserMsgDispather.p028_QuestOp;

import GC2GS.p028_QuestOp.GC2GS_028_004_ReqAddClientTargetCount;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPEnum.ENCounterDealType;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Quest.RefQuestTarget;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.QuestComp.PlayerQuestInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USLOGDB.OptBo.Opt028004QuestAddTargetCountBO;

public class MsgDealer_GC2GS_028_004_ReqAddClientTargetCount extends NPUserMsgDealer<GC2GS_028_004_ReqAddClientTargetCount>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_028_004_ReqAddClientTargetCount _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.QUEST_CLIENT_TRIGGER);

        //获取目标属性
        RefQuestTarget targetRef = RefQuestTarget.getMgr().get(_msg.getQuestStepTargetId());
        if (null == targetRef)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //判断是否客户端驱动，不是则返回错误
        if (!targetRef.is_client_target)
        {
            _commiter.commitFailRes(CommErr.OP_DISABLE.getCode());
            return;
        }

        //检查任务数据
        PlayerQuestInfo quest = _commiter.getUserData().getQuestComponent().lookupQuest(_msg.getQuestId());
        if (null == quest)
        {
            _commiter.commitFailRes(QuestErr.QUEST_NOT_EXIST.getCode());
            return;
        }

        //直接修改任务进度
        if (!_commiter.getUserData().getQuestComponent().chgTargetCount(_msg.getQuestId(), _msg.getQuestStepId()
                , _msg.getQuestStepTargetId(), _msg.getChangeCount(), ENCounterDealType.ADD, context))
        {
            _commiter.commitFailRes(QuestErr.QUEST_CLIENT_CHG_COUNT_FAIL.getCode());
            return;
        }

        //返回协议
        _commiter.commitSucRes(US2GCWriter_028_QuestOp.make_004_RetAddClientTargetCount());
        
        //日志数据
        Opt028004QuestAddTargetCountBO optBo = new Opt028004QuestAddTargetCountBO();
        optBo.setQuestId(getUSServer().getBM(), _msg.getQuestId());
        optBo.setStep(getUSServer().getBM(), _msg.getQuestStepId());
        optBo.setTarget(getUSServer().getBM(), _msg.getQuestStepTargetId());
        optBo.setChgCount(getUSServer().getBM(), _msg.getChangeCount());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
