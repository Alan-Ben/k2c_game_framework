package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_005_ReqRoundAttack;
import GS2GC.p023_ArenaOp.GS2GC_023_005_RetRoundAttack;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_005_ReqRoundAttack extends NPUserMsgDealer<GC2GS_023_005_ReqRoundAttack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_005_ReqRoundAttack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ARENA_ROUND_ATTACK);

        GS2GC_023_005_RetRoundAttack proto = US2GCWriter_023_ArenaOp.make_005_RetRoundAttack();

        Result result = userData.getArenaComponent().roundAttack(_msg.getOpponentHeroId(), proto, context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(proto);

        //如果结算了，需要通知客户端重置
        if (userData.getArenaComponent().getBattleInfo() == null)
            userData.sendMsgToGC(US2GCWriter_023_ArenaOp.make_054_OnArenaBattleReset());
    }
}
