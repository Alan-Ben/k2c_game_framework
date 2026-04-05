using UnityEngine;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 选择滚动条Mono基类
    /// </summary>
    public abstract class _ATNPGGUIMonoSelectContainer<_T_ITEM_MONO>
        : _TALUGUIMonoContainerWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _ANPGGUIMonoSelectContainerItem
    {
        [ALHeader("跳转到前/后一个item的按钮，在首尾item会隐藏对应按钮，不需要就放空")]
        [Tooltip("跳转到前一个item的按钮")]
        public GameObject btnPre;
        [Tooltip("跳转到后一个item的按钮")]
        public GameObject btnNext;

        [ALInfo("移动根据Content坐标计算，并且自动生成占位Go。Content的LayoutGroup需要使用HorizontalLayoutGroup或者VerticalLayoutGroup，Pivot为（0,1）。")]
        [ALHeader("滚动条相关参数", "#FFFF00")]
        [Tooltip("item大小")]
        public float itemSize;
        [Tooltip("item间距")]
        public float itemSpacing;
        [Tooltip("item居中总时长（秒）")]
        public float centeringTotalTime = 0.2f;
        [Tooltip("item居中加速段占比")]
        public float centeringAccTimeScale = 0f;
        [Tooltip("item与中心距离低于这个值视为已经居中")]
        public float minCenteringOffset = 0.1f;
        [Tooltip("滚动条速度低于这个值，视为停止运动")]
        public float minScrollMoveSpeed = 200f;
        [ALInfo("缩放比例曲线，以ViewPort为偏移参照物，0.5是中央，超出1和0的部分无效。")]
        public AnimationCurve scaleCurve;
        //TODO:item切换范围

    }
}
