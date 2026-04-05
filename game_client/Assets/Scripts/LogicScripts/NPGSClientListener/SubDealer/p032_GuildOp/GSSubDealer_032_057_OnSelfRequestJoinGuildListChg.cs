using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 自己请求加入联盟列表变更
    /// </summary>
    public class GSSubDealer_032_057_OnSelfRequestJoinGuildListChg : NPSubDealer<GS2GC_032_057_OnSelfRequestJoinGuildListChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_057_OnSelfRequestJoinGuildListChg _createProtocolObj()
        {
            return new GS2GC_032_057_OnSelfRequestJoinGuildListChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_057_OnSelfRequestJoinGuildListChg _msg)
        {
			NPPlayer.instance.guildComp.onSelfRequestJoinGuildListChg(_msg);
        }
    }
}