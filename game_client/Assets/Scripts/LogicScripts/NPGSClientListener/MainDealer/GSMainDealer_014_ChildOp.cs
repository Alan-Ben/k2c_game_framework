using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_014_ChildOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_014_ChildOp() :
            base(14, 70)
        {
	        regDealer(new GSSubDealer_014_001_RetSetChildName());
			regDealer(new GSSubDealer_014_002_RetTrainChild());
			regDealer(new GSSubDealer_014_003_RetAddSeatEnergy());
			regDealer(new GSSubDealer_014_004_RetSetChildGraduate());
			regDealer(new GSSubDealer_014_006_RetGetUnMarriedAdult());
			regDealer(new GSSubDealer_014_007_RetGetMarriedAdult());
			regDealer(new GSSubDealer_014_008_RetGetToMeApply());
			regDealer(new GSSubDealer_014_009_RetRefuseToMeApply());
			regDealer(new GSSubDealer_014_010_RetAkeyRefuseToMeApply());
			regDealer(new GSSubDealer_014_011_RetAgreeToMeApply());
			regDealer(new GSSubDealer_014_012_RetGetRecommendPlayerList());
			regDealer(new GSSubDealer_014_013_RetApplyToPlayer());
			regDealer(new GSSubDealer_014_014_RetApplyToGroup());
			regDealer(new GSSubDealer_014_015_RetAgreeApplyGroup());
			regDealer(new GSSubDealer_014_016_RetCancelApplyToPlayer());
			regDealer(new GSSubDealer_014_017_RetCancelApplyToGroup());
			regDealer(new GSSubDealer_014_018_RetGetPoolAdult());
			regDealer(new GSSubDealer_014_020_RetSetRefuseMarry());
            regDealer(new GSSubDealer_014_050_OnChildAdd());
            regDealer(new GSSubDealer_014_051_OnChildNameChg());
			regDealer(new GSSubDealer_014_052_OnChildLvlChg());
			regDealer(new GSSubDealer_014_053_OnSeatChg());
			regDealer(new GSSubDealer_014_054_OnUnmarriedAdultAdd());
			regDealer(new GSSubDealer_014_055_OnMarriedAdultAdd());
			regDealer(new GSSubDealer_014_056_OnToMeApplyAdd());
			regDealer(new GSSubDealer_014_057_OnChildRemove());
			regDealer(new GSSubDealer_014_058_OnToMeApplyDel());
			regDealer(new GSSubDealer_014_059_OnUnmarriedAdultStatusChg());
			regDealer(new GSSubDealer_014_060_OnToMeApplyClear());
			regDealer(new GSSubDealer_014_061_OnChildBonusSumChg());
			regDealer(new GSSubDealer_014_062_OnChildBonusChg());
			regDealer(new GSSubDealer_014_063_OnAdultBonusSumChg());
        }
    }
}
