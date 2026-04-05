using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item
    /// </summary>
    public class GGUIMonoHeroSuitSkillContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage texIcon;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
    }
}
