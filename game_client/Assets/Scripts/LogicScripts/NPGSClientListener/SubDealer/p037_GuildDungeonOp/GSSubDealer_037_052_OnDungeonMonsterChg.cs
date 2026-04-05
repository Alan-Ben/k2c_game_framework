using ALBasicProtocolPack;
using GS2GC.p037_GuildDungeonOp;

namespace GOE
{
    /// <summary>
    /// 副本怪物数据变化
    /// </summary>
    public class GSSubDealer_037_052_OnDungeonMonsterChg : NPSubDealer<GS2GC_037_052_OnDungeonMonsterChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_037_052_OnDungeonMonsterChg _createProtocolObj()
        {
            return new GS2GC_037_052_OnDungeonMonsterChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_037_052_OnDungeonMonsterChg _msg)
        {
            NPPlayer.instance.guildDungeonComp.onDungeonMonsterChg(_msg);
        }
    }
}