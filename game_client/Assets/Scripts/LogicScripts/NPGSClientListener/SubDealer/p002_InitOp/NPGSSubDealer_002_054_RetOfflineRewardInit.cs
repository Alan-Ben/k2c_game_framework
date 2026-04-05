using ALBasicProtocolPack;

namespace GOE
{
    //聚会初始化
    public class NPGSSubDealer_002_054_RetOfflineRewardInit : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_054_RetOfflineRewardInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_054_RetOfflineRewardInit _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_054_RetOfflineRewardInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_054_RetOfflineRewardInit _msg)
        {
        }
    }
}
