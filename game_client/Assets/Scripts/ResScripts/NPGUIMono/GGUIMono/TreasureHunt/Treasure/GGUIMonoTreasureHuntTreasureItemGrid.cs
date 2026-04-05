using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物itemGrid
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoTreasureHuntTreasureItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}