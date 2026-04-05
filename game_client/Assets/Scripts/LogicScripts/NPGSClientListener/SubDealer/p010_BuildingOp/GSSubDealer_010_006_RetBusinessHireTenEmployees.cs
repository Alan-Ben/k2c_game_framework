using ALBasicProtocolPack;
using GS2GC.p010_BuildingOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_010_006_RetBusinessHireTenEmployees : NPSubDealer<GS2GC_010_006_RetBusinessHireTenEmployees>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_010_006_RetBusinessHireTenEmployees _createProtocolObj()
        {
            return new GS2GC_010_006_RetBusinessHireTenEmployees();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_010_006_RetBusinessHireTenEmployees _msg)
        {
			
        }
    }
}