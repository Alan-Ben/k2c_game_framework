using ALPackage;

namespace GOE
{
    /// <summary>
    /// 主线任务入口提示
    /// </summary>
    public class GGUIMonoQuestEntryTip : _AALBasicUIWndMono
    {
        [ALHeader("无操作隐藏时间秒")]
        public float delayHideTimeSec;
        [ALHeader("未操作隐藏动画")]
        public CommonAnimationSingleInfo noOpHideAni;
        [ALHeader("任务入口的CustomMono")]
        public NPGGUICustomMonoQuestFollowingQuest questCustomMono;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2404); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2404); } }
    }
}
