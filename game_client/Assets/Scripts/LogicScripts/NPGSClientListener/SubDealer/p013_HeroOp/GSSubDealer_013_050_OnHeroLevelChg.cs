using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 骑士升级
    /// </summary>
    public class GSSubDealer_013_050_OnHeroLevelChg : NPSubDealer<GS2GC_013_050_OnHeroLevelChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_050_OnHeroLevelChg _createProtocolObj()
        {
            return new GS2GC_013_050_OnHeroLevelChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_050_OnHeroLevelChg _msg)
        {
		    NPPlayer.instance.heroComponent.onHeroLevelChg(_msg);
        }
    }
}