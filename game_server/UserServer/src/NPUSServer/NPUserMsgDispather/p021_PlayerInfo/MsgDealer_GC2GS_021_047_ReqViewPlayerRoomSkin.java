package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_047_ReqViewPlayerRoomSkin;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 设置房间皮肤信息已查看
 */
public class MsgDealer_GC2GS_021_047_ReqViewPlayerRoomSkin extends NPUserMsgDealer<GC2GS_021_047_ReqViewPlayerRoomSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_047_ReqViewPlayerRoomSkin _msg)
    {
        // 获取用户对象
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 设置对应房间皮肤已查看
        userData.getRoomSkinComponent().setItemViewed(_msg.getRoomSkinId());
    }
}
