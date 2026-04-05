
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子嗣的数据")]
        public GGUIMonoChildInfo monoAdultInfo;      
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2503); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2503); } }
    }
}