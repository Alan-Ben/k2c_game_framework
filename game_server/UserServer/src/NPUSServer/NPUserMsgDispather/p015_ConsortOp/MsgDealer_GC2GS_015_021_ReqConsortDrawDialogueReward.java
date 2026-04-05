package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_021_ReqConsortDrawDialogueReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.ConsortChat.RefConsortChatDialogue;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatDialogueInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;

public class MsgDealer_GC2GS_015_021_ReqConsortDrawDialogueReward extends NPUserMsgDealer<GC2GS_015_021_ReqConsortDrawDialogueReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_021_ReqConsortDrawDialogueReward _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_CONSORT_DIALOGUE_REWARD);

        Result result = dialogueInfo.drawReward(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_021_RetConsortDrawDialogueReward());
    }
}