using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 任务完成相关展示状态枚举
    /// </summary>
    public enum EStageGoalTaskItemCompleteShowState
    {
        [InspectorName("UNDONE（未完成状态）")]
        UNDONE,
        [InspectorName("SHOW_DONE（展示完成动画）")]
        SHOW_DONE,
        [InspectorName("DONE（已完成状态）")]
        DONE,
    }

    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum EStageGoalTaskItemState
    {
        [InspectorName("UNLOCK（未解锁）")]
        UNLOCK,
        [InspectorName("CAN_GET（可以领取）")]
        CAN_GET,
        [InspectorName("CAN_NOT_GET（还不能领取,指没达成或者没有奖励）")]
        CAN_NOT_GET,
        [InspectorName("HAS_GET（已经领取完奖励）")]
        HAS_GET,
    }

    public class GGUIMonoStageGoalTaskItem : _AALBasicUIWndMono
    {
        [ALHeader("目标图片")]
        public RawImage goalImg;
        public RawImage goalImg2;
        public RawImage goalImg3;
        public RawImage goalImg4;
        [ALHeader("目标标题")]
        public Text goalTitleTxt;
        [ALHeader("目标进度")]
        public NPGGUIMonoProgress goalProgressMono;
        [ALHeader("不同状态显示的GoList")]
        public List<NPCommonEnumStatInfo<EStageGoalTaskItemState>> getRewardStateList;
        [ALHeader("前往按钮")]
        public GameObject goToBtn;
        [ALHeader("获取、预览奖励按钮")]
        public GameObject getRewardBtn;
        [ALHeader("引导手指相关")]
        public Transform guideTarget;
        public int handGuideResId = 1000900;
        [ALHeader("任务连线相关状态动画")]
        public CommonAnimationShowTypeInfo<EStageGoalTaskItemCompleteShowState> stateAniInfo;
        [ALHeader("未领取奖励时标题文本颜色")]
        public Color notGetRewardTitleColor = Color.white;
        [ALHeader("已领取奖励时标题文本颜色")]
        public Color alreadyGetRewardTitleColor = Color.gray;
        [ALHeader("完成时当前任务计数文本颜色")]
        public Color completeCurProcessColor = Color.green;
        [ALHeader("未完成时当前任务计数文本颜色")]
        public Color notCompleteCurProcessColor = Color.red;
    }
}