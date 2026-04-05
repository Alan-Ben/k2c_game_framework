using GC2GS.p031_RankOp;
using System.Collections.Generic;

namespace GOE
{
    //排行榜相关
    public static class GSWriter_031_RankOp
    {
        /// <summary>
        /// 请求常驻排行榜信息
        /// </summary>
        public static GC2GS_031_001_ReqRankFixedBaseList make_001_ReqRankFixedBaseList(long _rankFixedId,bool _isCross = false)
        {
            GC2GS_031_001_ReqRankFixedBaseList protocol = new GC2GS_031_001_ReqRankFixedBaseList(_rankFixedId, _isCross);
            return protocol;
        }

        /// <summary>
        /// 对常驻排行榜点赞
        /// </summary>
        public static GC2GS_031_002_ReqRankFixedLike make_002_ReqRankFixedLike(long _rankFixedId, bool _isCross = false)
        {
            GC2GS_031_002_ReqRankFixedLike protocol = new GC2GS_031_002_ReqRankFixedLike(_rankFixedId, _isCross);
            return protocol;
        }

        /// <summary>
        /// 请求常驻排行榜的点赞积分信息
        /// </summary>
        public static GC2GS_031_003_ReqRankFixedLikeScore make_003_ReqRankFixedLikeScore(long _rankFixedId,long _cid, bool _isCross = false)
        {
            GC2GS_031_003_ReqRankFixedLikeScore protocol = new GC2GS_031_003_ReqRankFixedLikeScore(_rankFixedId, _cid,_isCross);
            return protocol;
        }

        /// <summary>
        /// 对排行榜一键点赞
        /// </summary>
        public static GC2GS_031_004_ReqRankFixedAKeyLike make_004_ReqRankFixedAKeyLike(List<long> _rankFixedIdList, bool _isCross = false)
        {
            GC2GS_031_004_ReqRankFixedAKeyLike protocol = new GC2GS_031_004_ReqRankFixedAKeyLike(_rankFixedIdList, _isCross);
            return protocol;
        }

        /// <summary>
        /// 通过排名请求排行榜信息
        /// </summary>
        /// <param name="_rankFixedIdList"></param>
        /// <returns></returns>
        public static GC2GS_031_005_ReqRankFixedInfoByRank make_005_ReqRankFixedInfoByRank(long _rankFixedId,int _rank, bool _isCross = false)
        {
            GC2GS_031_005_ReqRankFixedInfoByRank protocol = new GC2GS_031_005_ReqRankFixedInfoByRank(_rankFixedId,_rank, _isCross);
            return protocol;
        }


        /// <summary>
        /// 通过cid/联盟id 请求排行榜信息
        /// </summary>
        /// <returns></returns>
        public static GC2GS_031_006_ReqRankFixedInfoByKey make_006_ReqRankFixedInfoByKey(long _rankFixedId, long _key, bool _isCross = false)
        {
            GC2GS_031_006_ReqRankFixedInfoByKey protocol = new GC2GS_031_006_ReqRankFixedInfoByKey(_rankFixedId, _key, _isCross);
            return protocol;
        }

    }
}