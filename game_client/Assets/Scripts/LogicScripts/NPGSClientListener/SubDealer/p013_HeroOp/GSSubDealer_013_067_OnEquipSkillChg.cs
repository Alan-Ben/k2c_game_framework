using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 藏品技能信息变更
    /// </summary>
    public class GSSubDealer_013_067_OnEquipSkillChg : NPSubDealer<GS2GC_013_067_OnEquipSkillChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_067_OnEquipSkillChg _createProtocolObj()
        {
            return new GS2GC_013_067_OnEquipSkillChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_067_OnEquipSkillChg _msg)
        {
			NPPlayer.instance.equipComp.onEquipSkillChg(_msg);
        }
    }
}