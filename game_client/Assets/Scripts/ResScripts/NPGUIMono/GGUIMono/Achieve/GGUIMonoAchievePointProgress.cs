using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 动画枚举
    /// </summary>
    public enum EAchieveParticleAniType
    {
        [InspectorName("START_PARTICLE（粒子开始后需要播放的动画）")]
        START_PARTICLE,
        [InspectorName("SINGLE_PARTICLE（单个粒子需要播放的额外动画）")]
        SINGLE_PARTICLE,
    }

    /// <summary>
    /// 成就点进度状态
    /// </summary>
    public enum EAchievePointProgressState
    {
        CAN_GET,//可领取
        CAN_NOT_GET,//无法领取
        ALL_DONE,//全部领取完成
    }

    /// <summary>
    /// 成就点进度状态
    /// </summary>
    [System.Serializable]
    public class GGUIAchievePointProgressState
    {
        [ALHeader("状态")]
        public EAchievePointProgressState stete;
        [ALHeader("该状态需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("该状态需要隐藏的GO列表")]
        public List<GameObject> goHideList;
    }

    /// <summary>
    /// 成就点进度子窗口
    /// </summary>
    public class GGUIMonoAchievePointProgress : _AALBasicUIWndMono
    {
        [ALHeader("已领取宝箱图标")]
        public NPGTextureIndex alreadyGetBoxIcon;
        [ALHeader("未领取宝箱图标")]
        public NPGTextureIndex notGetBoxIcon;
        [ALHeader("成就点图标")]
        public RawImage imgIcon;
        [ALHeader("成就点阶段")]
        public Text txtStep;
        [ALHeader("预览奖励按钮")]
        public GameObject btnOpenPreview;
        [ALHeader("成就阶段奖励领取按钮")]
        public GameObject btnGetStepReward;
        [ALHeader("成就阶段进度条")]
        public Slider sldProgress;
        [ALHeader("成就进度")]
        public Text txtProgress;
        [ALHeader("成就阶段奖励item列表")]
        public NPGGUIMonoCommonItemContainer stepRewardItemContainer;
        [ALHeader("状态列表")]
        public List<GGUIAchievePointProgressState> showStateList;
        [ALHeader("成就奖励预览pathid")]
        public long rewardPreviewResPathId;
        [ALHeader("当前成就点文本大小")]
        public long curScoreTextSize = 38;
        [ALHeader("粒子动画终点")]
        public RectTransform particleEndRect;
        [ALHeader("粒子需要播放的额外动画")]
        public CommonAnimationShowTypeInfo<EAchieveParticleAniType> particleAni;

        [ALHeader("粒子到达时播放特效父节点")]
        public Transform particleSfxParent;
        [ALHeader("粒子到达时播放特效id")]
        public long particleSfxId;
        [ALHeader("特效可同时存在的最大数量")]
        public long particleSfxMaxNum = 1;

        [ALHeader("宝箱奖励可领取时需要播放的音效id")]
        public long canGetAudioId;
    }
}

