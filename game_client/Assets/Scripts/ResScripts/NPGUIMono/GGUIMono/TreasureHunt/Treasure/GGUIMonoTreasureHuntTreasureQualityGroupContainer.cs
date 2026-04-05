using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntTreasureQualityGroupContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntTreasureQualityGroupItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}