using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 伙伴游历额外加成实力推送
    /// </summary>
    public class GSSubDealer_013_069_OnHeroTravelAddPowerChg : NPSubDealer<GS2GC_013_069_OnHeroTravelAddPowerChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_069_OnHeroTravelAddPowerChg _createProtocolObj()
        {
            return new GS2GC_013_069_OnHeroTravelAddPowerChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_069_OnHeroTravelAddPowerChg _msg)
        {
            NPPlayer.instance.heroComponent.onHeroTravelAddPowerChg(_msg);
        }
    }
}