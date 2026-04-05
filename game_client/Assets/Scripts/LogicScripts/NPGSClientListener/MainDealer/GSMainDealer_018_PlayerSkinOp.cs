using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_018_PlayerSkinOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_018_PlayerSkinOp()
        : base(18, 70)
        {
            regDealer(new GSSubDealer_018_050_OnCommTitleChg());
			regDealer(new GSSubDealer_018_051_OnComboTitlePreChg());
			regDealer(new GSSubDealer_018_052_OnComboTitleSfxChg());
			regDealer(new GSSubDealer_018_053_OnComboTitleBgChg());
			regDealer(new GSSubDealer_018_054_OnCurTitleChg());
			regDealer(new GSSubDealer_018_060_OnPlayerSkinChg());
        }
    }
}