using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宝箱奖励领取状态
    /// </summary>
    public enum ESliderRewardState
    {
        [InspectorName("CAN_GET（可领取）")]
        CAN_GET,//可领取
        [InspectorName("CAN_NOT_GET（不可领取）")]
        CAN_NOT_GET,//不可领取
        [InspectorName("ALREADY_GET（已领取）")]
        ALREADY_GET,//已领取
    }

    /// <summary>
    /// 进度奖励item
    /// </summary>
    public class GGUIMonoCommonRewardSliderContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("已领取宝箱图标")]
        public NPGTextureIndex alreadyGetBoxIcon;
        [ALHeader("未领取宝箱图标")]
        public NPGTextureIndex notGetBoxIcon;

        [ALHeader("宝箱图标")]
        public RawImage imgBoxIcon;
        [ALHeader("需要展示的奖励")]
        public NPGGUIMonoCommonItem monoShowRewardItem;
        [ALHeader("分数")]
        public Text txtScore;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("状态动画列表")]
        public CommonAnimationShowTypeInfo<ESliderRewardState> stateAniList;
        [ALHeader("奖励可领取时需要播放的音效id")]
        public long canGetAudioId;
        [ALHeader("需要额外加载GO的父节点")]
        public Transform additionGoParent;
    }
}