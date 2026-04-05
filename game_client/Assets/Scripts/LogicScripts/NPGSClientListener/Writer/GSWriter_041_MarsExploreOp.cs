using GC2GS.p031_RankOp;
using GC2GS.p041_MarsExploreOp;
using System.Collections.Generic;

namespace GOE
{
    //排行榜相关
    public static class GSWriter_041_MarsExploreOp
    {
        /// <summary>
        /// 请求常驻排行榜信息
        /// </summary>
        public static GC2GS_041_013_ReqNoticeMarsMine make_013_ReqNoticeMarsMine(long _mineInstanceId)
        {
            GC2GS_041_013_ReqNoticeMarsMine protocol = new GC2GS_041_013_ReqNoticeMarsMine(_mineInstanceId, 0);
            return protocol;
        }

        /// <summary>
        /// 对常驻排行榜点赞
        /// </summary>
        public static GC2GS_041_013_ReqNoticeMarsMine make_013_ReqNoticeMarsMine(long _mineInstanceId, long _stateSerialize)
        {
            GC2GS_041_013_ReqNoticeMarsMine protocol = new GC2GS_041_013_ReqNoticeMarsMine(_mineInstanceId, _stateSerialize);
            return protocol;
        }

    }
}