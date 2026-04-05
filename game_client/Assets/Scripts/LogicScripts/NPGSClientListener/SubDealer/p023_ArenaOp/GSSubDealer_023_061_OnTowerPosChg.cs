using ALBasicProtocolPack;
using GS2GC.p023_ArenaOp;

namespace GOE
{
    /// <summary>
    /// 爬塔位置信息变更
    /// </summary>
    public class GSSubDealer_023_061_OnTowerPosChg : NPSubDealer<GS2GC_023_061_OnTowerPosChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_023_061_OnTowerPosChg _createProtocolObj()
        {
            return new GS2GC_023_061_OnTowerPosChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_023_061_OnTowerPosChg _msg)
        {
			NPPlayer.instance.towerComp.OnTowerPosChg(_msg);
        }
    }
}