package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import GC2GS.p023_ArenaOp.GC2GS_023_001_ReqRandomAttackPlayer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_001_ReqRandomAttackPlayer extends NPUserMsgDealer<GC2GS_023_001_ReqRandomAttackPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_001_ReqRandomAttackPlayer _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getArenaComponent().randomOpponent(_result ->
        {
            if (!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_001_RetRandomAttackPlayer());
        });
    }
}
