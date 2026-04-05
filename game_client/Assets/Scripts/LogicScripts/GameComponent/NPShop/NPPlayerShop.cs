using System.Collections.Generic;
using NPEnum;
using ALPackage;
using Common.ShopObj;

namespace GOE
{
    // 商店数据结构
    public class NPPlayerShop
    {
        //商店id -配表id
        private long _m_shopRefId;

        //配表数据
        private NPShopRefObj _m_shopRef;

        //下一次自动刷新时间戳
        private long _m_nextRefreshTimeMs;

        //当前付费刷新次数
        private int _m_refreshNum;

        //商品列表
        private List<NPPlayerShopItem> _m_shopItemList;

        //构造函数
        public NPPlayerShop(Shop_Info _info)
        {
            if (null == _info)
                return;

            _m_shopRefId = _info.getShopRefId();
            _m_nextRefreshTimeMs = _info.getNextRefreshTimeMs();
            _m_refreshNum = _info.getRefreshNum();

            _m_shopItemList = new List<NPPlayerShopItem>();
            if (null != _info.getGoodsList())
            {
                Shop_ItemInfo temp = null;
                for (int i = 0; i < _info.getGoodsList().Count; i++)
                {
                    temp = _info.getGoodsList()[i];
                    if (null == temp)
                        continue;

                    NPPlayerShopItem shopItem = new NPPlayerShopItem(temp,this);
                    _m_shopItemList.Add(shopItem);
                }
            }

            _m_shopRef = GRefdataCoreMgr.instance.shopMap.getRef(_m_shopRefId);
            if (null == _m_shopRef)
                ALLog.Error($"can not find {_m_shopRefId}'s NPShopRefObj!");
        }

        public long shopRefId { get { return _m_shopRefId; } }

        public long nextRefreshTimeMs { get { return _m_nextRefreshTimeMs; } }

        //免费刷新消耗
        public NPCommonItem freeRefreshCostItem { get { return null == _m_shopRef ? null : _m_shopRef.free_refresh_cd; } }

        //当前已经刷新的次数
        public int refreshCount { get { return _m_refreshNum; } }

        //付费刷新上限
        public int refreshMaxCount { get { return null == _m_shopRef ? 0 : _m_shopRef.pay_refresh_limit; } }

        public NPShopRefObj shopRefObj { get { return _m_shopRef; } }

        //剩余免费刷新次数
        public int freeRefreshCount
        {
            get
            {
                if (null == _m_shopRef || null == _m_shopRef.free_refresh_cd)
                    return 0;
                
                int hasCount = (int)GCommon.getItemCount(_m_shopRef.free_refresh_cd);
                return hasCount;
            }
        }
        
        //剩余免费刷新次数
        public int freeRefreshMaxCount
        {
            get
            {
                if (null == _m_shopRef || null == _m_shopRef.free_refresh_cd)
                    return 0;

                int max = 0;
                switch (_m_shopRef.free_refresh_cd.itemType)
                {
                    case ENPItemType.LAZY_CD:
                        max = NPPlayer.instance.lazyCdComp.getMaxCount(_m_shopRef.free_refresh_cd.itemId);
                        break;
                    case ENPItemType.FIXED_CD:
                        NPPlayerFixedCDInfo fixedCdInfo  = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_shopRef.free_refresh_cd.itemId);
                        if (null != fixedCdInfo)
                        {
                            max = fixedCdInfo.getMaxCount();
                        }
                        break;
                }
                return max;
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        public void getShopItemList(List<NPPlayerShopItem> _list)
        {
            if (null == _list)
                return;
            foreach (NPPlayerShopItem shopItem in _m_shopItemList)
            {
                if(null == shopItem)
                    continue;
                if(null == shopItem.shopItemGruopRef || shopItem.shopItemGruopRef.show_cond.IsEnable(null))
                    _list.Add(shopItem);
            }
            // _list.AddRange(_m_shopItemList);
        }

        /// <summary>
        /// 获取当前付费刷新需要消耗的Item
        /// </summary>
        /// <returns></returns>
        public NPCommonCostItem getRefreshCostItem()
        {
            if (null == _m_shopRef)
                return null;

            TimesPriceRefObj costRef = GRefdataCoreMgr.instance.getTimesPriceRefObj(_m_shopRef.pay_refresh_times_price_id, _m_refreshNum + 1);
            if (null == costRef || null == costRef.cost_item_formula)
                return null;

            return new NPCommonCostItem(costRef.item, costRef.cost_item_formula.CalculateVariableResult(null));
        }

        /// <summary>
        /// 更新商店
        /// </summary>
        public void update(Shop_Info _info)
        {
            if (null == _info)
                return;

            _m_nextRefreshTimeMs = _info.getNextRefreshTimeMs();
            _m_refreshNum = _info.getRefreshNum();

            _m_shopItemList.Clear();
            if (null != _info.getGoodsList())
            {
                Shop_ItemInfo temp = null;
                for (int i = 0; i < _info.getGoodsList().Count; i++)
                {
                    temp = _info.getGoodsList()[i];
                    if (null == temp)
                        continue;

                    NPPlayerShopItem shopItem = new NPPlayerShopItem(temp,this);
                    _m_shopItemList.Add(shopItem);
                }
            }

            WinMsg.SendMsg(WinMsgType.SHOP_CHG, _m_shopRefId);
        }

        /// <summary>
        /// 更新商店中的商品
        /// </summary>
        public void updateItem(Shop_ItemInfo _item)
        {
            if (null == _item)
                return;

            NPPlayerShopItem temp = null;
            for (int i = 0; i < _m_shopItemList.Count; i++)
            {
                temp = _m_shopItemList[i];
                if (null == temp)
                    continue;

                if (temp.instanceId == _item.getInstanceId())
                {
                    temp.update(_item);
                    break;
                }
            }
        }

        /// <summary>
        /// 剩余恢复倒计时
        /// </summary>
        /// <returns></returns>
        public long getRemainMs()
        {
            if (null == freeRefreshCostItem)
                return 0;
            if (freeRefreshCostItem.itemType == ENPItemType.LAZY_CD)
            {
                return NPPlayer.instance.lazyCdComp.getRemainMs(freeRefreshCostItem.itemId);
            }
            if (freeRefreshCostItem.itemType == ENPItemType.FIXED_CD)
            {
                NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(freeRefreshCostItem.itemId);
                if (null == fixedCdInfo)
                    return 0;
                long remainTimeMs = fixedCdInfo.getNextCalcTimeTagMs() - FpsAndPingMgr.instance.serverTimeTag;
                if (remainTimeMs < 0)
                    remainTimeMs = 0;
                return remainTimeMs;
            }
            return 0;
        }
    }
}
