package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.TowerObj.Tower_OpponentInfo;
import GC2GS.p023_ArenaOp.GC2GS_023_023_ReqTowerChapterList;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_023_ReqTowerChapterList extends NPUserMsgDealer<GC2GS_023_023_ReqTowerChapterList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_023_ReqTowerChapterList _msg)
    {
        List<Tower_OpponentInfo> opponentInfoList =
                getUSServer().getTowerMgr().makeForwardList(_msg.getChapterId(), _msg.getLevel(), _msg.getNum());

        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_023_RetTowerChapterList(opponentInfoList));
    }
}
