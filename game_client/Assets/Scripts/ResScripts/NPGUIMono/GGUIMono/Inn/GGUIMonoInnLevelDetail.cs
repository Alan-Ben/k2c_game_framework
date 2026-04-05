using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoInnLevelDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("等级详情列表")]
        public GGUIMonoInnLevelDetailGrid monoLevelGrid;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6417); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6417); } }
    }
}