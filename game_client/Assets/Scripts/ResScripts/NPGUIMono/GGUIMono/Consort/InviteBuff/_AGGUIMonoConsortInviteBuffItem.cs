using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class _AGGUIMonoConsortInviteBuffItem : _AALBasicUIWndMono
    {
        [ALHeader("buff次数")]
        public TextEx buffCount;
        [ALHeader("有buff次数时次数文本颜色")]
        public Color hasCountColor;
        [ALHeader("没有buff次数时次数文本颜色")]
        public Color noCountColor;

        [ALHeader("有buff次数时显示列表")]
        public List<GameObject> hasCountShowList;
        [ALHeader("没有buff次数时显示列表")]
        public List<GameObject> noCountShowList;
        
        [ALHeader("没有buff次数时置灰列表")]
        public List<MaskableGraphic> noCountGrayList;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("展示的弹窗ui路径id")]
        public long showToolTipUIResId;
        [ALHeader("展示的弹窗X轴间隔")]
        public float showToolTipIntervalX;
        [ALHeader("展示的弹窗Y轴间隔")]
        public float showToolTipIntervalY;
    }
}