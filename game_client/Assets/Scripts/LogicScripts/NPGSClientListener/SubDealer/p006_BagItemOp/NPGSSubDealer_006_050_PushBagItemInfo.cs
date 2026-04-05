
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSSubDealer_006_050_PushBagItemInfo : NPSubDealer<GS2GC.p006_BagItemOp.GS2GC_006_050_PushBagItemInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p006_BagItemOp.GS2GC_006_050_PushBagItemInfo _createProtocolObj()
        {
            return new GS2GC.p006_BagItemOp.GS2GC_006_050_PushBagItemInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p006_BagItemOp.GS2GC_006_050_PushBagItemInfo _msg)
        {
            if(_msg == null)
                return;

            //更新背包物品
            NPPlayer.instance.bagComp.updateItem(_msg.getBagItemInfo());
        }
    }
}
