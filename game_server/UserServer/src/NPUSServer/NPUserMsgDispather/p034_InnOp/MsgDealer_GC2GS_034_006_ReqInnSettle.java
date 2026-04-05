package NPUSServer.NPUserMsgDispather.p034_InnOp;

import Common.InnObj.Inn_SettleInfo;
import GC2GS.p034_InnOp.GC2GS_034_006_ReqInnSettle;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_006_ReqInnSettle extends NPUserMsgDealer<GC2GS_034_006_ReqInnSettle>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_006_ReqInnSettle _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_SETTLE);

        ResultOne<Inn_SettleInfo> result = userData.getInnComponent().settle(_msg.getGuestNum(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_006_RetInnSettle(result.getData()));
    }
}
