using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴未解锁技能展示页签
    /// </summary>
    public class GGUIMonoHeroLockInfoSkillPage : _AALBasicUIWndMono
    {
        [ALHeader("套系名")]
        public Text txtSuitName;
        [ALHeader("套系列表")]
        public GGUIMonoHeroConsortSimpleIconContainer monoSuitContainer;
        [ALHeader("没有套系时需要隐藏的GO列表")]
        public List<GameObject> goNoSuitHideList;
        [ALHeader("加护列表")]
        public GGUIMonoHeroConsortSimpleIconContainer monoConsortContainer;
        [ALHeader("没有加护时需要隐藏的GO列表")]
        public List<GameObject> goNoConsortHideList;
        [ALHeader("经营技能图标")]
        public RawImage imgBusinessIcon;
        [ALHeader("经营技能名称")]
        public Text txtBusinessName;
        [ALHeader("经营技能描述")]
        public Text txtBusinessDesc;
        [ALHeader("额外经营技能列表")]
        public List<GGUIMonoHeroInfoBusinessItem> monoAdditionItemList;
        [ALHeader("觉醒技能列表")]
        public GGUIMonoHeroStarSkillContainer monoStarSkillContainer;
        [ALHeader("没有觉醒时需要隐藏的GO列表")]
        public List<GameObject> goNoStarHideList;
        [ALHeader("资质技能列表")]
        public GGUIMonoHeroTalentSkillContainer monoTalentSkillContainer;
    }
}

