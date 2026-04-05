using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 联盟初始化
    /// </summary>
    public class GSSubDealer_002_063_RetGuildInit : NPSubDealer<GS2GC_002_063_RetGuildInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_063_RetGuildInit _createProtocolObj()
        {
            return new GS2GC_002_063_RetGuildInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_063_RetGuildInit _msg)
        {
			NPPlayer.instance.guildComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.guildComp.retGuildInit(_msg);
            });
        }
    }
}