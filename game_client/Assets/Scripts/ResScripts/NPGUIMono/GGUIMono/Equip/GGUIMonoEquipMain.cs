using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //页签类型
    public enum EEquipMainTabType
    {
        [InspectorName("OWN（拥有）")]
        OWN,
        [InspectorName("ILLUSTRATED_HANDBOOK（图鉴）")]
        ILLUSTRATED_HANDBOOK,
    }

    /// <summary>
    /// 藏品列表页签
    /// </summary>
    [System.Serializable]
    public class GGUIEquipMainTabMono
    {
        [ALHeader("页签类型")]
        public EEquipMainTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
    }

    /// <summary>
    /// 藏品主界面
    /// </summary>
    public class GGUIMonoEquipMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("回收按钮")]
        public GameObject btnRecycle;
        [ALHeader("页签列表")]
        public List<GGUIEquipMainTabMono> monoTabList;
        [ALHeader("已拥有藏品列表")]
        public GGUIMonoEquipMainListGrid monoListGrid;
        [ALHeader("图鉴藏品列表")]
        public GGUIMonoEquipMainListGrid monoHandbookListGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1800); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1800); } }
    }
}
