using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 冲榜详情前几名信息附加窗口
    /// </summary>
    public class GGUIMonoRankRushTopPlayerInfo : GGUIMonoBaseSubRankPlayerInfo
    {
        [ALHeader("信息为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("信息为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}