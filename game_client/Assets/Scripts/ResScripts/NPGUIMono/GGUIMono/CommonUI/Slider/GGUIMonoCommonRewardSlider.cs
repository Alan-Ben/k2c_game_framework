using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带奖励列表的进度条
    /// </summary>
    public class GGUIMonoCommonRewardSlider : _AALBasicUIWndMono
    {
        [ALHeader("当前进度文本")]
        public TextEx txtCurProcess;
        [ALHeader("进度条")] 
        public Slider sldProcess;
        [ALHeader("奖励宝箱列表")]
        public GGUIMonoCommonRewardSliderContainer monoRewardSliderContainer;
        [ALHeader("每个奖励位置是否平分进度条")]
        public bool isDivideEqually;
        [ALHeader("最后一个宝箱在进度条的位置（百分比0~100）"), Range(0, 100)]
        public long lastRewardBoxPosPercentage = 100;

        [ALHeader("最后一个需要特殊展示的item（该项配置了则宝箱列表最后一个不展示）")]
        public GGUIMonoCommonRewardSliderContainerItem monoLastSpecItem;

        [ALHeader("进度条满时特效id")]
        public long fullSfxId;
        [ALHeader("进度条满时特效父节点")]
        public Transform fullSfxParent;
    }
}