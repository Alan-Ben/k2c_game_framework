package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_020_ReqConsortChooseDialogueOption;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatDialogueInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;

public class MsgDealer_GC2GS_015_020_ReqConsortChooseDialogueOption extends NPUserMsgDealer<GC2GS_015_020_ReqConsortChooseDialogueOption>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_020_ReqConsortChooseDialogueOption _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        RefConsortChatDialogue refDialogue = RefConsortChatDialogue.getMgr().get(_msg.getDialogueId());
        if (refDialogue == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        ConsortChatInfo chatInfo = userData.getConsortChatComponent().lookupChatInfo(refDialogue.consort_id);
        if (chatInfo == null)
        {
            _commiter.commitFailRes(ConsortErr.CHAT_DIALOGUE_NOT_TRIGGERED.getCode());
            return;
        }

        ConsortChatDialogueInfo dialogueInfo = chatInfo.lookupDialogue(_msg.getDialogueId());
        if (null == dialogueInfo)
        {
            _commiter.commitFailRes(ConsortErr.CHAT_DIALOGUE_NOT_TRIGGERED.getCode());
            return;
        }

        dialogueInfo.recordDialogueOption(_msg.getSentenceId(), _msg.getOptionId());
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_020_RetConsortChooseDialogueOption());
    }
}