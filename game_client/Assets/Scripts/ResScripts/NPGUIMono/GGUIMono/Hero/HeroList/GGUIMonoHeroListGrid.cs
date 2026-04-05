using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士列表
    /// </summary>
    public class GGUIMonoHeroListGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoHeroListGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}