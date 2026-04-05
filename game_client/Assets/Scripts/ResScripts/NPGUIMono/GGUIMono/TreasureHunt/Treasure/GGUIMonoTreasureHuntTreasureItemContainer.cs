using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物itemContainer
    /// </summary>
    public class GGUIMonoTreasureHuntTreasureItemContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntTreasureItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}