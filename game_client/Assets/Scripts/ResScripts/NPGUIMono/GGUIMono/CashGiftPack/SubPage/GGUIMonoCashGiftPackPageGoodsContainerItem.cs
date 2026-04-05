using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 现金礼包组商品列表item
    /// </summary>
    public class GGUIMonoCashGiftPackPageGoodsContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("礼包名称")]
        public Text txtGiftPackName;
        [ALHeader("价值百分比")]
        public Text txtProfitPer;
        [ALHeader("限购次数")]
        public Text txtLimit;
        [ALHeader("购买按钮")]
        public GGUIMonoCommonBuyButton monoBuyButton;
        [ALHeader("特殊奖励道具")]
        public NPGGUIMonoCommonItem monoSpecialItem;
        [ALHeader("奖励道具列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("没有价值百分比时需要隐藏的GO列表")]
        public List<GameObject> goNoProfitPerHideList;
        [ALHeader("免费时需要显示的GO列表")]
        public List<GameObject> goFreeShowList;
        [ALHeader("免费时需要隐藏的GO列表")]
        public List<GameObject> goFreeHideList;
        [ALHeader("可购买时需要显示的GO列表")]
        public List<GameObject> goCanBuyShowList;
        [ALHeader("可购买时需要隐藏的GO列表")]
        public List<GameObject> goCanBuyHideList;
    }
}
