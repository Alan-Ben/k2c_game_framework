using ALBasicProtocolPack;
using GS2GC.p023_ArenaOp;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗数据变更
    /// </summary>
    public class GSSubDealer_023_051_OnArenaBattleInfoChg : NPSubDealer<GS2GC_023_051_OnArenaBattleInfoChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_023_051_OnArenaBattleInfoChg _createProtocolObj()
        {
            return new GS2GC_023_051_OnArenaBattleInfoChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_023_051_OnArenaBattleInfoChg _msg)
        {
			NPPlayer.instance.arenaComp.onArenaBattleInfoChg(_msg);
        }
    }
}