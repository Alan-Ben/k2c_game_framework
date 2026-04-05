using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 联盟派遣信息变更
    /// </summary>
    public class GSSubDealer_032_072_OnGuildDispatchChg : NPSubDealer<GS2GC_032_072_OnGuildDispatchChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_072_OnGuildDispatchChg _createProtocolObj()
        {
            return new GS2GC_032_072_OnGuildDispatchChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_072_OnGuildDispatchChg _msg)
        {
            NPPlayer.instance.guildComp.onGuildDispatchChg(_msg);
        }
    }
}