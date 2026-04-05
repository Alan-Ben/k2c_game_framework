package MGClient.Cmd.Cmds;

import GC2GS.p031_RankOp.GC2GS_031_002_ReqRankFixedLike;
import GC2GS.p031_RankOp.GC2GS_031_004_ReqRankFixedAKeyLike;
import GS2GC.p031_RankOp.GS2GC_031_002_RetRankFixedLike;
import GS2GC.p031_RankOp.GS2GC_031_004_RetRankFixedAKeyLike;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;
import NPCommon.Log.CommLog;

@Commander(comment = "排行榜命令", name = "rank")
public class CmdRank extends CmdBase
{
    @Command(comment = "随机点赞[常驻排行榜id]")
    public void randomLike(long _refId)
    {
        GC2GS_031_002_ReqRankFixedLike proto = new GC2GS_031_002_ReqRankFixedLike();
        proto.setRankFixedId(_refId);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_031_002_RetRankFixedLike>(GS2GC_031_002_RetRankFixedLike.class)
        {
            @Override
            public void handle(GS2GC_031_002_RetRankFixedLike _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }

    @Command(comment = "一键点赞")
    public void aKeyRandomLike()
    {
        GC2GS_031_004_ReqRankFixedAKeyLike proto = new GC2GS_031_004_ReqRankFixedAKeyLike();
        proto.getRankFixedIdList().add(101L);
        proto.getRankFixedIdList().add(201L);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_031_004_RetRankFixedAKeyLike>(GS2GC_031_004_RetRankFixedAKeyLike.class)
        {
            @Override
            public void handle(GS2GC_031_004_RetRankFixedAKeyLike _response)
            {
                CommLog.info(_response.toString());
            }
        });
    }
}
