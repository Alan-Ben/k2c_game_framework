using Common.ActivityObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 活动商店道具信息
    /// </summary>
    public class ActivityShopItemInfo
    {
        //活动实例id
        private long _m_lActivityInstanceId;
        //活动商店道具id
        private long _m_lActivityShopItemId;
        //活动商店道具配置数据
        private ActivityShopItemRefObj _m_activityShopItemRef;
        //已购买次数
        private long _m_lHasBuyCount;

        /// <summary>
        /// 活动实例id
        /// </summary>
        public long activityInstanceId { get { return _m_lActivityInstanceId; } }
        /// <summary>
        /// 活动商店道具id
        /// </summary>
        public long activityShopItemId { get { return _m_lActivityShopItemId; } }
        /// <summary>
        /// 活动商店道具配置数据
        /// </summary>
        public ActivityShopItemRefObj activityShopItemRef { get { return _m_activityShopItemRef; } }
        /// <summary>
        /// 已购买次数
        /// </summary>
        public long hasBuyCount { get { return _m_lHasBuyCount; } }
        /// <summary>
        /// 剩余购买次数
        /// </summary>
        public long leftBuyCount { get { return _m_activityShopItemRef == null ? 0 : _m_activityShopItemRef.buy_num - _m_lHasBuyCount; } }
        /// <summary>
        /// 是否已售罄
        /// </summary>
        public bool isSellOut { get { return leftBuyCount <= 0; } }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool isRecommend { get { return _m_activityShopItemRef != null ? _m_activityShopItemRef.is_recommend : false; } }

        public ActivityShopItemInfo(long _activityInstanceId, ActivityShopItemRefObj _shopItemRef)
        {
            if (_shopItemRef == null)
                return;

            _m_lActivityInstanceId = _activityInstanceId;
            _m_lActivityShopItemId = _shopItemRef.id;
            _m_activityShopItemRef = _shopItemRef;
            _m_lHasBuyCount = 0;
        }

        /// <summary>
        /// 更新已购买数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateBuyCount(Activity_ShopBuyRecord _info)
        {
            if (_info == null)
                return;

            _m_lHasBuyCount = _info.getHadBuyCount();
        }

        /// <summary>
        /// 当前购买该商品需要消耗的Item 打折之后的
        /// </summary>
        /// <returns></returns>
        public NPCommonCostItem getCostItemByDiscount()
        {
            if (null == _m_activityShopItemRef)
                return null;

            long count = 0;
            NPCommonItem item = null;
            TimesPriceRefObj timePriceRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_activityShopItemRef.times_price_type_id, _m_lHasBuyCount + 1);

            //如果有递增消耗配置，则优先使用递增消耗配置，否则使用固定消耗
            if (null != timePriceRef && null != timePriceRef.cost_item_formula)
            {
                count = timePriceRef.cost_item_formula.CalculateVariableResult(null);
                item = timePriceRef.item;
            }
            else
            {
                if (null != _m_activityShopItemRef.cost_item)
                {
                    count = _m_activityShopItemRef.cost_item.count;
                    item = _m_activityShopItemRef.cost_item.item;

                    if (null != _m_activityShopItemRef && _m_activityShopItemRef.discount > 0)
                        count = count * _m_activityShopItemRef.discount / 10000;
                }
            }
            
            if (null == item || item.itemType == ENPItemType.NONE || count == 0)
                return null;

            return new NPCommonCostItem(item, count);
        }

        /// <summary>
        /// 获取商品原来的价格
        /// </summary>
        /// <returns></returns>
        public int getBeforeDiscountCount()
        {
            if (null == _m_activityShopItemRef)
                return 0;

            TimesPriceRefObj timesPriceRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_activityShopItemRef.times_price_type_id, _m_lHasBuyCount + 1);
            if (null != timesPriceRef && null != timesPriceRef.cost_item_formula)
            {
                int curCount = (int)timesPriceRef.cost_item_formula.CalculateVariableResult(null);
                if (timesPriceRef.discount > 0)
                    return (int)(curCount / (timesPriceRef.discount / 10000));
                else
                    return curCount;
            }

            if (null != _m_activityShopItemRef.cost_item)
                return (int)_m_activityShopItemRef.cost_item.count;

            return 0;
        }
    }
}