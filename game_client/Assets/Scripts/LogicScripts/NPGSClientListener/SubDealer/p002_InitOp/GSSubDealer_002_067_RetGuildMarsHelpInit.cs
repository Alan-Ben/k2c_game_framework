using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 联盟火星求助初始化回包
    /// </summary>
    public class GSSubDealer_002_067_RetGuildMarsHelpInit : NPSubDealer<GS2GC_002_067_RetGuildMarsHelpInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_067_RetGuildMarsHelpInit _createProtocolObj()
        {
            return new GS2GC_002_067_RetGuildMarsHelpInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_067_RetGuildMarsHelpInit _msg)
        {
            NPPlayer.instance.guildMarsHelpComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.guildMarsHelpComp.retGuildMarsHelpInit(_msg);
            });
        }
    }
}
