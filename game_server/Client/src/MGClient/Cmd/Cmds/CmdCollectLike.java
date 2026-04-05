package MGClient.Cmd.Cmds;

import GC2GS.p004_PlayerOp.GC2GS_004_033_ReqTodayLikeCidInfo;
import GC2GS.p004_PlayerOp.GC2GS_004_034_ReqPlayerDetailLike;
import GC2GS.p004_PlayerOp.GC2GS_004_035_ReqSelfLikeCount;
import GS2GC.p004_PlayerOp.GS2GC_004_033_RetTodayLikeCidInfo;
import GS2GC.p004_PlayerOp.GS2GC_004_034_RetPlayerDetailLike;
import GS2GC.p004_PlayerOp.GS2GC_004_035_RetSelfLikeCount;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "集赞", name = "collectLike")
public class CmdCollectLike extends CmdBase
{
    @Command(comment = "今日点赞记录")
    public void todayLikeInfo()
    {
        GC2GS_004_033_ReqTodayLikeCidInfo proto = new GC2GS_004_033_ReqTodayLikeCidInfo();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_004_033_RetTodayLikeCidInfo>(GS2GC_004_033_RetTodayLikeCidInfo.class)
        {
            @Override
            public void handle(GS2GC_004_033_RetTodayLikeCidInfo _response)
            {
            }
        });
    }

    @Command(comment = "玩家详情点赞")
    public void detailLike(long _cid)
    {
        GC2GS_004_034_ReqPlayerDetailLike proto = new GC2GS_004_034_ReqPlayerDetailLike();
        proto.setCid(_cid);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_004_034_RetPlayerDetailLike>(GS2GC_004_034_RetPlayerDetailLike.class)
        {
            @Override
            public void handle(GS2GC_004_034_RetPlayerDetailLike _response)
            {
            }
        });
    }

    @Command(comment = "个人集赞数据")
    public void selfCount()
    {
        GC2GS_004_035_ReqSelfLikeCount proto = new GC2GS_004_035_ReqSelfLikeCount();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_004_035_RetSelfLikeCount>(GS2GC_004_035_RetSelfLikeCount.class)
        {
            @Override
            public void handle(GS2GC_004_035_RetSelfLikeCount _response)
            {
            }
        });
    }

}
