
using UnityEngine;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的上浮提示扩展mono
    /// </summary>
    public class NPGGUIMonoCommonTip : _AALBasicUIWndMono
    {
        [ALHeader("提示弹出的时候会使用这个Animation")]
        public Animation anim;

        [ALHeader("使用的动画的名字")]
        public string animName;

        [ALHeader("窗口位置调整,当作为子窗口时，一般是窗口自己的rect")]
        public RectTransform rectTransform;
    }
}