using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EHeroSuitAndPowerTabType
    {
        [InspectorName("SUIT（套系详情）")]
        SUIT,
        [InspectorName("POWER（战力详情）")]
        POWER,
    }

    /// <summary>
    /// 伙伴套系及战力详情界面页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroSuitAndPowerTabMono
    {
        [ALHeader("页签类型")]
        public EHeroSuitAndPowerTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 伙伴套系及战力详情界面
    /// </summary>
    public class GGUIMonoHeroSuitAndPower : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("伙伴名称")]
        public Text txtName;
        [ALHeader("页签列表")]
        public List<GGUIHeroSuitAndPowerTabMono> monoTabList;
        [ALHeader("页面父节点")]
        public Transform pageParent;
        [ALHeader("没有套系时需要显示的GO列表")]
        public List<GameObject> goNoSuitShowList;
        [ALHeader("没有套系时需要隐藏的GO列表")]
        public List<GameObject> goNoSuitHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1023); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1023); } }
    }
}