using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 贸易站初始化
    /// </summary>
    public class GSSubDealer_002_031_RetPlayerStationInit : NPSubDealer<GS2GC_002_031_RetPlayerStationInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_031_RetPlayerStationInit _createProtocolObj()
        {
            return new GS2GC_002_031_RetPlayerStationInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_031_RetPlayerStationInit _msg)
        {
            NPPlayer.instance.stationComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.stationComp.retStationInit(_msg);
            });
        }
    }
}