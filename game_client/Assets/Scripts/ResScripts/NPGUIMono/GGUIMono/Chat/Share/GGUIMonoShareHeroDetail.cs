using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴分享详情弹窗
    /// </summary>
    public class GGUIMonoShareHeroDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("简介按钮")]
        public GameObject btnInfo;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("相性名称")]
        public Text txtSpecAttrName;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;
        [ALHeader("伙伴名字")]
        public Text txtName;
        [ALHeader("伙伴等级")]
        public Text txtLevel;
        [ALHeader("实力")]
        public Text txtPower;
        [ALHeader("资质")]
        public Text txtTalent;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("觉醒技能列表")]
        public GGUIMonoHeroStarSkillContainer monoStarSkillContainer;
        [ALHeader("没有觉醒时需要隐藏的GO列表")]
        public List<GameObject> goNoStarHideList;
        [ALHeader("资质技能列表")]
        public GGUIMonoShareHeroTalentSkillContainer monoTalentSkillContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1320); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1320);} }
    }
}