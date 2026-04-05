using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴光环已提升详情
    /// </summary>
    public class GGUIMonoHeroHaloOwnInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("实力加成绝对值")]
        public Text txtAddValue;
        [ALHeader("实力加成百分比")]
        public Text txtAddPer;
        [ALHeader("套系技能列表")]
        public GGUIMonoHeroHaloSuitSkillContainer monoSuitSkillContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1013); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1013); } }
    }
}