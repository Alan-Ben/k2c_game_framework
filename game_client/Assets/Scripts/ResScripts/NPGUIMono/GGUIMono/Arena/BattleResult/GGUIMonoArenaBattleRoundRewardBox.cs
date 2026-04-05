using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合连胜奖励宝箱
    /// </summary>
    public class GGUIMonoArenaBattleRoundRewardBox : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("奖励道具")]
        public NPGGUIMonoCommonItem monoItem;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
    } 
}