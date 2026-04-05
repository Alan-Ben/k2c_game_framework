using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 七日登录奖励变化
    /// </summary>
    public class GSSubDealer_004_058_OnLoginCountChg : NPSubDealer<GS2GC_004_058_OnLoginCountChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_058_OnLoginCountChg _createProtocolObj()
        {
            return new GS2GC_004_058_OnLoginCountChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_058_OnLoginCountChg _msg)
        {
            NPPlayer.instance.sevenDayLoginComp.OnLoginCountChg(_msg);
        }
    }
}