using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageSelect : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子嗣列表")]
        public GGUIMonoAdultEngageSelectGrid monoAdultGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2510); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2510); } }      
    }
}