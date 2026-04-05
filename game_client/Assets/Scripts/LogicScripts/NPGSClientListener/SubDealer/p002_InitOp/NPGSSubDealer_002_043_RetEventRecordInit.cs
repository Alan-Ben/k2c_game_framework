using ALBasicProtocolPack;

namespace GOE
{
    //玩家行为组件初始化
    public class NPGSSubDealer_002_043_RetEventRecordInit : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_043_RetEventRecordInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_043_RetEventRecordInit _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_043_RetEventRecordInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_043_RetEventRecordInit _msg)
        {
            NPPlayer.instance.eventRecordComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.eventRecordComp.retEventRecordList(_msg);
            });
        }
    }
}
