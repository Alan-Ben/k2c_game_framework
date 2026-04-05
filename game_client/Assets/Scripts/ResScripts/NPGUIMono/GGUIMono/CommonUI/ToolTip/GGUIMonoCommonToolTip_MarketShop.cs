using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 商铺详情跟随窗口
    /// </summary>
    public class GGUIMonoCommonToolTip_MarketShop : NPGGUIMonoCommonToolTip
    {
        [ALHeader("奖励当前倍数文本")]
        public Text txtCurMultiple;
        [ALHeader("奖励升级后倍数文本")]
        public Text txtNextMultiple;
        [ALHeader("奖励翻倍当前概率文本")]
        public Text txtCurProbability;
        [ALHeader("奖励升级后翻倍概率文本")]
        public Text txtNextProbability;
        [ALHeader("满级时奖励倍数文本")]
        public Text txtMaxLevelMultiple;
        [ALHeader("满级时奖励翻倍概率文本")]
        public Text txtMaxLevelProbability;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer rewardContainerMono;
        [ALHeader("满级显示的GoList")]
        public List<GameObject> goLvMaxShowList;
        [ALHeader("满级隐藏的GoList")]
        public List<GameObject> goLvMaxHideList;
    }
}