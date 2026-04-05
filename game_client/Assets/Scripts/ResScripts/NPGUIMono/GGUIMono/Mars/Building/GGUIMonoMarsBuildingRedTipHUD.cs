using ALPackage;
using TMPro;

namespace GOE
{
    /// <summary>
    /// 建筑红点HUD
    /// </summary>
    public class GGUIMonoMarsBuildingRedTipHUD : ALGGUIMonoCommonFollowItem
    {
        [ALHeader("可建造红点")]
        public NPGGUIMonoCommonRedTip monoCanBuildRedTip;
        
        [ALHeader("可升级红点")]
        public NPGGUIMonoCommonRedTip monoCanUpgradeRedTip;
        
        [ALHeader("可派遣红点")]
        public NPGGUIMonoCommonRedTip monoCanDispatchRedTip;
        
        [ALHeader("建筑升级建造完成红点")]
        public NPGGUIMonoCommonRedTip monoBuildCompleteRedTip;
    }
}