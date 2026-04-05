using System.Collections.Generic;
using GOE;
using UnityEngine;
using UnityEngine.UI;


namespace Hotfix
{
    /// <summary>
    /// 消耗商店container item
    /// </summary>
    public class GGUIMonoRegularEventShopContainerItem : _AHotfixBaseMono
    {
        [HotfixMonoAttribute("购买的道具")]
        public NPGGUIMonoCommonItem monoTargetItem;
        [HotfixMonoAttribute("增加的积分描述")]
        public TextEx txtUseAddPoint;
        [HotfixMonoAttribute("购买限制")]
        public TextEx txtBuyLimit;
        [HotfixMonoAttribute("免费购买按钮")]
        public GameObject btnFree;
        [HotfixMonoAttribute("消耗购买按钮")]
        public GameObject btnBuy;
        [HotfixMonoAttribute("获取途径按钮")]
        public GameObject btnAccess;
        [HotfixMonoAttribute("消耗的道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [HotfixMonoAttribute("购买数量计数器")]
        public GGUIMonoBagPopCounter monoUseCounter;
        [HotfixMonoAttribute("免费时需要显示的GO列表")]
        public List<GameObject> goFreeShowList;
        [HotfixMonoAttribute("免费时需要隐藏的GO列表")]
        public List<GameObject> goFreeHideList;
        [HotfixMonoAttribute("售罄时需要显示的GO列表")]
        public List<GameObject> goSellOutShowList;
        [HotfixMonoAttribute("售罄时需要隐藏的GO列表")]
        public List<GameObject> goSellOutHideList;
    }
}