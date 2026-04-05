using ALBasicProtocolPack;
using GS2GC.p037_GuildDungeonOp;

namespace GOE
{
    /// <summary>
    /// 公会副本-出战大臣数据变更
    /// </summary>
    public class GSSubDealer_037_054_OnDungeonHeroFightChg : NPSubDealer<GS2GC_037_054_OnDungeonHeroFightChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_037_054_OnDungeonHeroFightChg _createProtocolObj()
        {
            return new GS2GC_037_054_OnDungeonHeroFightChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_037_054_OnDungeonHeroFightChg _msg)
        {
            NPPlayer.instance.guildDungeonComp.onDungeonHeroFightChg(_msg);
        }
    }
}