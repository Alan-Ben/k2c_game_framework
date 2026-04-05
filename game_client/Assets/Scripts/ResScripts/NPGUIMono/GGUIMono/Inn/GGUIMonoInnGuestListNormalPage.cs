using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGuestListNormalPage : _AALBasicUIWndMono
    {
        [ALHeader("当前收集进度")]
        public Text txtCollectTip;
        [ALHeader("客人列表")]
        public GGUIMonoInnGuestListNormalPageGrid monoItemGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6403); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6403); } }
    }
}