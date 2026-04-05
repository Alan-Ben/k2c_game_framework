using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 联盟总收益更新
    /// </summary>
    public class GSSubDealer_032_062_OnGuildTotalEarningsChg : NPSubDealer<GS2GC_032_062_OnGuildTotalEarningsChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_062_OnGuildTotalEarningsChg _createProtocolObj()
        {
            return new GS2GC_032_062_OnGuildTotalEarningsChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_062_OnGuildTotalEarningsChg _msg)
        {
            NPPlayer.instance.guildComp.onGuildTotalEarningsChg(_msg);
        }
    }
}