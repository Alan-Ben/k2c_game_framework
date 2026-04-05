using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_013_062_OnHeroPlaceBuildingChg : NPSubDealer<GS2GC_013_062_OnHeroPlaceBuildingChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_062_OnHeroPlaceBuildingChg _createProtocolObj()
        {
            return new GS2GC_013_062_OnHeroPlaceBuildingChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_062_OnHeroPlaceBuildingChg _msg)
        {
			NPPlayer.instance.heroComponent.onHeroPlaceBuildingChg(_msg);
        }
    }
}