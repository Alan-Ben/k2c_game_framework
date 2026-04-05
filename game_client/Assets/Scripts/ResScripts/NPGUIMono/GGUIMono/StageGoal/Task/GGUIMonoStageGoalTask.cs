using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIMonoStageGoalTask : _AALBasicUIWndMono
    {
        [ALHeader("任务列表")]
        public List<GGUIMonoStageGoalTaskItem> taskItemList;
        [ALHeader("任务都完成领取奖励时需要播放的动画")]
        public CommonAnimationSingleInfo allFinishAni;
    }
}