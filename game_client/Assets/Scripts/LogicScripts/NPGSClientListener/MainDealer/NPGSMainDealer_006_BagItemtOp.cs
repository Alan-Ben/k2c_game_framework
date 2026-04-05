using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSMainDealer_006_BagItemtOp : ALBasicProtocolMainOrderDealer
    {
        public NPGSMainDealer_006_BagItemtOp() :
            base(6, 100)
        {
            regDealer(new NPGSSubDealer_006_001_RetSellBagItem());
            regDealer(new NPGSSubDealer_006_002_RetBagUseItem());
            regDealer(new NPGSSubDealer_006_050_PushBagItemInfo());
            regDealer(new NPGSSubDealer_006_051_PushRemoveBagItem());
        }
    }
}
