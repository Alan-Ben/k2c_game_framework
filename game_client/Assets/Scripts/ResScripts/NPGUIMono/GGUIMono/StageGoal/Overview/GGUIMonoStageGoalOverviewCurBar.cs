using ALPackage;

namespace GOE
{
    /// <summary>
    /// 阶段目标预览当前阶段bar
    /// </summary>
    public class GGUIMonoStageGoalOverviewCurBar : _AALBasicUIWndMono
    {
        [ALHeader("到达新阶段动画")]
        public CommonAnimationSingleInfo newStageAni;
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5005); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5005); } }
    }
}
