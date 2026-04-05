using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟委托奖励预览
    /// </summary>
    public class GGUIMonoGuildEntrustRewardPreview : _AALBasicUIWndMono
    {
        [ALHeader("联盟总收益")]
        public TextEx txtGuildTotalEarnings;

        [ALHeader("每次委托获取金币")]
        public TextEx txtPerDealGainSilver;
        
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4919); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4919); } }
    }
}