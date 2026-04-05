using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动中心界面
    /// </summary>
    public class GGUIMonoActivityCenter : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public GGUIMonoActivityCenterTabContainer monoTabContainer;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("列表为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
        [ALHeader("页签额外向内移动距离参数")]
        public MoveItemAdditionDistanceParam moveItemAdditionDistanceParam;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_ACTIVITY_CENTER_RES_ID); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_ACTIVITY_CENTER_RES_ID); } }
    }
}