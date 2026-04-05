using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_039_MarsBuildingOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_039_MarsBuildingOp()
        : base(39, 70)
        {
            regDealer(new GSSubDealer_039_001_RetCreateBuilding());
			regDealer(new GSSubDealer_039_002_RetUpgradeBuildingLvl());
			regDealer(new GSSubDealer_039_003_RetSetHonePowerOn());
			regDealer(new GSSubDealer_039_004_RetDispatchPeople());
			regDealer(new GSSubDealer_039_005_RetUpgradeEquipment());
			regDealer(new GSSubDealer_039_006_RetGetBuildingEnergy());
			regDealer(new GSSubDealer_039_007_RetConfirmCreateBuilding());
            regDealer(new GSSubDealer_039_008_RetConfirmUpgradeBuildingLvl());
            regDealer(new GSSubDealer_039_009_RetHomeCollectTimeChg());
            regDealer(new GSSubDealer_039_050_OnBuildingChg());
			regDealer(new GSSubDealer_039_051_OnHomeChg());
			regDealer(new GSSubDealer_039_052_OnPeopleChg());
			regDealer(new GSSubDealer_039_053_OnEquipmentChg());
			regDealer(new GSSubDealer_039_054_OnEnergyOutputChg());
			regDealer(new GSSubDealer_039_055_OnMarsEnergyChg());
			regDealer(new GSSubDealer_039_056_OnBuildingUpQueueAdd());
			regDealer(new GSSubDealer_039_057_OnBuildingUpQueueDel());
			regDealer(new GSSubDealer_039_060_OnMarsTechnologyChg());
			regDealer(new GSSubDealer_039_061_OnExtMoodIndexChg());
			regDealer(new GSSubDealer_039_062_OnItemHelpSecsChg());
        }
    }
}