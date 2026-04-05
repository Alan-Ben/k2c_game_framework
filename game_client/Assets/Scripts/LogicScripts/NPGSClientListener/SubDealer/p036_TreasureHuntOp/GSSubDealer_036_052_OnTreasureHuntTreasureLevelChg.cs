using ALBasicProtocolPack;
using GS2GC.p035_MuseumOp;
using GS2GC.p036_TreasureHuntOp;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物等级变化推送
    /// </summary>
    public class GSSubDealer_036_052_OnTreasureHuntTreasureLevelChg : NPSubDealer<GS2GC_036_052_OnTreasureHuntTreasureLevelChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_036_052_OnTreasureHuntTreasureLevelChg _createProtocolObj()
        {
            return new GS2GC_036_052_OnTreasureHuntTreasureLevelChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_036_052_OnTreasureHuntTreasureLevelChg _msg)
        {
			NPPlayer.instance.treasureHuntComponent.onTreasureHuntTreasureLevelChg(_msg);
        }
    }
}
