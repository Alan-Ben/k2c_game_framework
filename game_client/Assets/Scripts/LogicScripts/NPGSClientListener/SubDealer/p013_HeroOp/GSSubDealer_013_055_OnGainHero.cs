using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 获得骑士
    /// </summary>
    public class GSSubDealer_013_055_OnGainHero : NPSubDealer<GS2GC_013_055_OnGainHero>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_055_OnGainHero _createProtocolObj()
        {
            return new GS2GC_013_055_OnGainHero();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_055_OnGainHero _msg)
        {
		    NPPlayer.instance.heroComponent.onGainHero(_msg);
        }
    }
}