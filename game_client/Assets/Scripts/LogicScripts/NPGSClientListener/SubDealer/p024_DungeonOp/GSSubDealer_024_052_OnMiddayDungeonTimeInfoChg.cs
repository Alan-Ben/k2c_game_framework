using ALBasicProtocolPack;
using GS2GC.p024_DungeonOp;

namespace GOE
{
    /// <summary>
    /// 午间副本时间推送
    /// </summary>
    public class GSSubDealer_024_052_OnMiddayDungeonTimeInfoChg : NPSubDealer<GS2GC_024_052_OnMiddayDungeonTimeInfoChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_024_052_OnMiddayDungeonTimeInfoChg _createProtocolObj()
        {
            return new GS2GC_024_052_OnMiddayDungeonTimeInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_024_052_OnMiddayDungeonTimeInfoChg _msg)
        {
			NPPlayer.instance.middayDungeonComp.onMiddayDungeonTimeInfoChg(_msg);
        }
    }
}