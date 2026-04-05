using ALBasicProtocolPack;
using GS2GC.p042_GuildRelatedOp;

namespace GOE
{
    /// <summary>
    /// 联盟活跃点变化数据
    /// </summary>
    public class GSSubDealer_042_058_OnGuildActivePointChg : NPSubDealer<GS2GC_042_058_OnGuildActivePointChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_042_058_OnGuildActivePointChg _createProtocolObj()
        {
            return new GS2GC_042_058_OnGuildActivePointChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_042_058_OnGuildActivePointChg _msg)
        {
            NPPlayer.instance.guildBoxComp.onGuildActivePointChg(_msg);
        }
    }
}