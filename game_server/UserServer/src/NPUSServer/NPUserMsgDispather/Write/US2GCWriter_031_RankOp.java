package NPUSServer.NPUserMsgDispather.Write;

import Common.RankObj.RankFixed_LikeResult;
import Common.RankObj.Rank_BaseItem;
import GS2GC.p031_RankOp.*;

import java.util.List;

/**
 * 031 协议writer
 */
public class US2GCWriter_031_RankOp
{
    public static GS2GC_031_001_RetRankFixedBaseList make_001_RetRankFixedBaseList(List<Rank_BaseItem> _rankBaseList)
    {
        GS2GC_031_001_RetRankFixedBaseList proto = new GS2GC_031_001_RetRankFixedBaseList();
        proto.getBaseItemlist().addAll(_rankBaseList);
        return proto;
    }

    public static GS2GC_031_002_RetRankFixedLike make_002_RetRankFixedLike(RankFixed_LikeResult _likeResult)
    {
        if (_likeResult == null)
            return new GS2GC_031_002_RetRankFixedLike();

        return new GS2GC_031_002_RetRankFixedLike(_likeResult);
    }

    public static GS2GC_031_003_RetRankFixedLikeScore make_003_RetRankFixedLikeScore(long _likeScore)
    {
        return new GS2GC_031_003_RetRankFixedLikeScore(_likeScore);
    }

    public static GS2GC_031_004_RetRankFixedAKeyLike make_004_RetRankFixedAKeyLike(List<RankFixed_LikeResult> _likeResultList)
    {
        GS2GC_031_004_RetRankFixedAKeyLike proto = new GS2GC_031_004_RetRankFixedAKeyLike();
        proto.getLikeResultList().addAll(_likeResultList);
        return proto;
    }

    public static GS2GC_031_005_RetRankFixedInfoByRank make_005_RetRankFixedInfoByRank(Rank_BaseItem _baseInfo)
    {
        if (_baseInfo == null)
            return new GS2GC_031_005_RetRankFixedInfoByRank();

        return new GS2GC_031_005_RetRankFixedInfoByRank(_baseInfo);
    }

    public static GS2GC_031_006_RetRankFixedInfoByKey make_006_RetRankFixedInfoByKey(Rank_BaseItem _baseInfo)
    {
        if (_baseInfo == null)
            return new GS2GC_031_006_RetRankFixedInfoByKey();

        return new GS2GC_031_006_RetRankFixedInfoByKey(_baseInfo);
    }
}
