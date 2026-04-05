package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_004_ReqInnDishUnlock;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_004_ReqInnDishUnlock extends NPUserMsgDealer<GC2GS_034_004_ReqInnDishUnlock>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_004_ReqInnDishUnlock _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_UNLOCK_DISH);

        Result result = userData.getInnComponent().getDishMgr().unlockDish(_msg.getDishId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_004_RetInnDishUnlock());
    }
}
