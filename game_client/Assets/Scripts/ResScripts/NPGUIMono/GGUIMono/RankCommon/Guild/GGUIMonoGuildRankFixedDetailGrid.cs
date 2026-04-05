
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟排行列表
    /// </summary>
    public class GGUIMonoGuildRankFixedDetailGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoGuildRankFixedDetailGridItem>
    {
        [ALHeader("列表为空时显示")]
        public List<GameObject> noItemShow;
    }
}
