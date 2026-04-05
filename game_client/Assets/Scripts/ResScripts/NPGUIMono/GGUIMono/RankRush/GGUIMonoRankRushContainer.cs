using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜详情奖励页面列表
    /// </summary>
    public class GGUIMonoRankRushContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoRankRushContainerItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}