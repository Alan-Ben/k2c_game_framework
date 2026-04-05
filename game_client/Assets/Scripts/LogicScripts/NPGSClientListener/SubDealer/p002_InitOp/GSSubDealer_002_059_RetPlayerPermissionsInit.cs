using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 玩家权限初始化
    /// </summary>
    public class GSSubDealer_002_059_RetPlayerPermissionsInit : NPSubDealer<GS2GC_002_059_RetPlayerPermissionsInit>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_059_RetPlayerPermissionsInit _createProtocolObj()
        {
            return new GS2GC_002_059_RetPlayerPermissionsInit();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_059_RetPlayerPermissionsInit _msg)
        {
            NPPlayer.instance.playerPermissionsComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.playerPermissionsComp.retPlayerPermissionsInit(_msg);
            });
        }
    }
}