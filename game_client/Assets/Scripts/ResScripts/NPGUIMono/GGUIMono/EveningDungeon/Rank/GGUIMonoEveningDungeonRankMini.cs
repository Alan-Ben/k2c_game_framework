using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动排行小窗口
    /// </summary>
    public class GGUIMonoEveningDungeonRankMini : _AALBasicUIWndMono
    {
        [ALHeader("第一名排行信息的描述")]
        public TextEx firstRankInfoDesc;

        [ALHeader("有排行数据时显示")]
        public List<GameObject> hasRankInfoShow;
        
        [ALHeader("打开排行窗口按钮")]
        public GameObject openRankBtn;
    }
}