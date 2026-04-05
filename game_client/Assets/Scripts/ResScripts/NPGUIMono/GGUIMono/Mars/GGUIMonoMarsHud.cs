using ALPackage;

namespace GOE
{
    public class GGUIMonoMarsHud : ALGGUIMonoCommonFollowRootWnd
    {
        [ALHeader("自动隐藏 hud 的时间，<=0 则不自动隐藏")]
        public float autoHideTime = 3f;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7100); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7100); } }
    }
}