using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗伙伴展示信息附加窗口
    /// </summary>
    public class GGUIMonoArenaBattleSubHeroInfoItem : _AALBasicUIWndMono
    {
        [ALHeader("谈判按钮")]
        public GameObject btnFight;
        [ALHeader("伙伴头像")]
        public RawImage imgHero;
        [ALHeader("伙伴头像品质背景")]
        public Image imgBg;
        [ALHeader("伙伴半身像")]
        public RawImage imgHeroCard;
        [ALHeader("伙伴名字")]
        public Text txtHeroName;
        [ALHeader("伙伴等级")]
        public Text txtLevel;
    }
}