using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跑马灯附加窗口
    /// </summary>
    public class GGUIMonoSubMarquee : _AALBasicUIWndMono
    {
        [ALHeader("连续点击需要特殊处理按钮")]
        public GGUIMonoContinuousClickBtn monoContinuousClickBtn;
        [ALHeader("跑马灯item父节点")]
        public Transform itemParent;
        [ALHeader("文字的可见区域")]
        public RectTransform transViewportArea;
        [ALHeader("每条跑马灯展示间隔时间(秒)")]
        public float intervalTimeSec;
        [ALHeader("跑马灯每秒的移动速度")]
        public float fMoveSpeed;
        [ALHeader("有跑马灯时需要展示的GO列表")]
        public List<GameObject> goHaveMarqueeShowList;
        [ALHeader("进入不可展示界面时需要隐藏的GO")]
        public List<GameObject> goCanNotShowHideList;
    }
}

