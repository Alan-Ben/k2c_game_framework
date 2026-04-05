using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴套系详情页签
    /// </summary>
    public class GGUIMonoHeroSuitDetailPage : _AALBasicUIWndMono
    {
        [ALHeader("套系描述")]
        public Text txtSuitDesc;
        [ALHeader("套系标题")]
        public Text txtSuitTitle;
        [ALHeader("套系伙伴列表")]
        public GGUIMonoHeroConsortSimpleIconContainer monoHeroContainer;
        [ALHeader("套系技能列表")]
        public GGUIMonoHeroSuitSkillContainer monoSuitSkillContainer;
    }
}

