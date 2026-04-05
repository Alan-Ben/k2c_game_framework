using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 骑士资质技能变更
    /// </summary>
    public class GSSubDealer_013_052_OnHeroTalentSkillChg : NPSubDealer<GS2GC_013_052_OnHeroTalentSkillChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_052_OnHeroTalentSkillChg _createProtocolObj()
        {
            return new GS2GC_013_052_OnHeroTalentSkillChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_052_OnHeroTalentSkillChg _msg)
        {
		    NPPlayer.instance.heroComponent.onHeroTalentSkillChg(_msg);
        }
    }
}