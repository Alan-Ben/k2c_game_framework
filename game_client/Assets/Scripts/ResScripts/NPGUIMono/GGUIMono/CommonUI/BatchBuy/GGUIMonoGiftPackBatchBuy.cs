using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 礼包批量购买商品弹窗
    /// </summary>
    public class GGUIMonoGiftPackBatchBuy : _AALBasicUIWndMono
    {
        [ALHeader("购买的物品列表")]
        public NPGGUIMonoCommonItemContainer monoGainItemContainer;
        [ALHeader("限购文本")]
        public Text limitBuyTxt;
        [ALHeader("有限购时显示的Go List")]
        public List<GameObject> hasBuyLimitList;
        [ALHeader("商品为免费时需要显示的Go List")]
        public List<GameObject> freeShowGoList;
        [ALHeader("商品为免费时需要隐藏的Go List")]
        public List<GameObject> freeHideGoList;
        [ALHeader("购买数量计数器")]
        public GGUIMonoBagPopCounter useCounter;
        [ALHeader("消耗足够展示的GoList")]
        public List<GameObject> costEnoughShowGoList;
        [ALHeader("消耗不足展示的GoList")]
        public List<GameObject> costNoEnoughShowGoList;
        [ALHeader("消耗展示")]
        public NPGGUIMonoCommonItem costUseItem;
        [ALHeader("消耗按钮")]
        public GameObject costUseBtn;
        [ALHeader("取消按钮")]
        public GameObject btnCancel;
        [ALHeader("关闭按钮")]
        public GameObject closeBtn;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6002); } }
    }
}

