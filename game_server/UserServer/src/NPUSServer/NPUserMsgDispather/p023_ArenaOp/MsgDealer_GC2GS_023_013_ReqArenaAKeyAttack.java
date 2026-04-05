package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_013_ReqArenaAKeyAttack;
import GS2GC.p023_ArenaOp.GS2GC_023_013_RetArenaAKeyAttack;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_013_ReqArenaAKeyAttack extends NPUserMsgDealer<GC2GS_023_013_ReqArenaAKeyAttack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_013_ReqArenaAKeyAttack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查是否解锁
        if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().arena_one_key_attack_simple_unlock_id, userData, null))
        {
            _commiter.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ARENA_AKEY_ATTACK);

        GS2GC_023_013_RetArenaAKeyAttack proto = US2GCWriter_023_ArenaOp.make_013_RetArenaAKeyAttack();
        Result result = userData.getArenaComponent().aKeyAttack(_msg.getBuffType(), proto, context);
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
