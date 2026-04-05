using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主界面藏品列表
    /// </summary>
    public class GGUIMonoEquipMainListGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoEquipMainListGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}