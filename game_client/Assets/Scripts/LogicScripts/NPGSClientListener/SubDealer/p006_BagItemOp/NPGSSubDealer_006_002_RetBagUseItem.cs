
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSSubDealer_006_002_RetBagUseItem : NPSubDealer<GS2GC.p006_BagItemOp.GS2GC_006_002_RetBagUseItem>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p006_BagItemOp.GS2GC_006_002_RetBagUseItem _createProtocolObj()
        {
            return new GS2GC.p006_BagItemOp.GS2GC_006_002_RetBagUseItem();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p006_BagItemOp.GS2GC_006_002_RetBagUseItem _msg)
        {
            WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_USE);
        }
    }
}
