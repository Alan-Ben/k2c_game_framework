using UnityEngine;
using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 通用3D场景跟随UI的容器窗口对象
    /// </summary>
    public class ALGGUIMonoCommonFollowRootWnd : _AALBasicUIWndMono
    {
        [ALHeader("所有子item的根节点对象")]
        public RectTransform itemRootParent;
    }
}
