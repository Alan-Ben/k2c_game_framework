package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_022_ReqSetCuteActor;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_022_ReqSetCuteActor extends NPUserMsgDealer<GC2GS_004_022_ReqSetCuteActor>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_022_ReqSetCuteActor _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //如果传入的气泡框id为0，则是卸下气泡框，不进行判断
        if (_msg.getCuteActorId() != 0)
        {
            //判断称号是否有效
            if (!userData.getCuteActorComponent().checkExpiredItemEnable(_msg.getCuteActorId()))
            {
                _commiter.commitFailRes(PlayerErr.PLAYER_CUTE_ACTOR_NOT_ENABLE.getCode());
                return;
            }
        }

        //设置玩家气泡框
        userData.getPlayerComponent().setParam(ENPPlayerParam.CUTE_ACTOR, _msg.getCuteActorId());

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_022_RetSetCuteActor());
    }
}
