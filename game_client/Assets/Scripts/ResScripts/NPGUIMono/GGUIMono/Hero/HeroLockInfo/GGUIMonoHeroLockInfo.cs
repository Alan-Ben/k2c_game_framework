using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EHeroLockInfoTabType
    {
        [InspectorName("INTRODUCTION（简介）")]
        INTRODUCTION,
        [InspectorName("SKILL（技能）")]
        SKILL,
    }

    /// <summary>
    /// 伙伴信息列表页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroLockInfoTabMono
    {
        [ALHeader("页签类型")]
        public EHeroLockInfoTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 伙伴未解锁详情信息主窗口
    /// </summary>
    public class GGUIMonoHeroLockInfo : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("伙伴名字")]
        public Text txtName;
        [ALHeader("伙伴皮肤名字（称号）")]
        public Text txtSkinName;
        [ALHeader("初始资质")]
        public Text txtTalent;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("相性名称")]
        public Text txtSpecAttrName;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;
        [ALHeader("伙伴配音气泡附加窗口")]
        public GGUIMonoHeroVoiceBubble monoVoiceBubble;
        [ALHeader("获取途径")]
        public GameObject btnAccess;
        [ALHeader("左边按钮")]
        public GameObject btnLeft;
        [ALHeader("右边按钮")]
        public GameObject btnRight;
        [ALHeader("页签列表")]
        public List<GGUIHeroLockInfoTabMono> monoTabList;
        [ALHeader("默认显示的页签类型")]
        public EHeroLockInfoTabType tabDefaultShow;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("获取途径tip的x偏移量")]
        public float accessTipIntervalX;
        [ALHeader("获取途径tip的y偏移量")]
        public float accessTipIntervalY;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1020); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1020); } }
    }
}