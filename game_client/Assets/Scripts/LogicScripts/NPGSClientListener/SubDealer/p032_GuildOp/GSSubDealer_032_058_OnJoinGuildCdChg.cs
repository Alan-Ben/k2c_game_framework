using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 加入联盟CD变化
    /// </summary>
    public class GSSubDealer_032_058_OnJoinGuildCdChg : NPSubDealer<GS2GC_032_058_OnJoinGuildCdChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_058_OnJoinGuildCdChg _createProtocolObj()
        {
            return new GS2GC_032_058_OnJoinGuildCdChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_058_OnJoinGuildCdChg _msg)
        {
			NPPlayer.instance.guildComp.onJoinGuildCdChg(_msg);
        }
    }
}