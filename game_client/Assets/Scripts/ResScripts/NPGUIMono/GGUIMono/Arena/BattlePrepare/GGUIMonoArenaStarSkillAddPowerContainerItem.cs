using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场伙伴觉醒技能实力加成列表item
    /// </summary>
    public class GGUIMonoArenaStarSkillAddPowerContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("头像")]
        public RawImage imgIcon;
        [ALHeader("头像品质背景")]
        public Image imgIconBg;
        [ALHeader("觉醒技能名称")]
        public Text txtStarSkillName;
        [ALHeader("觉醒技能描述")]
        public Text txtStarSkillDesc;
    }
}
