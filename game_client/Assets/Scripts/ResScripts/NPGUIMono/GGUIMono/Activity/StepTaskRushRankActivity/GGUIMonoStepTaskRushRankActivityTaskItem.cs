using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段任务冲榜活动任务item
    /// </summary>
    public class GGUIMonoStepTaskRushRankActivityTaskItem : _TALUGUIMonoGridItem
    {
        [ALHeader("任务名称")]
        public TextEx txtTaskName;

        [ALHeader("奖励点数")]
        public TextEx txtRewardPoints;//文本key: Reward:{0} Points

        [ALHeader("总点数")]
        public TextEx txtTotalPoints;//文本key: Obtained:{0} Points 有获取点数上限时中间参数展示为{当前分数}/{上限分数}, 没有点数限制时中间参数展示为{当前分数}
        
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
    }
}