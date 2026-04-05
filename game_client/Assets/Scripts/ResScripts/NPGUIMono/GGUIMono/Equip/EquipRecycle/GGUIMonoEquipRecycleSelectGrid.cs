using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品分解藏品选择列表item
    /// </summary>
    public class GGUIMonoEquipRecycleSelectGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoEquipRecycleSelectGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}