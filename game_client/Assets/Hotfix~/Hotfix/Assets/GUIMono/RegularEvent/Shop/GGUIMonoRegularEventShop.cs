using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店页签类型
    /// </summary>
    public class RegularEventShopTabType
    {
        public const string SHOP = "SHOP";//商店
        public const string WAREHOUSE = "WAREHOUSE";//仓库
    }

    /// <summary>
    /// 万能活动消耗商店界面
    /// </summary>
    public class GGUIMonoRegularEventShop : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("关闭按钮")]
        public GameObject btnClose;
        [HotfixMonoAttribute("页签列表")]
        public List<GGUIMonoActivityCommonTab> monoTabList;
        [HotfixMonoAttribute("商店列表")]
        public GGUIHotfixCommonMono monoShopContainer;
        [HotfixMonoAttribute("仓库列表")]
        public GGUIHotfixCommonMono monoWarehouseContainer;
    }
}