using ALBasicProtocolPack;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 权益卡变更推送
    /// </summary>
    public class GSSubDealer_021_054_OnPrivilegeCardChg : NPSubDealer<GS2GC_021_054_OnPrivilegeCardChg>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_021_054_OnPrivilegeCardChg _createProtocolObj()
        {
            return new GS2GC_021_054_OnPrivilegeCardChg();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_021_054_OnPrivilegeCardChg _msg)
        {
            NPPlayer.instance.privilegeCardComp.onPrivilegeCardChg(_msg);
        }
    }
}