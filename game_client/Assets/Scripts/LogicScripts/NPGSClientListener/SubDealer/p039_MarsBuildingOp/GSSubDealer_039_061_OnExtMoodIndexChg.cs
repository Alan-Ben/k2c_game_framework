using ALBasicProtocolPack;
using GS2GC.p039_MarsBuildingOp;

namespace GOE
{
    /// <summary>
    /// 火星系统-额外心情指数变化
    /// </summary>
    public class GSSubDealer_039_061_OnExtMoodIndexChg : NPSubDealer<GS2GC_039_061_OnExtMoodIndexChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_039_061_OnExtMoodIndexChg _createProtocolObj()
        {
            return new GS2GC_039_061_OnExtMoodIndexChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_039_061_OnExtMoodIndexChg _msg)
        {
			NPPlayer.instance.marsComp.buildingSubComponent._onExtMoodIndexChg(_msg);
        }
    }
}