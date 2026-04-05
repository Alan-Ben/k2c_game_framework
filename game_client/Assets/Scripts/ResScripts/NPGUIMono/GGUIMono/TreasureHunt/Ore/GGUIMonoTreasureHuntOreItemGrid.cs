using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石itemGrid
    /// </summary>
    public class GGUIMonoTreasureHuntOreItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoTreasureHuntOreItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}