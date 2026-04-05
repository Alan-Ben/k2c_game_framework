using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoInnGuestList : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("切换页签")]
        public GGUIMonoInnGuestListPageTab pageTab;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6402); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6402); } }
    }
}