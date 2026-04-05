using ALPackage;

namespace GOE
{
    public class GGUIMonoSevenDayGoalsDayContentPageShop : _AALBasicUIWndMono
    {
        [ALHeader("礼包列表")]
        public GGUIMonoActivityCrystalGiftPackPageContainer monoPackContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5802); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5802); } }
    }
}