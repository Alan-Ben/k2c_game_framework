using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum EDungeonRankTab
    {
        NONE,
        [InspectorName("午间副本排行榜Tab")]
        MIDDAY,
        [InspectorName("晚间副本排行榜Tab")]
        EVENING,
    }

    [System.Serializable]
    public class DungeonRankTabMono
    {
        [ALHeader("页签类型")]
        public EDungeonRankTab tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public NPCommonAssetPathInfo tabAssetPathInfo;
    }
    
    public class GGUIMonoDungeonRank : _ANPBasicUIWndResBarMono
    {
        [ALHeader("页签列表")]
        public List<DungeonRankTabMono> tabList;
        [ALHeader("默认选中的页签")]
        public EDungeonRankTab defaultSelectTab;

        public EEveningDungeonRankAndRewardDetailTabType defaultSelectEveningDungeonTab;

        [ALHeader("页签page挂载父节点")]
        public Transform tabPageParent;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5510); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5510); } }
    }
}