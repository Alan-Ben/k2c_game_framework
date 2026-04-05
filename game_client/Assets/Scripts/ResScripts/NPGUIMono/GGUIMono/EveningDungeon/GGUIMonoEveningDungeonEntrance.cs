using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间副本入口
    /// </summary>
    public class GGUIMonoEveningDungeonEntrance : _ANPBasicUIWndResBarMono
    {
        [ALHeader("晚间副本活动持续时间")]
        public TextEx txtEveningDungeonDurationTime;
        
        [ALHeader("活动展示状态配置列表")]
        public List<GGUIEveningDungeonActivityStateShow> showStateList;

        [ALHeader("进入游戏按钮")]
        public GameObject btnGame;
        
        [ALHeader("藏品合成按钮")]
        public GameObject btnEquipCombine;

        [ALHeader("排行榜按钮")]
        public GameObject btnRank;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5508); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5508);} }
    }
}