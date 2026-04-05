using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜附加窗口
    /// </summary>
    public class GGUIMonoArenaSubCelebrity : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("上榜条件描述")]
        public Text txtConditionDesc;
        [ALHeader("名人榜列表")]
        public GGUIMonoArenaSubCelebrityGrid monoSubCelebrityGrid;
        [ALHeader("上次选择描述")]
        public Text txtLastSelect;
        [ALHeader("再次挑战按钮")]
        public GameObject btnFightAgain;
        [ALHeader("可以再次挑战时显示的GO列表")]
        public List<GameObject> goCanFightAgainShowList;
        [ALHeader("可以再次挑战时隐藏的GO列表")]
        public List<GameObject> goCanFightAgainHideList;
    }
}