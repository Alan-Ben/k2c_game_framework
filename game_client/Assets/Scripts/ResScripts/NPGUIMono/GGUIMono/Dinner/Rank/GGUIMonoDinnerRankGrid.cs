using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会排行榜列表
    /// </summary>
    public class GGUIMonoDinnerRankGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoDinnerRankGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}