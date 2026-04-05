
using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum GGUIMonoAdultEngageRequestSendTabType
    {
        [InspectorName("Server === 本服")]
        Server,
        [InspectorName("Guild === 公会")]
        Guild,
        [InspectorName("Custom === 指定")]
        Custom
    }
    public class GGUIMonoAdultEngageRequestSend : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("联姻功能 tab ")]
        public GGUIMonoAdultEngageRequestSendPageTabList monoTabList;  
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2507); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2507); } }      
    }
}