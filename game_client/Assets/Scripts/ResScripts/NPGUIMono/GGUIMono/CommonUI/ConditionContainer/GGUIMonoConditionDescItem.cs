using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 条件描述item
    /// </summary>
    public class GGUIMonoConditionDescItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage icon;

        [ALHeader("描述")]
        public TextEx txtDesc;

        [ALHeader("跳转按钮")]
        public GameObject btnJumpTo;

        [ALHeader("条件完成描述颜色")]
        public Color enableDescColor;
        [ALHeader("未条件完成描述颜色")]
        public Color unableDescColor;

        [ALHeader("条件完成时显示go")]
        public List<GameObject> enableShow;
        [ALHeader("条件未完成时显示")]
        public List<GameObject> unableShow;

        [ALHeader("不满足条件时置灰列表")]
        public List<MaskableGraphic> unableGrayList;

        [ALHeader("当条件达成时播放的动画名")]
        public string onConditionEnableAnimationName;
    }
}