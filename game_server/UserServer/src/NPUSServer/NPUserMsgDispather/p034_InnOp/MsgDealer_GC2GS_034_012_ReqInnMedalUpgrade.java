package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_012_ReqInnMedalUpgrade;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_012_ReqInnMedalUpgrade extends NPUserMsgDealer<GC2GS_034_012_ReqInnMedalUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_012_ReqInnMedalUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_MEDAL_UPGRADE);

        Result result = userData.getInnComponent().medalUpgrade(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_012_RetInnMedalUpgrade());
    }
}
