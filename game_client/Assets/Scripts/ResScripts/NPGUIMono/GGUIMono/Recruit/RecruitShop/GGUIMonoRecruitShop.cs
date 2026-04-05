using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// 召唤商店
    /// </summary>
    public class GGUIMonoRecruitShop : _AALBasicUIWndMono
    {
        [ALHeader("招募消耗的物品")]
        public NPGGUIMonoCommonItem monoRecruitCostItem;

        [FormerlySerializedAs("monoRecruitShopItemContainer")] [ALHeader("招募商店的物品容器")]
        public GGUIMonoRecruitShopCardItemContainer monoRecruitShopCardItemContainer;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
    }
}