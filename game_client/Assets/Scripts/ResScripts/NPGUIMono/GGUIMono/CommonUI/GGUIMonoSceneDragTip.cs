using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoSceneDragTip : _AALBasicUIWndMono
    {
        [ALInfo("这个节点的 anchor 应该设置为全覆盖")]
        [ALHeader("按钮的父节点")]
        public RectTransform btnParent;
        [ALHeader("往四个方向移动的按钮")]
        public GameObject btnGoToLeft;
        public GameObject btnGoToRight;
        public GameObject btnGoToUp;
        public GameObject btnGoToDown;

        [ALHeader("按下按钮后移动到边缘的所需时间")]
        public float moveDuration = 0.5f;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_SCENE_DRAG_TIP); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_SCENE_DRAG_TIP);} }
    }
}