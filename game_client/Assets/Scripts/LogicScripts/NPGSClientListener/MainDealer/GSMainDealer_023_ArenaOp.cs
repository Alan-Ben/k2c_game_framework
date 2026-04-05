using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_023_ArenaOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_023_ArenaOp()
        : base(23, 70)
        {
            regDealer(new GSSubDealer_023_051_OnArenaBattleInfoChg());
			regDealer(new GSSubDealer_023_052_OnArenaBaseInfoChg());
			regDealer(new GSSubDealer_023_054_OnArenaBattleReset());
			regDealer(new GSSubDealer_023_061_OnTowerPosChg());
			regDealer(new GSSubDealer_023_062_OnTowerResearchActivePosChg());
			regDealer(new GSSubDealer_023_063_OnTowerResearchRewardDraw());
			regDealer(new GSSubDealer_023_064_OnTowerHighestPosHadReachChg());
        }
    }
}