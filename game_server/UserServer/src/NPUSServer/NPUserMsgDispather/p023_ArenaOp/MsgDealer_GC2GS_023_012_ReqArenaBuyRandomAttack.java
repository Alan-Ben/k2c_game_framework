package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_012_ReqArenaBuyRandomAttack;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_012_ReqArenaBuyRandomAttack extends NPUserMsgDealer<GC2GS_023_012_ReqArenaBuyRandomAttack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_012_ReqArenaBuyRandomAttack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //参数物品数量检查
        if(!userData.checkItemCount(_msg.getNum()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ARENA_BUY_ATTACK_TIMES);
        Result result = userData.getArenaComponent().buyRandomAttackNum(_msg.getNum(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_012_RetArenaBuyRandomAttack());
    }
}
