using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    /// <summary>
    /// 情人收集初始化
    /// </summary>
    public class GSSubDealer_002_088_RetLoverCollectInit : NPSubDealer<GS2GC_002_088_RetLoverCollectInit>
    {
        protected override GS2GC_002_088_RetLoverCollectInit _createProtocolObj()
        {
            return new GS2GC_002_088_RetLoverCollectInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_088_RetLoverCollectInit _msg)
        {
            NPPlayer.instance.loverCollectComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.loverCollectComp.retLoverCollectInit(_msg);
            });
        }
    }
}
