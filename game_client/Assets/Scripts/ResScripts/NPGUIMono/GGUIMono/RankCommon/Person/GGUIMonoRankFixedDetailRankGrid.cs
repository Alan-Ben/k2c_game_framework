using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜详情排行榜列表
    /// </summary>
    public class GGUIMonoRankFixedDetailRankGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoRankFixedDetailRankGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}