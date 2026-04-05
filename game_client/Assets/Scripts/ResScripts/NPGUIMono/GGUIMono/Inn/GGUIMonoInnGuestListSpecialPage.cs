using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGuestListSpecialPage : _AALBasicUIWndMono
    {
        [ALHeader("当前收集进度")]
        public Text txtCollectTip;
        [ALHeader("客人列表")]
        public GGUIMonoInnGuestListSpecialPageGrid monoItemGrid; 
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6404); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6404); } }
    }
}