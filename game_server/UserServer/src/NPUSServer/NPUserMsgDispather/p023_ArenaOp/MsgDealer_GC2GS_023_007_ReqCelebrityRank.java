package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.ArenaObj.Arena_CelebrityRankInfo;
import GC2GS.p023_ArenaOp.GC2GS_023_007_ReqCelebrityRank;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_007_ReqCelebrityRank extends NPUserMsgDealer<GC2GS_023_007_ReqCelebrityRank>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_007_ReqCelebrityRank _msg)
    {
        List<Arena_CelebrityRankInfo> rankList = getUSServer().getArenaCelebrityRankMgr().getRankList(_msg.getDbId());
        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_007_RetCelebrityRank(rankList));
    }
}
