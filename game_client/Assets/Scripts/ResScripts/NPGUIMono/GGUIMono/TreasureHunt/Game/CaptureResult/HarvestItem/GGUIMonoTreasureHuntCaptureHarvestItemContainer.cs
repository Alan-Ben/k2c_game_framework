using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝-打捞收获物item列表
    /// </summary>
    public class GGUIMonoTreasureHuntCaptureHarvestItemContainer: _ATNPGGUIMonoShowAnimContainer<GGUIMonoTreasureHuntCaptureHarvestItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}