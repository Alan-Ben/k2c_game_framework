package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_007_ReqSetIconBgk;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_007_ReqSetIconBgk extends NPUserMsgDealer<GC2GS_004_007_ReqSetIconBgk>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_007_ReqSetIconBgk _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //如果传入的头像框id为0，则是卸下头像框，不进行判断
        if (_msg.getIconBgkId() != 0)
        {
            //判断头像框是否有效
            if (!userData.getIconBgkComponent().checkExpiredItemEnable(_msg.getIconBgkId()))
            {
                _commiter.commitFailRes(PlayerErr.PLAYER_ICON_BGK_NOT_ENABLE.getCode());
                return;
            }
        }

        //设置玩家称号
        userData.getPlayerComponent().setParam(ENPPlayerParam.ICON_BGK, _msg.getIconBgkId());

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_007_SetIconBgkRes());
    }
}
