using ALBasicProtocolPack;

namespace GOE
{
    //CD组件初始化
    public class NPGSSubDealer_002_038_RetRecordInit : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_038_RetRecordInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_038_RetRecordInit _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_038_RetRecordInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_038_RetRecordInit _msg)
        {
            NPPlayer.instance.recordComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.recordComp.retRecordList(_msg);
            });
        }
    }
}
