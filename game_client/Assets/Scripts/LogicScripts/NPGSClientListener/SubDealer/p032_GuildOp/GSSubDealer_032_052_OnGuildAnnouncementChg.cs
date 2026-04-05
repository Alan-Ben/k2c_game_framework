using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 联盟公告变更
    /// </summary>
    public class GSSubDealer_032_052_OnGuildAnnouncementChg : NPSubDealer<GS2GC_032_052_OnGuildAnnouncementChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_052_OnGuildAnnouncementChg _createProtocolObj()
        {
            return new GS2GC_032_052_OnGuildAnnouncementChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_052_OnGuildAnnouncementChg _msg)
        {
			NPPlayer.instance.guildComp.onGuildAnnouncementChg(_msg);
        }
    }
}