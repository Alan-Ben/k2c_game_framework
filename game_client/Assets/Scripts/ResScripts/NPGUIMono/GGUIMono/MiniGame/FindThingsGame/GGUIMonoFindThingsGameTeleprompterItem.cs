using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 找东西小游戏题词item
    /// </summary>
    public class GGUIMonoFindThingsGameTeleprompterItem : _AALBasicUIWndMono
    {
        [ALHeader("物品名称")]
        public TextEx txtThingsName;

        [ALHeader("有物品信息时显示")]
        public List<GameObject> hasThingInfoShow;
        [ALHeader("没有物品信息时显示")]
        public List<GameObject> noThingInfoShow;

        [ALHeader("飞行目标")]
        public Transform flyTarget;

        [ALHeader("动画")]
        public Animation teleprompterAnimation;

        [ALHeader("变化item时, 隐藏动画名")]
        public string changeItemHideAnimationName;
        [ALHeader("变化item时, 显示动画名")]
        public string changeItemShowAnimationName;
    }
}