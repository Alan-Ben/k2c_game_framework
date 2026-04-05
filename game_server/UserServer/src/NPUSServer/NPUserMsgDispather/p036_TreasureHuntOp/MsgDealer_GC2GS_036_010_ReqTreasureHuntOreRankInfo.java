package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import Common.TreasureHuntObj.TreasureHunt_OreRankItem;
import GC2GS.p036_TreasureHuntOp.GC2GS_036_010_ReqTreasureHuntOreRankInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import NPUSServer.USRank.TreasureHuntOreRank.TreasureHuntRankItem;
import NPUSServer.USRank.TreasureHuntOreRank.TreasureHuntRankList;

import java.util.List;

/**
 * 太空寻宝-矿石排行信息 协议处理类
 */
public class MsgDealer_GC2GS_036_010_ReqTreasureHuntOreRankInfo extends NPUserMsgDealer<GC2GS_036_010_ReqTreasureHuntOreRankInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_010_ReqTreasureHuntOreRankInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        TreasureHuntRankList rankList = getUSServer().getTreasureHuntRankMgr().lookupRank(_msg.getOreId());
        if (rankList == null)
        {
            _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_010_RetTreasureHuntTransOreRankInfo(null, -1));
            return;
        }

        // 获取当前用户的排行信息
        TreasureHuntRankItem myRankItem = rankList.lookupItem(userData.getCid());
        int myRank = (myRankItem != null) ? myRankItem.getRank() : -1;

        // 获取前3名排行信息
        List<TreasureHunt_OreRankItem> top3List = rankList.makeTop3ProtoList();

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_010_RetTreasureHuntTransOreRankInfo(top3List, myRank));
    }
}
