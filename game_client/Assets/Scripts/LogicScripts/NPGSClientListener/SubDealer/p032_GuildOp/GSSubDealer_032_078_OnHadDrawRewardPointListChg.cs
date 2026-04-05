using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 已领取奖励点列表变更推送
    /// </summary>
    public class GSSubDealer_032_078_OnHadDrawRewardPointListChg : NPSubDealer<GS2GC_032_078_OnHadDrawRewardPointListChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_078_OnHadDrawRewardPointListChg _createProtocolObj()
        {
            return new GS2GC_032_078_OnHadDrawRewardPointListChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_078_OnHadDrawRewardPointListChg _msg)
        {
            NPPlayer.instance.guildCooperateComp.onHadDrawRewardPointListChg(_msg);
        }
    }
}