using ALBasicProtocolPack;

namespace GOE
{
    public class GSMainDealer_015_ConsortOp : ALBasicProtocolMainOrderDealer
    {
        public GSMainDealer_015_ConsortOp() :
            base(15, 80)
        {
            
            regDealer(new GSSubDealer_015_050_OnConsortAdd());
            regDealer(new GSSubDealer_015_051_OnConsortIntimacyChg());
            regDealer(new GSSubDealer_015_052_OnConsortCharmChg());
            regDealer(new GSSubDealer_015_053_OnConsortCharmPointChg());
            regDealer(new GSSubDealer_015_054_OnTriggeredCallDlgIdAdd());
            regDealer(new GSSubDealer_015_055_OnSkinAdd());
            regDealer(new GSSubDealer_015_056_OnCurSkinChg());
            regDealer(new GSSubDealer_015_057_OnFettersChg());
            regDealer(new GSSubDealer_015_058_OnBusinessSkillChg());
            regDealer(new GSSubDealer_015_059_OnBlessSkillChg());
            regDealer(new GSSubDealer_015_060_OnHaloChg());
            regDealer(new GSSubDealer_015_061_OnCgChg());
			regDealer(new GSSubDealer_015_062_OnRandCallConsortChg());
			regDealer(new GSSubDealer_015_070_OnConsortChatDialogueAdd());
			regDealer(new GSSubDealer_015_071_OnConsortChatDialogueRewardDraw());
			regDealer(new GSSubDealer_015_072_OnConsortChatDialogueOptionChg());
			regDealer(new GSSubDealer_015_073_OnConsortAiChatMsgAdd());
			regDealer(new GSSubDealer_015_074_OnConsortChatHasAddChg());
			regDealer(new GSSubDealer_015_075_OnConsortChatInfoAdd());
			regDealer(new GSSubDealer_015_076_OnConsortAiChatMomentMsgAdd());
        }
    }
}
