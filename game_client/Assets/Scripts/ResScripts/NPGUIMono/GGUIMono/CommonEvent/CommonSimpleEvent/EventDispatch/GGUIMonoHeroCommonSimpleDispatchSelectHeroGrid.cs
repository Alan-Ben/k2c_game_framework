using UnityEngine;

namespace GOE
{
    public class GGUIMonoHeroCommonSimpleDispatchSelectHeroGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoHeroCommonSimpleDispatchSelectHeroItem>
    {
        // 无物品提示
        [ALHeader("没有item时显示提示")]
        public GameObject noneItemsTips;
    }
}