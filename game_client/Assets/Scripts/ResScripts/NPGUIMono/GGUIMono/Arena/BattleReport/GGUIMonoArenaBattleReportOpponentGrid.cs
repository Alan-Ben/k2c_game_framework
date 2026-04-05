using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战报对手列表
    /// </summary>
    public class GGUIMonoArenaBattleReportOpponentGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoArenaBattleReportOpponentGridItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}