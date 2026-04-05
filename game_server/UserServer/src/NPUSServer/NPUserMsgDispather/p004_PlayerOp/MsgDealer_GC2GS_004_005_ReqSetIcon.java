package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_005_ReqSetIcon;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_005_ReqSetIcon extends NPUserMsgDealer<GC2GS_004_005_ReqSetIcon>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_005_ReqSetIcon _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //如果传入的头像id为0，则是卸下头像，不进行判断
        if (_msg.getIconId() != 0)
        {
            //判断头像是否有效
            if (!userData.getIconComponent().checkExpiredItemEnable(_msg.getIconId()))
            {
                _commiter.commitFailRes(PlayerErr.PLAYER_ICON_NOT_ENABLE.getCode());
                return;
            }
        }

        //设置玩家称号
        userData.getPlayerComponent().setParam(ENPPlayerParam.ICON, _msg.getIconId());

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_005_SetIconRes());
    }
}
