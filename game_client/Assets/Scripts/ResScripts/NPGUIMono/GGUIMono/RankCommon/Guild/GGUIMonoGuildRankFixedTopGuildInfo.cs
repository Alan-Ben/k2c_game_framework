using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟排行列表前几名item
    /// </summary>
    public class GGUIMonoGuildRankFixedTopGuildInfo : GGUIMonoBaseSubRankGuildInfo
    {
        [ALInfo("建议信息为空时的显隐GO 为 请求数据和解散显隐GO 的父节点")]
        [ALHeader("信息为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("信息为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}
