using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴列表
    /// </summary>
    public class GGUIMonoArenaBattleSelectHeroGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoArenaBattleSelectHeroGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}