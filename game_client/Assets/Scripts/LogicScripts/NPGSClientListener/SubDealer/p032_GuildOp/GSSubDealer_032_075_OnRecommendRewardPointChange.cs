using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 推荐奖励据点变更推送
    /// </summary>
    public class GSSubDealer_032_075_OnRecommendRewardPointChange : NPSubDealer<GS2GC_032_075_OnRecommendRewardPointChange>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_075_OnRecommendRewardPointChange _createProtocolObj()
        {
            return new GS2GC_032_075_OnRecommendRewardPointChange();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_075_OnRecommendRewardPointChange _msg)
        {
            NPPlayer.instance.guildCooperateComp.onRecommendRewardPointChange(_msg);
        }
    }
}