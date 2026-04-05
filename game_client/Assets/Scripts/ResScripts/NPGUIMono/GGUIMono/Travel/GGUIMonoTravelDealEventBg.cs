using ALPackage;

namespace GOE
{
    /// <summary>
    /// 处理事件时的背景
    /// </summary>
    public class GGUIMonoTravelDealEventBg : _AALBasicUIWndMono
    {
        [ALHeader("背景ShowCase")]
        public GGUIMonoCommonShowCase monoShowCaseBg;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3632); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3632); } }
    }
}