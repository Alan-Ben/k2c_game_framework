using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能等级详情界面
    /// </summary>
    public class GGUIMonoHeroStarSkillLevelDetailToolTip : NPGGUIMonoCommonToolTip
    {
        [ALHeader("等级名称")]
        public Text txtLevelName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("等级描述列表")]
        public GGUIMonoHeroStarSkillLevelDetailContainer monoLevelDescContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1027); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1027); } }
    }
}