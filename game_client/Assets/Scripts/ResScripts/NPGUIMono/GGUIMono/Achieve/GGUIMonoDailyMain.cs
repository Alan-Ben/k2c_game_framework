using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 页签类型
    /// </summary>
    public enum EDailyTab
    {
        ACHIEVE,//成就
        DAILY_QUEST,//每日任务
        DAILY_CHECK,//每日签到
    }

    //页签类型
    [System.Serializable]
    public class GGUIDailyTabMono
    {
        [ALHeader("页签类型")]
        public EDailyTab tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("页签对应的子窗口")]
        public long tabSubWndAssetId;
    }

    /// <summary>
    /// 成就主窗口
    /// </summary>
    public class GGUIMonoDailyMain : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签列表")]
        public List<GGUIDailyTabMono> monoTabList;
        [ALHeader("子窗口页面")]
        public Transform pageParent;

        /************
       * 资源加载路径
       */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3000); } }
    }
}
