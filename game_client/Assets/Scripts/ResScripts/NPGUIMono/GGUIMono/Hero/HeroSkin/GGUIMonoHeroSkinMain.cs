using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{    
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EHeroSkinTabType
    {
        [InspectorName("CLOSET（衣柜）")]
        CLOSET,
    }

    /// <summary>
    /// 伙伴信息列表页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroSkinTabMono
    {
        [ALHeader("页签类型")]
        public EHeroSkinTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 伙伴皮肤主界面
    /// </summary>
    public class GGUIMonoHeroSkinMain : _AALBasicUIWndMono
    {
        [ALHeader("称号")]
        public Text txtTitle;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("页签列表")]
        public List<GGUIHeroSkinTabMono> monoTabList;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("皮肤列表")]
        public GGUIMonoHeroSkinContainer monoSkinContainer;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1024); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1024); } }
    }
}

