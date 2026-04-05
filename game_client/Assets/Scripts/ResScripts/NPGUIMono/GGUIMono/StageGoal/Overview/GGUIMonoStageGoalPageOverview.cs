using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoStageGoalPageOverview : _AALBasicUIWndMono
    {
        [ALHeader("所有任务总览的列表")]
        public GGUIMonoStageGoalOverviewGrid monoGrid;
        [ALHeader("不公开的任务数量")]
        public int privateCount = 10;
        [ALHeader("跟随列表的背景动画")]
        public CommonAnimationSingleInfo aniBg;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5001); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5001); } }
    }
}