using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_002_055_RetConsortChatInit : NPSubDealer<GS2GC_002_055_RetConsortChatInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_055_RetConsortChatInit _createProtocolObj()
        {
            return new GS2GC_002_055_RetConsortChatInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_055_RetConsortChatInit _msg)
        {
            NPPlayer.instance.consortChatComp.dealPreInitFunc(()=>
            {
                NPPlayer.instance.consortChatComp.retConsortChatInit(_msg);
            });
        }
    }
}