using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_013_HeroOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_013_HeroOp()
        : base(13, 70)
        {
			regDealer(new GSSubDealer_013_010_RetBuildingPlaceHero());
            regDealer(new GSSubDealer_013_050_OnHeroLevelChg());
			regDealer(new GSSubDealer_013_052_OnHeroTalentSkillChg());
			regDealer(new GSSubDealer_013_054_OnHeroBusinessSkillChg());
			regDealer(new GSSubDealer_013_055_OnGainHero());
			regDealer(new GSSubDealer_013_056_OnHeroStarChg());
			regDealer(new GSSubDealer_013_057_OnHeroSkinChg());
			regDealer(new GSSubDealer_013_058_OnHeroStepChg());
			regDealer(new GSSubDealer_013_059_OnHeroWearSkinChg());
			regDealer(new GSSubDealer_013_060_OnHeroSuitChg());
			regDealer(new GSSubDealer_013_061_OnHeroHaloChg());
			regDealer(new GSSubDealer_013_062_OnHeroPlaceBuildingChg());
			regDealer(new GSSubDealer_013_063_OnHeroItemAddPowerChg());
			regDealer(new GSSubDealer_013_064_OnHeroArenaAddPowerChg());
			regDealer(new GSSubDealer_013_065_OnEquipAdd());
			regDealer(new GSSubDealer_013_066_OnEquipBaseChg());
			regDealer(new GSSubDealer_013_067_OnEquipSkillChg());
			regDealer(new GSSubDealer_013_068_OnEquipRemove());
			regDealer(new GSSubDealer_013_069_OnHeroTravelAddPowerChg());
        }
    }
}