using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_037_GuildDungeonOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_037_GuildDungeonOp()
        : base(37, 70)
        {
            regDealer(new GSSubDealer_037_050_OnDungeonSetChg());
			regDealer(new GSSubDealer_037_051_OnDungeonInstanceChg());
			regDealer(new GSSubDealer_037_052_OnDungeonMonsterChg());
			regDealer(new GSSubDealer_037_053_OnDungeonGainedRewardChg());
			regDealer(new GSSubDealer_037_054_OnDungeonHeroFightChg());
			regDealer(new GSSubDealer_037_055_OnDungeonTagMonsterChg());
			regDealer(new GSSubDealer_037_056_OnDungeonPush());
        }
    }
}