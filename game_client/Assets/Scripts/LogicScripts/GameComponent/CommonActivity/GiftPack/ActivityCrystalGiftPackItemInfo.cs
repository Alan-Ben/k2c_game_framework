using System.Collections.Generic;
using Common.CommonFuncObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 活动钻石礼包道具信息
    /// </summary>
    public class ActivityCrystalGiftPackItemInfo : _IGiftPackInfo
    {
        //活动实例id
        private long _m_lActivityInstanceId;
        //钻石礼包id
        private long _m_lCrystalGiftPackId;
        //钻石礼包配置数据
        private CrystalGiftPackRefObj _m_crystalGiftPackRef;
        //已购买次数
        private long _m_lHasBuyCount;


        /// <summary>
        /// 活动实例id
        /// </summary>
        public long activityInstanceId { get { return _m_lActivityInstanceId; } }
        /// <summary>
        /// 钻石礼包id
        /// </summary>
        public long crystalGiftPackId { get { return _m_lCrystalGiftPackId; } }
        /// <summary>
        /// 活动商店道具配置数据
        /// </summary>
        public CrystalGiftPackRefObj crystalGiftPackRef { get { return _m_crystalGiftPackRef; } }
        /// <summary>
        /// 已购买次数
        /// </summary>
        public long hasBuyCount { get { return _m_lHasBuyCount; } }
        /// <summary>
        /// 剩余购买次数
        /// </summary>
        public long leftBuyCount { get { return _m_crystalGiftPackRef == null ? 0 : _m_crystalGiftPackRef.buy_num - _m_lHasBuyCount; } }
        /// <summary>
        /// 可购买的最大次数
        /// </summary>
        public long buyCountMax { get { return _m_crystalGiftPackRef == null ? 0 : _m_crystalGiftPackRef.buy_num; } }
        /// <summary>
        /// 是否已售罄
        /// </summary>
        public bool isSellOut { get { return leftBuyCount <= 0; } }
        /// <summary>
        /// 是否免费
        /// </summary>
        public bool isFree
        {
            get
            {
                return _m_crystalGiftPackRef != null &&
                       _m_crystalGiftPackRef.times_price_type_id <= 0 &&
                       (_m_crystalGiftPackRef.cost_item == null || _m_crystalGiftPackRef.cost_item.item == null || _m_crystalGiftPackRef.cost_item.item.itemType == ENPItemType.NONE);
            }
        }
        /// <summary>
        /// 次数递增表类型id
        /// </summary>
        public long timePriceTypeId { get {return _m_crystalGiftPackRef != null ? _m_crystalGiftPackRef.times_price_type_id : 0; } }
        /// <summary>
        /// 折扣
        /// </summary>
        public long discount { get { return _m_crystalGiftPackRef != null ? _m_crystalGiftPackRef.discount : 0; } }
        /// <summary>
        /// 需要消耗的物品
        /// </summary>
        public NPCommonCostItem costItem { get { return _m_crystalGiftPackRef != null ? _m_crystalGiftPackRef.cost_item : null; } }
        /// <summary>
        /// 可获得的物品列表
        /// </summary>
        public List<NPCommonCostItem> gainItemList {get{return _m_crystalGiftPackRef != null ? _m_crystalGiftPackRef.item_list : null; } }
        /// <summary>
        /// 礼包名称
        /// </summary>
        public string giftPackName { get {return _m_crystalGiftPackRef != null ? TextTranslate.instance.getLanguage(_m_crystalGiftPackRef.name) : null; } }

        public ActivityCrystalGiftPackItemInfo(long _activityInstanceId, CrystalGiftPackRefObj _crystalGiftPackRef)
        {
            if (_crystalGiftPackRef == null)
                return;

            _m_lActivityInstanceId = _activityInstanceId;
            _m_lCrystalGiftPackId = _crystalGiftPackRef.id;
            _m_crystalGiftPackRef = _crystalGiftPackRef;
            _m_lHasBuyCount = 0;
        }

        /// <summary>
        /// 更新已购买数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateBuyCount(CrystalGiftPack_BuyRecord _info)
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
            if (null == _m_crystalGiftPackRef)
                return null;

            TimesPriceRefObj timePriceRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_crystalGiftPackRef.times_price_type_id, _m_lHasBuyCount + 1);

            long count = 0;
            NPCommonItem item = null;

            if (null != _m_crystalGiftPackRef.cost_item)
            {
                count = _m_crystalGiftPackRef.cost_item.count;
                item = _m_crystalGiftPackRef.cost_item.item;
            }

            if (null != timePriceRef && null != timePriceRef.cost_item_formula)
            {
                count = timePriceRef.cost_item_formula.CalculateVariableResult(null);
                item = timePriceRef.item;
            }

            if (null == item || item.itemType == ENPItemType.NONE || count == 0)
                return null;

            if (null != _m_crystalGiftPackRef && _m_crystalGiftPackRef.discount > 0)
                count = count * _m_crystalGiftPackRef.discount / 10000;

            return new NPCommonCostItem(item, count);
        }

        /// <summary>
        /// 获取商品原来的价格
        /// </summary>
        /// <returns></returns>
        public int getBeforeDiscountCount()
        {
            if (null == _m_crystalGiftPackRef)
                return 0;

            TimesPriceRefObj costRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_crystalGiftPackRef.times_price_type_id, _m_lHasBuyCount + 1);
            if (null != costRef && null != costRef.cost_item_formula)
                return (int)costRef.cost_item_formula.CalculateVariableResult(null);

            if (null != _m_crystalGiftPackRef.cost_item)
                return (int)_m_crystalGiftPackRef.cost_item.count;

            return 0;
        }
    }
}