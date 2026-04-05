using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 招募组件初始化
    /// </summary>
    public class GSSubDealer_002_041_RetRecuritInfo : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_041_RetRecuritInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_041_RetRecuritInfo _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_041_RetRecuritInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_041_RetRecuritInfo _msg)
        {
            NPPlayer.instance.recruitComp.dealPreInitFunc(() =>
            {
                //情人信息
                NPPlayer.instance.recruitComp.retRecuritInfo(_msg);
            });
        }
    }
}
