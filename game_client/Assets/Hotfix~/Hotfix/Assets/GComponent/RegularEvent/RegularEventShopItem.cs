using GOE;
using Hotfix.Common.HotSimpleActivityObj;
using NPEnum;

namespace Hotfix
{
    /// <summary>
    /// 万能万能活动消耗商店商品
    /// </summary>
    public class RegularEventShopItem
    {
        //万能活动实例id
        private long _m_lActivityInstanceId;
        //万能活动商店道具id
        private long _m_lRegularEventShopItemId;
        //万能活动商店道具配置数据
        private RegularEventShopItemRefObj _m_regularEventShopItemRef;
        //已购买次数
        private long _m_lHasBuyCount;

        /// <summary>
        /// 万能活动信息
        /// </summary>
        public long activityInstanceId { get { return _m_lActivityInstanceId; } }
        /// <summary>
        /// 万能活动商店道具id
        /// </summary>
        public long regularEventShopItemId { get { return _m_lRegularEventShopItemId; } }
        /// <summary>
        /// 万能活动商店道具配置数据
        /// </summary>
        public RegularEventShopItemRefObj regularEventShopItemRef { get { return _m_regularEventShopItemRef; } }
        /// <summary>
        /// 最大购买次数
        /// </summary>
        public long maxBuyCount { get { return _m_regularEventShopItemRef != null ? _m_regularEventShopItemRef.buy_num : 0; } }
        /// <summary>
        /// 已购买次数
        /// </summary>
        public long hasBuyCount { get { return _m_lHasBuyCount; } }
        /// <summary>
        /// 剩余购买次数
        /// </summary>
        public long leftBuyCount { get { return _m_regularEventShopItemRef == null ? 0 : _m_regularEventShopItemRef.buy_num - _m_lHasBuyCount; } }
        /// <summary>
        /// 是否已售罄
        /// </summary>
        public bool isSellOut { get { return leftBuyCount <= 0; } }
        /// <summary>
        /// 是否免费
        /// </summary>
        public bool isFree { get { return _m_regularEventShopItemRef != null && (_m_regularEventShopItemRef.buy_cost == null || _m_regularEventShopItemRef.buy_cost.getItemType() == ENPItemType.NONE); } }

        public RegularEventShopItem(long _activityInstanceId, RegularEventShopItemRefObj _itemRef)
        {
            if (_itemRef == null)
                return;

            _m_lActivityInstanceId = _activityInstanceId;
            _m_lRegularEventShopItemId = _itemRef.id;
            _m_regularEventShopItemRef = _itemRef;
            _m_lHasBuyCount = 0;
        }

        /// <summary>
        /// 更新已购买数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateBuyCount(RegularActivity_ShopBuyRecord _info)
        {
            if (_info == null || _info.getItemId() != _m_lRegularEventShopItemId)
                return;

            updateBuyCount(_info.getBuyCount());
        }
        public void updateBuyCount(long _buyCount)
        {
            _m_lHasBuyCount = _buyCount;
        }
    }
}