using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴替换藏品列表
    /// </summary>
    public class GGUIMonoHeroEquipChangeGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoHeroEquipChangeGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}