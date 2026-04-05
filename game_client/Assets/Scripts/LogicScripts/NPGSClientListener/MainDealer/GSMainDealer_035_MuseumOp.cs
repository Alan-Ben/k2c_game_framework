using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_035_MuseumOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_035_MuseumOp()
        : base(35, 70)
        {
            regDealer(new GSSubDealer_035_001_RetMuseumItemUpgrade());
			regDealer(new GSSubDealer_035_002_RetMuseumItemActive());
			regDealer(new GSSubDealer_035_050_OnMuseumItemAdd());
			regDealer(new GSSubDealer_035_051_OnMuseumItemChg());
        }
    }
}