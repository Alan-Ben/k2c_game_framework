using ALBasicProtocolPack;
using GS2GC.p024_DungeonOp;

namespace GOE
{
    /// <summary>
    /// 晚间副本活动时间变化
    /// </summary>
    public class GSSubDealer_024_062_OnEveningDungeonTimeInfoChg : NPSubDealer<GS2GC_024_062_OnEveningDungeonTimeInfoChg>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_024_062_OnEveningDungeonTimeInfoChg _createProtocolObj()
        {
            return new GS2GC_024_062_OnEveningDungeonTimeInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_024_062_OnEveningDungeonTimeInfoChg _msg)
        {
            NPPlayer.instance.eveningDungeonComp.OnEveningDungeonTimeInfoChg(_msg);
        }
    }
}