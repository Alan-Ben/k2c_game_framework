using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 午间副本和晚间副本入口窗口
    /// </summary>
    public class GGUIMonoDungeonEntrance : _ANPBasicUIWndResBarMono
    {
        [ALHeader("晚间活动入口子窗口")]
        public GGUISubMonoEveningDungeonEntrance monoEveningDungeonEntrance;

        [ALHeader("晚间活动入口子窗口")]
        public GGUISubMonoMiddayDungeonEnter monoMiddayDungeonEnter;
        
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5509); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5509);} }
    }
}