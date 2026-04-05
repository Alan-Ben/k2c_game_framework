using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜详情前几名信息附加窗口
    /// </summary>
    public class GGUIMonoRankFixedTopPlayerInfo : GGUIMonoBaseSubRankPlayerInfo
    {
        [ALHeader("信息为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("信息为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}