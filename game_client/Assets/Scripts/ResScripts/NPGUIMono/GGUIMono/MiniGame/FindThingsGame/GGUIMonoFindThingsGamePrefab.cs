using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 不同找东西小游戏加载prefab
    /// </summary>
    public class GGUIMonoFindThingsGamePrefab : _AALBasicUIWndMono
    {
        [ALHeader("物品列表")]
        public List<GGUIMonoFindThingsGameThing> monoThingsList;
        
        [ALHeader("题词item父节点列表")]
        public List<Transform> monoTeleprompterParentList;
        [ALHeader("题词item预制路径")]
        public NPCommonAssetPathInfo teleprompterItemAssetPath;

        [ALHeader("进度条")]
        public NPGGUIMonoProgress monoProgress;
        [ALHeader("进度条变化时间")]
        public float progressChgTime;

        [ALHeader("点击错误按钮")]
        public GameObject clickErrorBtn;
        
        [ALHeader("点击错误时tips")]
        public NPGGUIMonoCommonTip clickErrorTipMono;
        [ALHeader("tip的父节点")]
        public Transform tipParent;
        [ALHeader("提示显示时间(超过这个时间, tip销毁)")]
        public float tipShowTime;

        [ALHeader("动画")]
        public Animation gameAnimation;
        [ALHeader("游戏成功动画名")]
        public string gameSuccessAnimationName;
    }
}