using ALBasicProtocolPack;
using GS2GC.p018_PlayerSkinOp;

namespace GOE
{
    /// <summary>
    /// 组合称号后缀变更
    /// </summary>
    public class GSSubDealer_018_052_OnComboTitleSfxChg : NPSubDealer<GS2GC_018_052_OnComboTitleSfxChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_018_052_OnComboTitleSfxChg _createProtocolObj()
        {
            return new GS2GC_018_052_OnComboTitleSfxChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_018_052_OnComboTitleSfxChg _msg)
        {
			NPPlayer.instance.titleComp.onComboTitleSfxChg(_msg);
        }
    }
}