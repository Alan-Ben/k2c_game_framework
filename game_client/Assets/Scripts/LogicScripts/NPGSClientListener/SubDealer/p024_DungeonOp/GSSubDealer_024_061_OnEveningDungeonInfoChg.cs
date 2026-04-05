using ALBasicProtocolPack;
using GS2GC.p024_DungeonOp;

namespace GOE
{
    /// <summary>
    /// 晚间副本数据变化
    /// </summary>
    public class GSSubDealer_024_061_OnEveningDungeonInfoChg : NPSubDealer<GS2GC_024_061_OnEveningDungeonInfoChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_024_061_OnEveningDungeonInfoChg _createProtocolObj()
        {
            return new GS2GC_024_061_OnEveningDungeonInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_024_061_OnEveningDungeonInfoChg _msg)
        {
            NPPlayer.instance.eveningDungeonComp.OnEveningDungeonInfoChg(_msg);
        }
    }
}