using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTreasureHuntAkeyCaptureResultContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntAkeyCaptureResultItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}