using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 大臣详细信息prefab窗口
    /// </summary>
    public class GGUIPrefabMonoRecruitItemDetail_Hero : GGUIPrefabMonoRecruitItemDetail
    {
        [ALHeader("伙伴名字")]
        public TMP_Text txtName;
        [ALHeader("伙伴皮肤名字（称号）")]
        public TextEx txtSkinName;
        [ALHeader("初始资质")]
        public TextEx txtTalent;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("相性名称")]
        public TextEx txtSpecAttrName;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;
        
        [ALHeader("经营技能图标")]
        public RawImage imgBusinessIcon;
        [ALHeader("经营技能名称")]
        public Text txtBusinessName;
        [ALHeader("经营技能描述")]
        public Text txtBusinessDesc;

        [ALHeader("觉醒技能列表")]
        public GGUIMonoHeroStarSkillContainer monoStarSkillContainer;
        
        [ALHeader("更多信息按钮")]
        public GameObject btnMoreInfo;
        
        [ALHeader("关联妃子图标容器")]
        public GGUIMonoHeroConsortSimpleIconContainer relationConsortIconContainer;
    }
}