using ALBasicProtocolPack;
using GS2GC.p042_GuildRelatedOp;

namespace GOE
{
    /// <summary>
    /// 玩家自身求助被处理推送
    /// </summary>
    public class GSSubDealer_042_053_OnMyMarsHelpDealed : NPSubDealer<GS2GC_042_053_OnMyMarsHelpDealed>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_042_053_OnMyMarsHelpDealed _createProtocolObj()
        {
            return new GS2GC_042_053_OnMyMarsHelpDealed();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_042_053_OnMyMarsHelpDealed _msg)
        {
            NPPlayer.instance.guildMarsHelpComp.onMyMarsHelpDealed(_msg);
        }
    }
}
