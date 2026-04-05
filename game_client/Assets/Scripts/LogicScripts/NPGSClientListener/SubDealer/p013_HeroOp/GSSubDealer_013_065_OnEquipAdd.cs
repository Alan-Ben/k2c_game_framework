using ALBasicProtocolPack;
using GS2GC.p013_HeroOp;

namespace GOE
{
    /// <summary>
    /// 藏品新增推送
    /// </summary>
    public class GSSubDealer_013_065_OnEquipAdd : NPSubDealer<GS2GC_013_065_OnEquipAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_013_065_OnEquipAdd _createProtocolObj()
        {
            return new GS2GC_013_065_OnEquipAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_013_065_OnEquipAdd _msg)
        {
			NPPlayer.instance.equipComp.onEquipAdd(_msg);
        }
    }
}