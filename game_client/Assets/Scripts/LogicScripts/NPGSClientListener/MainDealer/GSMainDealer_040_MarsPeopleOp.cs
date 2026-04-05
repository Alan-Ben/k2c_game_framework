using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_040_MarsPeopleOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_040_MarsPeopleOp()
        : base(40, 80)
        {
            regDealer(new GSSubDealer_040_050_OnMarsPeopleNumChg());
            regDealer(new GSSubDealer_040_051_OnIntelligentChg());
            regDealer(new GSSubDealer_040_052_OnSatisfactionChg());
            regDealer(new GSSubDealer_040_053_OnLetterChg());
            regDealer(new GSSubDealer_040_054_OnLetterDel());
            regDealer(new GSSubDealer_040_055_OnHelpChg());
            regDealer(new GSSubDealer_040_056_OnHelpDel());
            regDealer(new GSSubDealer_040_057_OnMarsEventTrigger());
            regDealer(new GSSubDealer_040_058_OnPeopleImmigrantAdd());
            regDealer(new GSSubDealer_040_059_OnPeopleImmigrantDel());
            regDealer(new GSSubDealer_040_060_OnPeopleImmigrantCountChg());
        }
    }
}