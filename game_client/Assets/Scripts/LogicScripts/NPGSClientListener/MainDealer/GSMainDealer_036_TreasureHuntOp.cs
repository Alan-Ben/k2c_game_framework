using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_036_TreasureHuntOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_036_TreasureHuntOp()
        : base(36, 70)
        {
            regDealer(new GSSubDealer_036_051_OnTreasureHuntTreasureAdd());
			regDealer(new GSSubDealer_036_052_OnTreasureHuntTreasureLevelChg());
            regDealer(new GSSubDealer_036_053_OnTreasureHuntOreAdd());
            regDealer(new GSSubDealer_036_054_OnTreasureHuntOreNumChg());
            regDealer(new GSSubDealer_036_055_OnTreasureHuntOreSkillChg());
            regDealer(new GSSubDealer_036_056_OnTreasureCompositeChg());
            regDealer(new GSSubDealer_036_057_OnTreasureStationChg());
            regDealer(new GSSubDealer_036_058_OnTreasureHuntOreMaxRecordChg());
            regDealer(new GSSubDealer_036_059_OnTreasureHuntOreHadDrawRecordRewardChg());
            regDealer(new GSSubDealer_036_061_OnTreasureHuntTreasureOutputChg());
        }
    }
}