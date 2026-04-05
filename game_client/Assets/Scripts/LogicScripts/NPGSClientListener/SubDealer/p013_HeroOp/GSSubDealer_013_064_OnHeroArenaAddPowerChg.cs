using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 大臣竞技场额外加成实力推送
    /// </summary>
    public class GSSubDealer_013_064_OnHeroArenaAddPowerChg : NPSubDealer<GS2GC_013_064_OnHeroArenaAddPowerChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_064_OnHeroArenaAddPowerChg _createProtocolObj()
        {
            return new GS2GC_013_064_OnHeroArenaAddPowerChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_064_OnHeroArenaAddPowerChg _msg)
        {
            NPPlayer.instance.heroComponent.onHeroArenaAddPowerChg(_msg);
        }
    }
}