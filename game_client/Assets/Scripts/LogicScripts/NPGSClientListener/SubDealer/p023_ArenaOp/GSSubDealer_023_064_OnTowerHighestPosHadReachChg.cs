using ALBasicProtocolPack;
using GS2GC.p023_ArenaOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_023_064_OnTowerHighestPosHadReachChg : NPSubDealer<GS2GC_023_064_OnTowerHighestPosHadReachChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_023_064_OnTowerHighestPosHadReachChg _createProtocolObj()
        {
            return new GS2GC_023_064_OnTowerHighestPosHadReachChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_023_064_OnTowerHighestPosHadReachChg _msg)
        {
			NPPlayer.instance.towerComp.onTowerHighestPosHadReachChg(_msg);
        }
    }
}