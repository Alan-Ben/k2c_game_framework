using ALBasicProtocolPack;
using GS2GC.p042_GuildRelatedOp;

namespace GOE
{
    /// <summary>
    /// 可以帮助的火星求助实例减少推送
    /// </summary>
    public class GSSubDealer_042_052_OnCanDealMarsHelpDel : NPSubDealer<GS2GC_042_052_OnCanDealMarsHelpDel>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_042_052_OnCanDealMarsHelpDel _createProtocolObj()
        {
            return new GS2GC_042_052_OnCanDealMarsHelpDel();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_042_052_OnCanDealMarsHelpDel _msg)
        {
            NPPlayer.instance.guildMarsHelpComp.onCanDealMarsHelpDel(_msg);
        }
    }
}
