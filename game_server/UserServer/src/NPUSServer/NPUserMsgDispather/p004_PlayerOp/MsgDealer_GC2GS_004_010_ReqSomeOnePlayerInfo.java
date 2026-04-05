package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import GC2GS.p004_PlayerOp.GC2GS_004_010_ReqSomeOnePlayerInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_010_ReqSomeOnePlayerInfo extends NPUserMsgDealer<GC2GS_004_010_ReqSomeOnePlayerInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_010_ReqSomeOnePlayerInfo _msg)
    {
        getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_CommonShow.class, _msg.getCid(), new HandlerTwo<Boolean, PlayerInfo_CommonShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_CommonShow _showInfo)
            {
                if (!_isSuc)
                {
                    //做失败处理
                    _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                    return;
                }

                _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_010_RetSomeOnePlayerInfo(_showInfo));
            }
        });
    }
}
