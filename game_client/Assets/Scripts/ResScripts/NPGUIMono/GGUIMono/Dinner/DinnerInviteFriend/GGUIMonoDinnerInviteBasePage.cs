using ALPackage;

namespace GOE
{
    public class GGUIMonoDinnerInviteBasePage : _AALBasicUIWndMono
    {
        [ALHeader("好友列表列表")]
        public GGUIMonoDinnerInviteFriendItemGrid friendGrid;
        //资源加载路径
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2907); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2907); } }
    }
}