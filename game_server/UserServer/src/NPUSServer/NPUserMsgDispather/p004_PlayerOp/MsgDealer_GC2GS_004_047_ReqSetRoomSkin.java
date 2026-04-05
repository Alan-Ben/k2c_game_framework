package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_047_ReqSetRoomSkin;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/**
 * 设置玩家当前房间皮肤请求处理器
 */
public class MsgDealer_GC2GS_004_047_ReqSetRoomSkin extends NPUserMsgDealer<GC2GS_004_047_ReqSetRoomSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_047_ReqSetRoomSkin _msg)
    {
        // 获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        // 如果传入的房间皮肤ID为0，则是卸下房间皮肤，不进行判断
        if (_msg.getRoomSkinId() != RefGeneral.Ref().default_player_room_skin)
        {
            // 判断房间皮肤是否有效
            if (!userData.getRoomSkinComponent().checkExpiredItemEnable(_msg.getRoomSkinId()))
            {
                _commiter.commitFailRes(PlayerErr.PLAYER_ROOM_SKIN_NOT_ENABLE.getCode());
                return;
            }
        }

        // 设置玩家房间皮肤
        userData.getPlayerComponent().setParam(ENPPlayerParam.ROOM_SKIN, _msg.getRoomSkinId());

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_047_RetSetRoomSkin());
    }
}
