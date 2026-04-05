package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_011_ReqDrawSpecialGuestHandbookReward;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.SpecialGuest.InnSpecialGuestInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_011_ReqDrawSpecialGuestHandbookReward extends NPUserMsgDealer<GC2GS_034_011_ReqDrawSpecialGuestHandbookReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_011_ReqDrawSpecialGuestHandbookReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        InnSpecialGuestInfo guestInfo = userData.getInnComponent().getSpecialGuestMgr().lookupSpecialGuestInfo(_msg.getSpecialGuestId());
        if (null == guestInfo)
        {
            _commiter.commitFailRes(InnErr.INN_SPECIAL_GUEST_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_DRAW_SPECIAL_GUEST_HANDBOOK_REWARD);

        Result result = guestInfo.drawHandbookReward(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_011_RetDrawSpecialGuestHandbookReward());
    }
}
