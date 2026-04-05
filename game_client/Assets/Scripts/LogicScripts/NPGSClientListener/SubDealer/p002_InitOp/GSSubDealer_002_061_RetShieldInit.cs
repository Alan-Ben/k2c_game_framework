using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 已屏蔽玩家CID列表
    /// </summary>
    public class GSSubDealer_002_061_RetShieldInit : NPSubDealer<GS2GC_002_061_RetShieldInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_061_RetShieldInit _createProtocolObj()
        {
            return new GS2GC_002_061_RetShieldInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_061_RetShieldInit _msg)
        {
            NPPlayer.instance.friendsComp.retShieldInit(_msg);
        }
    }
}