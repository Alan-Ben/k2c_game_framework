using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 实验室奇物Mono
    /// </summary>
    public class GTDMonoTreasureHuntLabTreasure : MonoBehaviour
    {
        // 旧版本, GTDMonoTreasureHuntLabTreasure无需加载, 直接在GTDMonoTreasureHuntLab中配置
        // [ALHeader("奇物ID")]
        // public long treasureId;
        //
        // [ALHeader("不同奇物状态显示")]
        // public MultiStateShow<ETreasureHuntCanPutInLabThingsState> stateShow;
        //
        // [ALHeader("ui跟随窗口路径ID")]
        // public int uiFollowResPathId = -1;
        // [ALHeader("ui跟随节点")]
        // public Transform uiFollowParent;
        
        [ALHeader("是产出奇物时显示")]
        public List<GameObject> isOutputTreasureShow;
        [ALHeader("是产出奇物时隐藏")]
        public List<GameObject> isOutputTreasureHide;

        [ALHeader("在是产出奇物的前提下, 有产出可领取时显示")]
        public List<GameObject> hasOutputCanDrawShow;
        [ALHeader("在是产出奇物的前提下, 无产出可领取时显示")]
        public List<GameObject> noOutputCanDrawShow;

        [ALHeader("领取按钮")]
        public GTDCommonPosClickMono btnDraw;
    }
}