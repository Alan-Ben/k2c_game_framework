using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 排行榜礼包
    /// </summary>
    public class GGUIMonoRankGiftPack: _AALBasicUIWndMono
    {
        [ALHeader("礼包名字")]
        public Text txtName;
        [ALHeader("价值百分比")]
        public Text txtProfitPer;
        [ALHeader("限购次数")]
        public Text txtLimit;
        [ALHeader("剩余时间")]
        public Text txtTime;
        [ALHeader("奖励道具列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("原价")]
        public Text txtOldCost;
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        [ALHeader("已售罄显示的go列表")]
        public List<GameObject> limitShowGoList;
        [ALHeader("未购买显示的go列表")]
        public List<GameObject> normalShowGoList;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(8900); } }
        public static string objName { get { return UIResPathAssistant.getObjName(8900);} }
    }
}