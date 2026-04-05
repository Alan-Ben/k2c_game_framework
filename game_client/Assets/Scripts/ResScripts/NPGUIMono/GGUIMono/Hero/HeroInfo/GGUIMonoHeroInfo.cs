using System.Collections.Generic;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 仅展示形象动画类型
    /// </summary>
    public enum EHeroPreviewAniType
    {
        [InspectorName("START_PREVIEW（仅展示形象动画）")]
        START_PREVIEW,
        [InspectorName("CLOSE_PREVIEW（关闭仅展示形象动画）")]
        CLOSE_PREVIEW,
    }

    //页签类型
    public enum EHeroInfoTabType
    {
        [InspectorName("INFO（信息）")]
        INFO,
        [InspectorName("BUSINESS（经营）")]
        BUSINESS,
        [InspectorName("TALENT（资质）")]
        TALENT,
        [InspectorName("STAR（觉醒）")]
        STAR,
        [InspectorName("HALO（星辉）")]
        HALO,
        [InspectorName("BLESS（加护）")]
        BLESS,
    }

    /// <summary>
    /// 伙伴信息列表页签
    /// </summary>
    [System.Serializable]
    public class GGUIHeroInfoTabMono
    {
        [ALHeader("页签类型")]
        public EHeroInfoTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
        [ALHeader("指引GO")]
        public GameObject goGuide;
    }

    /// <summary>
    /// 伙伴详情信息主窗口
    /// </summary>
    public class GGUIMonoHeroInfo : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIHeroInfoTabMono> monoTabList;
        [ALHeader("默认显示的页签类型")]
        public EHeroInfoTabType tabDefaultShow;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("伙伴名字")]
        public Text txtName;
        [ALHeader("伙伴皮肤名字（称号）")]
        public Text txtSkinName;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("相性图标")]
        public RawImage imgSpecAttrIcon;
        [ALHeader("相性名称")]
        public Text txtSpecAttrName;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("套系数量")]
        public Text txtSuitCount;
        [ALHeader("套系按钮")]
        public GameObject btnSuitDetail;
        [ALHeader("无套系时需要隐藏的GO列表")]
        public List<GameObject> goNoSuitHideList;
        [ALHeader("伙伴简介按钮")]
        public GameObject btnIntroduction;
        [ALHeader("仅展示形象按钮")]
        public GameObject btnPreview;
        [ALHeader("关闭仅展示形象按钮")]
        public GameObject btnPreviewClose;
        [ALHeader("仅展示形象动画配置")]
        public CommonAnimationShowTypeInfo<EHeroPreviewAniType> previewAni;
        [ALHeader("左边按钮")]
        public GameObject btnLeft;
        [ALHeader("右边按钮")]
        public GameObject btnRight;
        [ALHeader("伙伴形象子窗口")]
        public GGUIMonoCommonShowCase monoShowCaseWnd;
        [ALHeader("进入播放的动画")]
        public string enterPlayAniName;
        [ALHeader("点击伙伴形象随机的动画名字列表")]
        public List<string> clickRandomAniNameList;
        [ALHeader("实力变化上浮提示id")]
        public long powerCenterTipId;
        [ALHeader("伙伴配音气泡附加窗口")]
        public GGUIMonoHeroVoiceBubble monoVoiceBubble;
        [ALHeader("单次升级成功特效id")]
        public long singleUpgradeSfxId;
        [ALHeader("十连升级成功特效id")]
        public long tenUpgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;

        [ALHeader("伙伴形象进入动画名")]
        public string heroEnterAniName = "enter";
        [ALHeader("伙伴形象停留动画名")]
        public string heroIdleAniName = "idle";

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1002); } }
    }
}