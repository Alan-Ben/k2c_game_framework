using ALBasicProtocolPack;
using GS2GC.p042_GuildRelatedOp;

namespace GOE
{
    /// <summary>
    /// 玩家自身求助数据变更推送
    /// </summary>
    public class GSSubDealer_042_050_OnMyMarsHelpChg : NPSubDealer<GS2GC_042_050_OnMyMarsHelpChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_042_050_OnMyMarsHelpChg _createProtocolObj()
        {
            return new GS2GC_042_050_OnMyMarsHelpChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_042_050_OnMyMarsHelpChg _msg)
        {
            NPPlayer.instance.guildMarsHelpComp.onMyMarsHelpChg(_msg);
        }
    }
}
