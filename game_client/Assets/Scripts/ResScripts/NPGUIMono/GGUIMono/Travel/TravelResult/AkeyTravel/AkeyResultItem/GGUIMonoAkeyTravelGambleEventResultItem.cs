using System;
using System.Collections.Generic;
using Common.TravelEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键游历 - 博彩事件结果item
    /// </summary>
    public class GGUIMonoAkeyTravelGambleEventResultItem : _AGGUIMonoAkeyTravelResultItem
    {
        [ALHeader("押注钻石通用组件（押注量）")]
        public GGUIMonoCommonSimpleItem monoBeforeItem;

        [ALHeader("结算钻石通用组件（结算量）")]
        public GGUIMonoCommonSimpleItem monoAfterItem;

        [ALHeader("钻石变化量文本")]
        public TextEx txtDiamondChange;

        [ALHeader("结果显示列表")]
        public List<NPCommonEnumStatMutexShowInfo<ETravelGambleResult>> resultShowList;
    }
}
