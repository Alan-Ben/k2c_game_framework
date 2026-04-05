using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情前几名信息附加窗口
    /// </summary>
    public class GGUIMonoRankRushTopGuildInfo : GGUIMonoBaseSubRankGuildInfo
    {
        [ALInfo("建议信息为空时的显隐GO 为 请求数据和解散显隐GO 的父节点")]
        [ALHeader("信息为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("信息为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}