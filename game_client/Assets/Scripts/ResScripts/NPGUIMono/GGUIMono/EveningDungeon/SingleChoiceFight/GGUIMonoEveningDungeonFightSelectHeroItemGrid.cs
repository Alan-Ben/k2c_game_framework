using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动选择大臣出战item列表
    /// </summary>
    public class GGUIMonoEveningDungeonFightSelectHeroItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoEveningDungeonFightSelectHeroItem>
    {
        [ALHeader("列表没有时的提示")]
        public GameObject noneItemsTips;
    }
}