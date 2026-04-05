using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战报列表
    /// </summary>
    public class GGUIMonoArenaBattleReportGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoArenaBattleReportGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}