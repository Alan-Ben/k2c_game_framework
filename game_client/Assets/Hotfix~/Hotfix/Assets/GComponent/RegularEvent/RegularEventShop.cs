using GOE;
using System.Collections.Generic;
using Hotfix.Common.HotSimpleActivityObj;
using System.Collections;

namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店
    /// </summary>
    public class RegularEventShop
    {
        //下一次自动刷新时间戳
        private long _m_lNextRefreshTimeMs;
        //活动商店道具列表 <活动商店id,活动商店信息>
        private Dictionary<long, RegularEventShopItem> _m_lRegularEventShopItemList;
        //万能活动实例id
        private long _m_lActivityInstanceId;
        //是否正在请求刷新
        private bool _m_bIsRequsetingRefresh;

        /// <summary>
        /// 下一次自动刷新时间戳
        /// </summary>
        public long nextRefreshTimeMs { get { return _m_lNextRefreshTimeMs; } }

        public RegularEventShop(long _activityId, long _activityInstanceId, RegularActivity_ShopInfo _shopInfo)
        {
            _m_lActivityInstanceId = _activityInstanceId;
            _m_lRegularEventShopItemList = new Dictionary<long, RegularEventShopItem>();
            //获取活动商店道具列表
            HotfixRefdataCoreMgr.instance.regularEventShopItemRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.activity_id == _activityId)
                    _m_lRegularEventShopItemList[_ref.id] = new RegularEventShopItem(_activityInstanceId, _ref);
            });

            //更新数据
            updateInfo(_shopInfo);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(RegularActivity_ShopInfo _info)
        {
            if (_info == null)
                return;

            _m_lNextRefreshTimeMs = _info.getNextRefreshTimeMs();

            //更新已购买数据
            if (_info.getBuyRecordList() != null && _m_lRegularEventShopItemList != null)
            {
                for (int i = 0; i < _info.getBuyRecordList().Count; i++)
                {
                    RegularActivity_ShopBuyRecord tempBuyRecord = _info.getBuyRecordList()[i];
                    if (tempBuyRecord == null)
                        continue;

                    if (_m_lRegularEventShopItemList.TryGetValue(tempBuyRecord.getItemId(), out RegularEventShopItem shopItemInfo))
                        shopItemInfo?.updateBuyCount(tempBuyRecord);
                }
            }
        }

        /// <summary>
        /// 更新购买记录
        /// </summary>
        /// <param name="_buyRecord"></param>
        public void updateBuyRecord(RegularActivity_ShopBuyRecord _buyRecord)
        {
            if (_buyRecord == null || _m_lRegularEventShopItemList == null)
                return;

            if (_m_lRegularEventShopItemList.TryGetValue(_buyRecord.getItemId(), out RegularEventShopItem shopItemInfo))
                shopItemInfo?.updateBuyCount(_buyRecord);
        }

        /// <summary>
        /// 获取可购买万能活动商店道具列表
        /// </summary>
        /// <returns></returns>
        public void getCanBuyRegularEventShopItemList(List<RegularEventShopItem> _list)
        {
            if (_list == null || _m_lRegularEventShopItemList == null)
                return;

            foreach (RegularEventShopItem regularEventShopItem in _m_lRegularEventShopItemList.Values)
            {
                if(regularEventShopItem != null && regularEventShopItem.regularEventShopItemRef != null && regularEventShopItem.regularEventShopItemRef.can_direct_buy)
                    _list.Add(regularEventShopItem);
            }
        }

        /// <summary>
        /// 获取可免费购买次数
        /// </summary>
        /// <returns></returns>
        public long getFreeBuyCount()
        {
            long freeCount = 0;
            foreach (RegularEventShopItem regularEventShopItem in _m_lRegularEventShopItemList.Values)
            {
                if (regularEventShopItem != null && regularEventShopItem.regularEventShopItemRef != null && regularEventShopItem.regularEventShopItemRef.can_direct_buy && regularEventShopItem.isFree)
                    freeCount += regularEventShopItem.leftBuyCount;
            }

            return freeCount;
        }

        /// <summary>
        /// 检查是否刷新商店
        /// </summary>
        public void checkRefresh()
        {
            if (_m_bIsRequsetingRefresh)
                return;

            long serverTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            //如果已经到达自动刷新时间，请求刷新
            if (_m_lNextRefreshTimeMs > 0 && _m_lNextRefreshTimeMs <= serverTimeMs)
            {
                //设置正在请求刷新中
                _m_bIsRequsetingRefresh = true;
                //请求刷新
                HotfixNPPlayer.instance.regularEventComponent.reqRegularActivityShopRefresh(_m_lActivityInstanceId, () =>
                {
                    //设置请求完成
                    _m_bIsRequsetingRefresh = false;
                });
            }
        }

        /// <summary>
        /// 重置购买次数
        /// </summary>
        public void resetBuyCount()
        {
            if (_m_lRegularEventShopItemList == null)
                return;

            foreach (RegularEventShopItem regularEventShopItem in _m_lRegularEventShopItemList.Values)
            {
                regularEventShopItem?.updateBuyCount(0);
            }
        }
    }
}