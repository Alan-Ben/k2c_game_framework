package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_010_ReqDrawGuestHandbookReward;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Guest.InnGuestInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_010_ReqDrawGuestHandbookReward extends NPUserMsgDealer<GC2GS_034_010_ReqDrawGuestHandbookReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_010_ReqDrawGuestHandbookReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        InnGuestInfo guestInfo = userData.getInnComponent().getGuestMgr().lookupGuestInfo(_msg.getGuestId());
        if (null == guestInfo)
        {
            _commiter.commitFailRes(InnErr.INN_GUEST_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_DRAW_GUEST_HANDBOOK_REWARD);

        Result result = guestInfo.drawHandbookReward(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_010_RetDrawGuestHandbookReward());
    }
}
