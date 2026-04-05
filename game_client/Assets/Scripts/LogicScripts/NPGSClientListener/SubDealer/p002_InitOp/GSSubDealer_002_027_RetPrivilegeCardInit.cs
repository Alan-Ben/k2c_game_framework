using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 权益卡初始化
    /// </summary>
    public class GSSubDealer_002_027_RetPrivilegeCardInit : NPSubDealer<GS2GC_002_027_RetPrivilegeCardInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_027_RetPrivilegeCardInit _createProtocolObj()
        {
            return new GS2GC_002_027_RetPrivilegeCardInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_027_RetPrivilegeCardInit _msg)
        {
			NPPlayer.instance.privilegeCardComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.privilegeCardComp.retPrivilegeCardInit(_msg);
            });
        }
    }
}