package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_087_ReqPlayerRoomSkin;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/**
 * 请求玩家房间皮肤数据
 */
public class MsgDealer_GC2GS_002_085_ReqPlayerRoomSkin extends NPUserMsgDealer<GC2GS_002_087_ReqPlayerRoomSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_087_ReqPlayerRoomSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_085_RetPlayerRoomSkin(userData));
    }
}
