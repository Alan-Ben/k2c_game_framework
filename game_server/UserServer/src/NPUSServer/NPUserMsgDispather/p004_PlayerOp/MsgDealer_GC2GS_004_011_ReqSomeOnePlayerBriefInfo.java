package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_011_ReqSomeOnePlayerBriefInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_011_ReqSomeOnePlayerBriefInfo extends NPUserMsgDealer<GC2GS_004_011_ReqSomeOnePlayerBriefInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_011_ReqSomeOnePlayerBriefInfo _msg)
    {
        getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _msg.getCid(), new HandlerTwo<Boolean, PlayerInfo_IconShow>()
        {
            @Override
            public void handle(Boolean _isSuc, PlayerInfo_IconShow _iconShowInfo)
            {
                if (!_isSuc)
                {
                    //做失败处理
                    _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                    return;
                }

                _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_011_RetSomeOnePlayerBriefInfo(_iconShowInfo));
            }
        });
    }
}
