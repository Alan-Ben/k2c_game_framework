using System.Collections.Generic;
using Common.CommonFuncObj;

namespace GOE
{
    /// <summary>
    /// 礼包信息
    /// </summary>
    public interface _IGiftPackInfo
    {
        /// <summary>
        /// 是否免费
        /// </summary>
        bool isFree { get; }
        /// <summary>
        /// 需要消耗的物品
        /// </summary>
        NPCommonCostItem costItem { get; }
        /// <summary>
        /// 已购买次数
        /// </summary>
        long hasBuyCount { get; }
        /// <summary>
        /// 剩余购买次数
        /// </summary>
        long leftBuyCount { get; }
        /// <summary>
        /// 可购买最大次数
        /// </summary>
        long buyCountMax { get; }
        /// <summary>
        /// 次数递增表类型id
        /// </summary>
        long timePriceTypeId { get; }
        /// <summary>
        /// 折扣
        /// </summary>
        long discount { get; }
        /// <summary>
        /// 可获得的物品列表
        /// </summary>
        List<NPCommonCostItem> gainItemList { get; }
        /// <summary>
        /// 礼包名称
        /// </summary>
        string giftPackName { get; }
        /// <summary>
        /// 获取打折之后的消耗物品
        /// </summary>
        /// <returns></returns>
        NPCommonCostItem getCostItemByDiscount();
    }
}