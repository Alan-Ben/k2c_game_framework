using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 联盟建造次数信息变更
    /// </summary>
    public class GSSubDealer_032_067_OnGuildConstructListChg : NPSubDealer<GS2GC_032_067_OnGuildConstructListChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_067_OnGuildConstructListChg _createProtocolObj()
        {
            return new GS2GC_032_067_OnGuildConstructListChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_067_OnGuildConstructListChg _msg)
        {
            NPPlayer.instance.guildComp.onGuildConstructListChg(_msg);
        }
    }
}