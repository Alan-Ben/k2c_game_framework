using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntGamePlayQuitCheck : _AALBasicUIWndMono
    {
        [ALHeader("今天不再提示的勾选")]
        public NPGGUIMonoCommonToggleEx monoDontShowToday;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6828); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6828); } }
    }
}