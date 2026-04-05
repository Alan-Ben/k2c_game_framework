using ALBasicProtocolPack;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_039_055_OnMarsEnergyChg : NPSubDealer<GS2GC_039_055_OnMarsEnergyChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_039_055_OnMarsEnergyChg _createProtocolObj()
        {
            return new GS2GC_039_055_OnMarsEnergyChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_039_055_OnMarsEnergyChg _msg)
        {
		    NPPlayer.instance.specialItemComp.marsEnergyData._onMarsEnergyInfoChg(_msg);	
        }
    }
}