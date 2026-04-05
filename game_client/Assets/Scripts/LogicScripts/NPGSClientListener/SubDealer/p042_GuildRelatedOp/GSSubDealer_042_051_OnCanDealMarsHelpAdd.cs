using ALBasicProtocolPack;
using GS2GC.p042_GuildRelatedOp;

namespace GOE
{
    /// <summary>
    /// 可以帮助的火星求助实例新增推送
    /// </summary>
    public class GSSubDealer_042_051_OnCanDealMarsHelpAdd : NPSubDealer<GS2GC_042_051_OnCanDealMarsHelpAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_042_051_OnCanDealMarsHelpAdd _createProtocolObj()
        {
            return new GS2GC_042_051_OnCanDealMarsHelpAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_042_051_OnCanDealMarsHelpAdd _msg)
        {
            NPPlayer.instance.guildMarsHelpComp.onCanDealMarsHelpAdd(_msg);
        }
    }
}
