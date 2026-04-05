using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 爬塔初始化协议
    /// </summary>
    public class GSSubDealer_002_044_RetTowerInit : NPSubDealer<GS2GC_002_044_RetTowerInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_044_RetTowerInit _createProtocolObj()
        {
            return new GS2GC_002_044_RetTowerInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_044_RetTowerInit _msg)
        {
			NPPlayer.instance.towerComp.retTowerInit(_msg);
        }
    }
}