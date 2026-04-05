
using GC2GS.p008_TravelOp;

namespace GOE
{
    public static class GSWriter_008_TravelOp
    {
        /// <summary>
        /// 请求单次游历
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_001_ReqStartTravel make_001_ReqStartTravel()
        {
            GC2GS_008_001_ReqStartTravel protocol = new GC2GS_008_001_ReqStartTravel();
            return protocol;
        }
        
        /// <summary>
        /// 请求一键游历
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_002_ReqAkeyTravel make_002_ReqAkeyTravel()
        {
            GC2GS_008_002_ReqAkeyTravel protocol = new GC2GS_008_002_ReqAkeyTravel();
            return protocol;
        }
        
        /// <summary>
        /// 请求处理奖励游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_003_ReqDealRewardTravel make_003_ReqDealRewardTravel(long instanceId)
        {
            GC2GS_008_003_ReqDealRewardTravel protocol = new GC2GS_008_003_ReqDealRewardTravel(instanceId);
            return protocol;
        }
        
        /// <summary>
        /// 请求处理妃子酒馆游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_004_ReqDealConsortBarTravel make_004_ReqDealConsortBarTravel(long instanceId, long consortId, long costId)
        {
            GC2GS_008_004_ReqDealConsortBarTravel protocol = new GC2GS_008_004_ReqDealConsortBarTravel(instanceId, consortId, costId);
            return protocol;
        }
        
        /// <summary>
        /// 请求处理兑换游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_005_ReqDealChangeTravel make_005_ReqDealChangeTravel(long instanceId, bool isChange)
        {
            GC2GS_008_005_ReqDealChangeTravel protocol = new GC2GS_008_005_ReqDealChangeTravel(instanceId, isChange);
            return protocol;
        }
        
        /// <summary>
        /// 处理妃子邀约游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_006_ReqDealInvitationTravel make_006_ReqDealInvitationTravel(long instanceId, long consortId)
        {
            GC2GS_008_006_ReqDealInvitationTravel protocol = new GC2GS_008_006_ReqDealInvitationTravel(instanceId, consortId);
            return protocol;
        }
        
        /// <summary>
        /// 处理大臣加国力游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_007_ReqDealAddPowerTravel make_007_ReqDealAddPowerTravel(long instanceId, long heroId)
        {
            GC2GS_008_007_ReqDealAddPowerTravel protocol = new GC2GS_008_007_ReqDealAddPowerTravel(instanceId, heroId);
            return protocol;
        }
        
        /// <summary>
        /// 处理增加妃子好感度游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_008_ReqDealConsortLikeTravel mak_008_ReqDealConsortLikeTravel(long instanceId)
        {
            GC2GS_008_008_ReqDealConsortLikeTravel protocol = new GC2GS_008_008_ReqDealConsortLikeTravel(instanceId);
            return protocol;
        }
        
        /// <summary>
        /// 处理增加妃子亲密度游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_009_ReqDealConsortIntimacyTravel mak_009_ReqDealConsortIntimacyTravel(long instanceId)
        {
            GC2GS_008_009_ReqDealConsortIntimacyTravel protocol = new GC2GS_008_009_ReqDealConsortIntimacyTravel(instanceId);
            return protocol;
        }
        
        /// <summary>
        /// 处理增加必生卷王游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_010_ReqDealGiftedTravel mak_010_ReqDealGiftedTravel(long instanceId)
        {
            GC2GS_008_010_ReqDealGiftedTravel protocol = new GC2GS_008_010_ReqDealGiftedTravel(instanceId);
            return protocol;
        }

        /// <summary>
        /// 处理博彩游历事件
        /// </summary>
        /// <returns></returns>
        public static GC2GS_008_011_ReqDealGamblingTravel make_011_ReqDealGamblingTravel(long instanceId, int betAmount, bool isAbandon)
        {
            GC2GS_008_011_ReqDealGamblingTravel protocol = new GC2GS_008_011_ReqDealGamblingTravel(instanceId, betAmount, isAbandon);
            return protocol;
        }
    }
}