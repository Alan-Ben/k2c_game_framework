using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场排行榜列表
    /// </summary>
    public class GGUIMonoArenaRankGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoArenaRankGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}