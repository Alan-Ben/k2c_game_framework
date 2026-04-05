
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜获取途径界面列表
    /// </summary>
    public class GGUIMonoRankRushAccessContainer : _ATNPGGUIMonoShowAnimContainer<NPGGUIMonoAccessWayItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}