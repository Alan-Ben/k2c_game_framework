using ALBasicProtocolPack;
using GS2GC.p032_GuildOp;

namespace GOE
{
    /// <summary>
    /// 属性据点信息变更推送
    /// </summary>
    public class GSSubDealer_032_076_OnPropertyPointInfoChange : NPSubDealer<GS2GC_032_076_OnPropertyPointInfoChange>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_032_076_OnPropertyPointInfoChange _createProtocolObj()
        {
            return new GS2GC_032_076_OnPropertyPointInfoChange();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_032_076_OnPropertyPointInfoChange _msg)
        {
            NPPlayer.instance.guildCooperateComp.onPropertyPointInfoChange(_msg);
        }
    }
}