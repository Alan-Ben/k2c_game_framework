using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 矿石品质组itemContainer
    /// </summary>
    public class GGUIMonoTreasureHuntOreQualityGroupContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntOreQualityGroupItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}