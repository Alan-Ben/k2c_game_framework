package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_009_ReqViewPlayerCuteActor;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_021_009_ReqViewPlayerCuteActor extends NPUserMsgDealer<GC2GS_021_009_ReqViewPlayerCuteActor>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_009_ReqViewPlayerCuteActor _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //设置对应Q版形象已查看
        userData.getCuteActorComponent().setItemViewed(_msg.getRefId());
    }
}
