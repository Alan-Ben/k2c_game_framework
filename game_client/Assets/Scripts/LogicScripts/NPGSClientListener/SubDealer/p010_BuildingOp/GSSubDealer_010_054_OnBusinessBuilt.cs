using ALBasicProtocolPack;
using GS2GC.p010_BuildingOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_010_054_OnBusinessBuilt : NPSubDealer<GS2GC_010_054_OnBusinessBuilt>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_010_054_OnBusinessBuilt _createProtocolObj()
        {
            return new GS2GC_010_054_OnBusinessBuilt();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_010_054_OnBusinessBuilt _msg)
        {
			NPPlayer.instance.buildingComp._onBusinessBuildingBuilt(_msg);
        }
    }
}