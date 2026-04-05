using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 新增订单
    /// </summary>
    public class GSSubDealer_004_066_OnOrderAdd : NPSubDealer<GS2GC_004_066_OnOrderAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_066_OnOrderAdd _createProtocolObj()
        {
            return new GS2GC_004_066_OnOrderAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_066_OnOrderAdd _msg)
        {
            NPPlayer.instance.payOrderComp.onOrderAdd(_msg);
        }
    }
}