using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜列表
    /// </summary>
    public class GGUIMonoArenaSubCelebrityGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoArenaSubCelebrityGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}