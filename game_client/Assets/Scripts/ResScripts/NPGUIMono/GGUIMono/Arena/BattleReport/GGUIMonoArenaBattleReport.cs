using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EArenaBattleReportTabType
    {
        [InspectorName("OPPONENT（对手）")]
        OPPONENT,
        [InspectorName("WAR_REPORT（战报）")]
        REPORT,
    }

    /// <summary>
    /// 竞技场战报界面页签
    /// </summary>
    [System.Serializable]
    public class GGUIArenaBattleReportTabMono
    {
        [ALHeader("页签类型")]
        public EArenaBattleReportTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
    }

    /// <summary>
    /// 竞技场战报界面
    /// </summary>
    public class GGUIMonoArenaBattleReport : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIArenaBattleReportTabMono> monoTabList;
        [ALHeader("战报列表")]
        public GGUIMonoArenaBattleReportGrid monoBattleReportGrid;
        [ALHeader("对手列表")]
        public GGUIMonoArenaBattleReportOpponentGrid monoBattleReportOpponentGrid;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5205); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5205); } }
    }
}