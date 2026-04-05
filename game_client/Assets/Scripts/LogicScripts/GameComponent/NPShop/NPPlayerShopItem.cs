using NPEnum;
using ALPackage;
using Common.ShopObj;

namespace GOE
{
    // 商店中的商品数据结构
    public class NPPlayerShopItem
    {
        //商品实例id 
        private long _m_instanceId;

        //商品配表id
        private long _m_shopItemRefId;

        //折扣配置id
        private long _m_discountRefId;

        //已经购买次数
        private int _m_hasBuyNum;
        //购买次数上限
        private int _m_buyNumMax;

        //商品配表数据
        private NPShopItemRefObj _m_shopItemRef;

        //折扣配表数据
        private NPShopItemDiscountRefObj _m_shopItemDiscountRef;
        private readonly NPPlayerShop _m_shop;//所属商店
        private long _m_shopItemGroupId;
        private NPShopItemGroupRefObj _m_shopItemGruopRef;

        //构造函数
        public NPPlayerShopItem(Shop_ItemInfo _info, NPPlayerShop _shop)
        {
            if (null == _info)
                return;
            _m_shop = _shop;
            _m_instanceId = _info.getInstanceId();
            _m_shopItemRefId = _info.getShopItemRefId();
            _m_discountRefId = _info.getDiscountRefId();
            _m_hasBuyNum = (int)_info.getHasBuyNum();
            _m_buyNumMax = (int)_info.getCanBuyNum();
            _m_shopItemGroupId = _info.getShopItemGroupId();

            _m_shopItemRef = GRefdataCoreMgr.instance.shopItemMap.getRef(_m_shopItemRefId);
            if (null == _m_shopItemRef)
                ALLog.Error($"can not find {_m_shopItemRefId}'s NPShopItemRefObj!");
            
            _m_shopItemGruopRef = GRefdataCoreMgr.instance.shopItemGroupMap.getRef(_m_shopItemGroupId);
            if (null == _m_shopItemGruopRef)
                ALLog.Error($"can not find {_m_shopItemGroupId}'s NPShopItemGroupRefObj!");

            _m_shopItemDiscountRef = GRefdataCoreMgr.instance.shopItemDiscountMap.getRef(_m_discountRefId);
            if (null == _m_shopItemDiscountRef && _m_discountRefId != 0)
                ALLog.Error($"can not find {_m_discountRefId}'s NPShopItemDiscountRefObj!");
        }

        /// <summary>
        /// 商品配表id
        /// </summary>
        public long shopItemRefId { get { return _m_shopItemRefId; } }
        
        /// <summary>
        /// 商品组配置
        /// </summary>
        public NPShopItemGroupRefObj shopItemGruopRef { get { return _m_shopItemGruopRef; } }

        public NPPlayerShop shop { get { return _m_shop; } }
        public long instanceId { get { return _m_instanceId; } }

        //推荐标识路径id
        public int recommendResId { get { return null == _m_shopItemRef ? 0 : _m_shopItemRef.ui_res_id; } }

        //折扣资源路径id
        public int discountUIResPathId { get { return null == _m_shopItemDiscountRef ? 0 : _m_shopItemDiscountRef.ui_res_id; } }

        //折扣配置id
        public long discountRefId { get { return _m_discountRefId; } }

        //折扣
        public long discount { get { return null == _m_shopItemDiscountRef ? 0 : _m_shopItemDiscountRef.discount; } }

        //获得的物品-展示用
        public NPCommonCostItem gainItem { get { return null == _m_shopItemRef ? null : _m_shopItemRef.item; } }

        //剩余购买次数
        public int lastBuyCount { get { return _m_buyNumMax - _m_hasBuyNum; } }

        //已经购买次数
        public int hasBuyNum { get { return _m_hasBuyNum; } }
        public int buyNumMax { get { return _m_buyNumMax; } }

        //排序id
        public long sortId { get { return null == _m_shopItemRef ? 0 : _m_shopItemRef.sort_id; } }

        //次数递增id
        public long timePriceTypeId { get { return null == _m_shopItemRef ? 0 : _m_shopItemRef.times_price_type_id; } }

        //是否为推荐商品
        public bool isRecommend { get { return null == _m_shopItemRef ? false : _m_shopItemRef.is_recommend; } }

        /// <summary>
        /// 当前购买该商品需要消耗的Item 打折之后的
        /// </summary>
        /// <returns></returns>
        public NPCommonCostItem getCostItem()
        {
            if (null == _m_shopItemRef)
                return null;

            long count = 0;
            NPCommonItem item = null;
            TimesPriceRefObj costRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_shopItemRef.times_price_type_id, _m_hasBuyNum + 1);

            //如果有递增消耗配置，则优先使用递增消耗配置，否则使用固定消耗
            if (null != costRef && null != costRef.cost_item_formula)
            {
                count = costRef.cost_item_formula.CalculateVariableResult(null);
                item = costRef.item;
            }
            else
            {
                if (null != _m_shopItemRef.cost_item)
                {
                    count = _m_shopItemRef.cost_item.count;
                    item = _m_shopItemRef.cost_item.item;

                    if (null != _m_shopItemDiscountRef)
                        count = count * _m_shopItemDiscountRef.discount / 10000;
                }
            }

            if (null == item || item.itemType == ENPItemType.NONE || count == 0)
                return null;

            return new NPCommonCostItem(item, count);
        }

        /// <summary>
        /// 更新商品 - 目前只有购买数量会变动
        /// </summary>
        /// <param name="_item"></param>
        public void update(Shop_ItemInfo _item)
        {
            if (null == _item)
                return;

            _m_hasBuyNum = (int)_item.getHasBuyNum();

            WinMsg.SendMsg(WinMsgType.SHOP_ITEM_CHG, _item.getInstanceId());
        }

        /// <summary>
        /// 获取商品原来的价格
        /// </summary>
        /// <returns></returns>
        public int getBeforeDiscountCount()
        {
            if (null == _m_shopItemRef)
                return 0;

            TimesPriceRefObj timesPriceRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_shopItemRef.times_price_type_id, _m_hasBuyNum + 1);
            if (null != timesPriceRef && null != timesPriceRef.cost_item_formula)
            {
                int curCount = (int)timesPriceRef.cost_item_formula.CalculateVariableResult(null);
                if (timesPriceRef.discount > 0)
                    return (int) (curCount / (timesPriceRef.discount / 10000));
                else
                    return curCount;
            }

            if (null != _m_shopItemRef.cost_item)
                return (int)_m_shopItemRef.cost_item.count;

            return 0;
        }
    }
}
