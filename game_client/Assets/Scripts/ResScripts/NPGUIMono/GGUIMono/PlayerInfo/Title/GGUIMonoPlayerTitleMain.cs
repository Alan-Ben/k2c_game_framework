using System.Collections.Generic;
using ALPackage;
using UnityEngine;
namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EPlayerTitleTabType
    {
        [InspectorName("COMBO（组合）")]
        COMBO,
        [InspectorName("FIXED（固定）")]
        FIXED,
        [InspectorName("LIMITED（限时）")]
        LIMITED,
    }

    /// <summary>
    /// 玩家称号界面页签
    /// </summary>
    [System.Serializable]
    public class GGUIPlayerTitleMainTabMono
    {
        [ALHeader("页签类型")]
        public EPlayerTitleTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 玩家称号详情界面
    /// </summary>
    public class GGUIMonoPlayerTitleMain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIPlayerTitleMainTabMono> monoTabList;
        [ALHeader("页面父节点")]
        public Transform pageParent;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1717); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1717); } }
    }
}