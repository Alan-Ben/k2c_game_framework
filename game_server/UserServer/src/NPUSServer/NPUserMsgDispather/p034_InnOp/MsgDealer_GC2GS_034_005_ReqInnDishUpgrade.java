package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_005_ReqInnDishUpgrade;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Dish.InnDishInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_005_ReqInnDishUpgrade extends NPUserMsgDealer<GC2GS_034_005_ReqInnDishUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_005_ReqInnDishUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        InnDishInfo dishInfo = userData.getInnComponent().getDishMgr().lookupDish(_msg.getDishId());
        if (null == dishInfo)
        {
            _commiter.commitFailRes(InnErr.INN_DISH_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_DISH_UPGRADE);

        Result result = dishInfo.upgrade(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_005_RetInnDishUpgrade());
    }
}
