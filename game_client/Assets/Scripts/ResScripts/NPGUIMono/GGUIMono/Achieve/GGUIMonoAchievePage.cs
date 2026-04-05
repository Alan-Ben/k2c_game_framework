using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就信息页面
    /// </summary>
    public class GGUIMonoAchievePage : _AALBasicUIWndMono
    {
        [ALHeader("成就点进度")]
        public GGUIMonoAchievePointProgress monoAchievePointProgress;
        [ALHeader("成就列表")]
        public GGUIMonoAchieveGrid monoAchieveGrid;
        [ALHeader("成就页签列表")]
        public GGUIMonoAchieveTypeTabContainer monoTypeTabContainer;
    }
}

