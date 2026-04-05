using ALPackage;

namespace GOE
{
    /// <summary>
    /// 实验室奇物跟随item
    /// </summary>
    public class GGUIMonoTreasureHuntLabTreasureFollowItem : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("奇物状态显示物体")]
        public MultiStateShow<ETreasureHuntTreasureState> stateShow;
    }
}