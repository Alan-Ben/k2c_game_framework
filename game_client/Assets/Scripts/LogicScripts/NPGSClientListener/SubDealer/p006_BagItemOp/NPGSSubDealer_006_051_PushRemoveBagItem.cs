
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSSubDealer_006_051_PushRemoveBagItem : NPSubDealer<GS2GC.p006_BagItemOp.GS2GC_006_051_PushRemoveBagItem>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p006_BagItemOp.GS2GC_006_051_PushRemoveBagItem _createProtocolObj()
        {
            return new GS2GC.p006_BagItemOp.GS2GC_006_051_PushRemoveBagItem();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p006_BagItemOp.GS2GC_006_051_PushRemoveBagItem _msg)
        {
            if(_msg == null)
                return;

            //移除背包物品
            NPPlayer.instance.bagComp.removeItem(_msg.getItemId());
        }
    }
}
