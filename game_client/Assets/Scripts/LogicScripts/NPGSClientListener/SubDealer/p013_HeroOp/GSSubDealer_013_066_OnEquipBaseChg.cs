using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 藏品基础信息变更
    /// </summary>
    public class GSSubDealer_013_066_OnEquipBaseChg : NPSubDealer<GS2GC_013_066_OnEquipBaseChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_066_OnEquipBaseChg _createProtocolObj()
        {
            return new GS2GC_013_066_OnEquipBaseChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_066_OnEquipBaseChg _msg)
        {
			NPPlayer.instance.equipComp.onEquipBaseChg(_msg);
        }
    }
}