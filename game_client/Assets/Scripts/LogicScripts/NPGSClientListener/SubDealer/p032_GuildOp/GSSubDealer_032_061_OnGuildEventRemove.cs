using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 联盟事件移除
    /// </summary>
    public class GSSubDealer_032_061_OnGuildEventRemove : NPSubDealer<GS2GC_032_061_OnGuildEventRemove>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_061_OnGuildEventRemove _createProtocolObj()
        {
            return new GS2GC_032_061_OnGuildEventRemove();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_061_OnGuildEventRemove _msg)
        {
			NPPlayer.instance.guildComp.onGuildEventRemove(_msg);
        }
    }
}