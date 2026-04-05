using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间副本入口子窗口
    /// </summary>
    public class GGUISubMonoEveningDungeonEntrance : _AALBasicUIWndMono
    {
        [ALHeader("晚间副本活动持续时间")]
        public TextEx txtEveningDungeonDurationTime;
        
        [ALHeader("活动展示状态配置列表")]
        public List<GGUIEveningDungeonActivityStateShow> showStateList;

        [ALHeader("进入游戏按钮")]
        public GameObject btnGame;
        
        [ALHeader("藏品合成按钮")]
        public GameObject btnEquipCombine;
    }
}