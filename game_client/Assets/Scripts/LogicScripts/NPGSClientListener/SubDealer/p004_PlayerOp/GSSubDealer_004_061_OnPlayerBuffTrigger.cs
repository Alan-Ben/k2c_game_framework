using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSubDealer_004_061_OnPlayerBuffTrigger : NPSubDealer<GS2GC_004_061_OnPlayerBuffTrigger>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_004_061_OnPlayerBuffTrigger _createProtocolObj()
        {
            return new GS2GC_004_061_OnPlayerBuffTrigger();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_061_OnPlayerBuffTrigger _msg)
        {
            NPPlayer.instance.playerBuffComp.onPlayerBuffTrigger(_msg);
        }
    }
}