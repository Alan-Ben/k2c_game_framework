using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestReceive : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("联姻请求列表")]
        public GGUIMonoAdultEngageRequestReceiveGrid monoRequestGrid;
        [ALHeader("拒绝联姻请求的勾选")]
        public NPGGUIMonoCommonToggleEx monoToggleRefuseAll;
        [ALHeader("一键拒绝按钮")]
        public GameObject btnOneKeyRefuse;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2504); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2504); } }
    }
}