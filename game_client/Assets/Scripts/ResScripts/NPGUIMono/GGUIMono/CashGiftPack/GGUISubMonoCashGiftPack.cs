using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 现金礼包子窗口
    /// </summary>
    public class GGUISubMonoCashGiftPack : _AALBasicUIWndMono
    {
        [ALHeader("礼包名称")]
        public Text txtGiftPackName;
        [ALHeader("礼包名称TMP文本")]
        public TMP_Text tmpGiftPackName;
        
        [ALHeader("礼包描述")]
        public Text txtGiftPackDesc;
        
        [ALHeader("价值百分比")]
        public Text txtProfitPer;
        [ALHeader("价值百分比")]
        public TMP_Text tmpProfitPer;
        [ALHeader("价值百分比Key, 一个参数, 价值百分比(代码会加入%显示, key中不需要包含%)")]
        public string profitPerKey;
        
        [ALHeader("限购次数")]
        public Text txtLimit;
        [ALHeader("购买按钮")]
        public GGUIMonoCommonBuyButton monoBuyButton;
        [ALHeader("特殊奖励道具")]
        public NPGGUIMonoCommonItem monoSpecialItem;
        [ALHeader("特殊奖励道具是否需要单独显示")]
        public bool specialItemNeedShowAsSingle = true;
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