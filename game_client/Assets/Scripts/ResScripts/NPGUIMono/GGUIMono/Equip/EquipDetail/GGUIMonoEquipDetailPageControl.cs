using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //页签类型
    public enum EEquipDetailTabType
    {
        [InspectorName("UPGRADE（升级）")]
        UPGRADE,
        [InspectorName("SKILL（技能）")]
        SKILL,
    }

    /// <summary>
    /// 藏品列表页签
    /// </summary>
    [System.Serializable]
    public class GGUIEquipDetailPageTabMono
    {
        [ALHeader("页签类型")]
        public EEquipDetailTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 藏品详情页签管理界面
    /// </summary>
    public class GGUIMonoEquipDetailPageControl : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public List<GGUIEquipDetailPageTabMono> monoTabList;
        [ALHeader("藏品技能列表")]
        public GGUIMonoEquipSkillContainer monoSkillContainer;
        [ALHeader("伙伴点击按钮")]
        public GameObject btnHero;
        [ALHeader("伙伴头像")]
        public RawImage imgHeroIcon;
        [ALHeader("伙伴品质背景")]
        public Image imgHeroBg;
        [ALHeader("伙伴名称")]
        public Text txtHeroName;
        [ALHeader("属性加成值")]
        public Text txtAddValue;
        [ALHeader("有伙伴佩戴时需要显示的GO列表")]
        public List<GameObject> goHaveHeroShowList;
        [ALHeader("有伙伴佩戴时需要隐藏的GO列表")]
        public List<GameObject> goHaveHeroHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1802); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1802); } }
    }
}
