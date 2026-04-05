using ALBasicProtocolPack;
using GS2GC.p037_GuildDungeonOp;

namespace GOE
{
    /// <summary>
    /// 公会副本-推送数据
    /// </summary>
    public class GSSubDealer_037_056_OnDungeonPush : NPSubDealer<GS2GC_037_056_OnDungeonPush>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_037_056_OnDungeonPush _createProtocolObj()
        {
            return new GS2GC_037_056_OnDungeonPush();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_037_056_OnDungeonPush _msg)
        {
            NPPlayer.instance.guildDungeonComp.onDungeonPush(_msg);
        }
    }
}