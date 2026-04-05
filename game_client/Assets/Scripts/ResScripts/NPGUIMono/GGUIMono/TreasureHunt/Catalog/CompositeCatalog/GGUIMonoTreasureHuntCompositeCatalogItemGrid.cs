using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴item列表
    /// </summary>
    public class GGUIMonoTreasureHuntCompositeCatalogItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoTreasureHuntCompositeCatalogItem>
    {
        [ALHeader("没有item时显示")]
        public List<GameObject> noItemShow;
    }
}