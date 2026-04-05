using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面钻石礼包页面item
    /// </summary>
    public class GGUIMonoActivityCrystalGiftPackPageContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("礼包名称")]
        public Text txtName;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("剩余购买次数")]
        public Text txtLeftBuyCount;
        [ALHeader("购买按钮")]
        public GameObject btnBuy;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("免费时需要显示的GO列表")]
        public List<GameObject> goFreeShowList;
        [ALHeader("免费时需要隐藏的GO列表")]
        public List<GameObject> goFreeHideList;
        [ALHeader("售罄时需要展示的GO列表")]
        public List<GameObject> goSellOutShowList;
        [ALHeader("售罄时需要隐藏的GO列表")]
        public List<GameObject> goSellOutHideList;
    }
}
