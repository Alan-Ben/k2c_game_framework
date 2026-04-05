using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_038_MarsOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_038_MarsOp()
        : base(38, 70)
        {
            regDealer(new GSSubDealer_038_050_OnGoToStageArrived());
			regDealer(new GSSubDealer_038_051_OnGoToAllStageDone());
        }
    }
}