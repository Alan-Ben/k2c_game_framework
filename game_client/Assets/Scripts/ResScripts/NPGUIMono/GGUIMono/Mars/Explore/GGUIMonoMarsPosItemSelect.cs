using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsPosItemSelect : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("列表容器")]
        public GGUIMonoMarsPosItemSelectContainer monoItemContainer;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7418); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7418); } }
    }
}