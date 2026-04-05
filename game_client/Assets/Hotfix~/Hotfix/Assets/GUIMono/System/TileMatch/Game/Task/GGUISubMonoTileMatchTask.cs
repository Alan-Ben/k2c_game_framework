using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消任务子窗口
    /// </summary>
    public class GGUISubMonoTileMatchTask : _AHotfixBaseMono
    {
        [HotfixMono("有任务时显示")]
        public List<GameObject> hasTaskShow;
        [HotfixMono("没有任务时显示")]
        public List<GameObject> noTaskShow;
        
        [HotfixMono("剩余步数进度条")]
        public NPGGUIMonoProgress monoLeftStepSlider;

        [HotfixMono("任务信息容器")]
        public GGUIHotfixCommonMono monoTaskBlockContainer;

        [HotfixMono("增加的分数")]
        public TextEx txtAddScore;
        
        [HotfixMono("飞行任务item缓存父节点")]
        public Transform taskFlyItemCacheRoot;
        
        [HotfixMono("飞行任务item预制")]
        public GGUIHotfixCommonMono monoTaskFlyItemPrefab;

        [HotfixMono("任务动画")]
        public Animation ani;
        
        [HotfixMono("任务完成动画名称")]
        public string taskCompleteAnimName;
        [HotfixMono("任务失败动画名称")]
        public string taskFailAniAnimName;

        [HotfixMono("阶段奖励子窗口")]
        public GGUIHotfixCommonMono monoStepReward;
        [HotfixMono("客人表现子窗口")]
        public GGUIHotfixCommonMono monoGuestShow;
    }
}