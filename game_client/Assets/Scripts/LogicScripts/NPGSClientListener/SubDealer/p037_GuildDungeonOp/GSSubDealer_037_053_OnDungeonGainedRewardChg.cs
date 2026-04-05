using ALBasicProtocolPack;
using GS2GC.p037_GuildDungeonOp;

namespace GOE
{
    /// <summary>
    /// 已领取奖励的副本怪物数据列表
    /// </summary>
    public class GSSubDealer_037_053_OnDungeonGainedRewardChg : NPSubDealer<GS2GC_037_053_OnDungeonGainedRewardChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_037_053_OnDungeonGainedRewardChg _createProtocolObj()
        {
            return new GS2GC_037_053_OnDungeonGainedRewardChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_037_053_OnDungeonGainedRewardChg _msg)
        {
            NPPlayer.instance.guildDungeonComp.onDungeonGainedRewardChg(_msg);
        }
    }
}