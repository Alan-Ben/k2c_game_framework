using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石itemContainer
    /// </summary>
    public class GGUIMonoTreasureHuntOreItemContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntOreItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}