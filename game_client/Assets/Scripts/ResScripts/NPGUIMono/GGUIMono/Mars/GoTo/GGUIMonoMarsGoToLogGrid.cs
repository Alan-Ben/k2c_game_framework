using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星航行日志列表
    /// </summary>
    public class GGUIMonoMarsGoToLogGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoMarsGoToLogGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}