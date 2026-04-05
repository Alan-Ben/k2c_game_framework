using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_010_BuildingOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_010_BuildingOp()
        : base(10, 70)
        {
            regDealer(new GSSubDealer_010_001_RetBuildingBuild());
			regDealer(new GSSubDealer_010_002_RetFarmUpgradeLvl());
			regDealer(new GSSubDealer_010_003_RetFarmClickOutput());
			regDealer(new GSSubDealer_010_004_RetBusinessUpgradeLvl());
			regDealer(new GSSubDealer_010_005_RetBusinessHireEmployee());
			regDealer(new GSSubDealer_010_006_RetBusinessHireTenEmployees());
			regDealer(new GSSubDealer_010_007_RetBusinessUnlockProduct());
			regDealer(new GSSubDealer_010_050_OnBuidingBuilt());
			regDealer(new GSSubDealer_010_051_OnFarmChg());
			regDealer(new GSSubDealer_010_052_OnBusinessChg());
			regDealer(new GSSubDealer_010_054_OnBusinessBuilt());
			regDealer(new GSSubDealer_010_055_OnFarmBuilt());
			regDealer(new GSSubDealer_010_056_OnEffectGainBusinessWorkes());
			regDealer(new GSSubDealer_010_057_OnBusinessUnlockProduct());
			regDealer(new GSSubDealer_010_058_OnFarmMultipleInfoReset());
        }
    }
}