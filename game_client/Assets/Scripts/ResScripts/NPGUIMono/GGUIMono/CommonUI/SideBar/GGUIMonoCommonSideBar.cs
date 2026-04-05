using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用伸缩侧边栏
    /// </summary>
    public class GGUIMonoCommonSideBar : _AALBasicUIWndMono
    {
        [ALHeader("侧边栏布局类型")]
        public GridLayoutGroup.Axis axisType;
        [ALHeader("点击展开后列表默认靠边方向位置")]
        public EScrollRectMoveType defaultSpreadState;
        [ALHeader("侧边栏滚动")]
        public ScrollRect sidebarScrollRect;
        [ALHeader("侧边栏展开按钮")]
        public GameObject btnSpread;
        [ALHeader("侧边栏子元素")]
        public List<GameObject> sidebarElements;
        [ALHeader("侧边栏背景")]
        public RectTransform sidebarBg;
        [ALHeader("侧边栏展开显示的列表")]
        public List<GameObject> onSpreadShow;
        [ALHeader("侧边栏收起显示的列表")]
        public List<GameObject> onPackUpShow;
        [ALHeader("每个按钮高度")]
        public float btnHeight;
        [ALHeader("按钮间距")]
        public float btnSpace;
        [ALHeader("收缩状态时高度")]
        public float shrinkHeight;
        [ALHeader("背景高度限制, 超过最大高度可滚动")]
        public float bgLimit = 400f;
        [ALHeader("缩放时间")]
        public float chgSizeTime = 0.25f;
        [ALHeader("动画曲线")]
        public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
    }
}

