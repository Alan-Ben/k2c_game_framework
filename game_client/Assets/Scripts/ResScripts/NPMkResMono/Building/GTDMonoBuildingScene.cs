
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public struct BuildingSceneConfig
    {
        [ALHeader("建筑资源变化时的特效 id ")]
        public long buildingResChgSfxId;
        [ALHeader("建筑资源替换在特效开始后多久执行")]
        public float buildingResChgStartDelay;
        [ALHeader("建筑资源变化加载完成时，延迟多久删除特效")]
        public float buildingResChgSfxDeleteDelay;
        [ALHeader("资源建筑隔多久跳一次收益")]
        public float businessBuildingEarningTipSpace;
        [ALHeader("农田间隔多久跳一次自动点击的收益")]
        public float farmingBuildingEarningTipSpace;
    }
    public class GTDMonoBuildingScene : MonoBehaviour
    {
        [ALHeader("场景的一些配置")]
        public BuildingSceneConfig config;
        [ALHeader("特殊的建筑聚焦位置列表")
        ,ALInfo("需要特殊的建筑聚焦位置时使用，这个列表里找不到会用下面的建筑加载位置列表")]
        public List<GTDMonoBuildingPos> specialBuildingFocusPosList;
        [ALHeader("建筑位置列表")]
        public List<GTDMonoBuildingPos> buildingPosList;
        [ALHeader("建筑的父节点")]
        public Transform buildingParent;
        [ALHeader("经营事件位置列表")]
        public List<GTDMonoAnecdotePos> anecdotePosList;
        [ALHeader("经营事件的父节点")]
        public Transform anecdoteParent;
        [ALHeader("特卖礼包位置")]
        public Transform rankGiftPackPos;
        [ALHeader("限时兑换")]
        public GTDMonoRushExchange rushExchangeMono;
    }
}