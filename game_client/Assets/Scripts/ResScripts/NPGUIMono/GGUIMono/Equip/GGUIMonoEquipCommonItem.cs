using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品通用item
    /// </summary>
    public class GGUIMonoEquipCommonItem : _AALBasicUIWndMono
    {
        [ALHeader("藏品图标")]
        public RawImage imgIcon;
        [ALHeader("藏品品质底图(quality表icon字段)")]
        public Image imgBg;
        [ALHeader("藏品卡牌品质底图(quality_ext表equip_card_bg字段)")]
        public Image imgCardBg;
        [ALHeader("藏品额外等级图标")]
        public RawImage imgAddLevelIcon;
        [ALHeader("藏品品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("藏品名称")]
        public Text txtName;
        [ALHeader("藏品等级")]
        public Text txtLevel;
        [ALHeader("藏品资质")]
        public Text txtTalent;
        [ALInfo("====伙伴配置====")]
        [ALHeader("是否需要展示佩戴的伙伴")]
        public bool needShowHero = false;
        [ALHeader("佩戴的伙伴图标")]
        public RawImage imgHero;
        [ALHeader("佩戴的伙伴底图")]
        public Image imgHeroBg;
        [ALHeader("有伙伴佩戴时需要显示的GO列表")]
        public List<GameObject> goHaveHeroShowList;
        [ALHeader("有伙伴佩戴时需要隐藏的GO列表")]
        public List<GameObject> goHaveHeroHideList;
    }
}
