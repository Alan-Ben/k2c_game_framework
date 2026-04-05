using System.Collections.Generic;
using Common.ActivityObj;

namespace GOE
{
    /// <summary>
    /// 活动商店信息
    /// </summary>
    public class ActivityShopInfo
    {
        //活动商店id
        private long _m_lActivityShopId;
        //下一次自动刷新时间戳
        private long _m_lNextRefreshTimeMs;
        //活动商店配置
        private ActivityShopRefObj _m_activityShopRefObj;
        //活动商店道具列表 <活动商店id,活动商店信息>
        private Dictionary<long, ActivityShopItemInfo> _m_lActivityShopItemInfoList;

        /// <summary>
        /// 活动商店id
        /// </summary>
        public long activityShopId { get { return _m_lActivityShopId; } }
        /// <summary>
        /// 对应的活动货币id
        /// </summary>
        public long activityCurrencyId { get { return _m_activityShopRefObj != null ? _m_activityShopRefObj.activity_currency_id : 0; } }
        /// <summary>
        /// 活动商店配置
        /// </summary>
        public ActivityShopRefObj activityShopRefObj { get { return _m_activityShopRefObj; } }
        /// <summary>
        /// 下一次自动刷新时间戳
        /// </summary>
        public long nextRefreshTimeMs { get { return _m_lNextRefreshTimeMs; } }

        public ActivityShopInfo(long _activityInstanceId, long _activityShopId)
        {
            _m_lActivityShopId = _activityShopId;
            _m_lActivityShopItemInfoList = new Dictionary<long, ActivityShopItemInfo>();
            _m_activityShopRefObj = GRefdataCoreMgr.instance.activityShopRefCore.getRef(_activityShopId);

            //获取活动商店道具列表
            GRefdataCoreMgr.instance.activityShopItemRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.activity_shop_id == _m_lActivityShopId)
                    _m_lActivityShopItemInfoList[_ref.id] = new ActivityShopItemInfo(_activityInstanceId, _ref);
            });
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Activity_ShopInfo _info)
        {
            if(_info == null || _info.getShopId() != _m_lActivityShopId)
                return;

            _m_lNextRefreshTimeMs = _info.getNextRefreshTimeMs();

            //更新已购买数据
            if (_info.getBuyRecordList() != null && _m_lActivityShopItemInfoList != null)
            {
                for (int i = 0; i < _info.getBuyRecordList().Count; i++)
                {
                    Activity_ShopBuyRecord tempBuyRecord = _info.getBuyRecordList()[i];
                    if(tempBuyRecord == null)
                        continue;

                    if(_m_lActivityShopItemInfoList.TryGetValue(tempBuyRecord.getItemId(), out ActivityShopItemInfo shopItemInfo))
                        shopItemInfo?.updateBuyCount(tempBuyRecord);
                }
            }
        }

        /// <summary>
        /// 更新购买记录
        /// </summary>
        /// <param name="_buyRecord"></param>
        public void updateBuyRecord(Activity_ShopBuyRecord _buyRecord)
        {
            if (_buyRecord == null || _m_lActivityShopItemInfoList == null)
                return;

            if (_m_lActivityShopItemInfoList.TryGetValue(_buyRecord.getItemId(), out ActivityShopItemInfo shopItemInfo))
                shopItemInfo?.updateBuyCount(_buyRecord);
        }

        /// <summary>
        /// 获取活动商店道具列表
        /// </summary>
        /// <returns></returns>
        public void getActivityShopItemList(List<ActivityShopItemInfo> _list)
        {
            if (_list == null || _m_lActivityShopItemInfoList == null)
                return;

            _list.AddRange(_m_lActivityShopItemInfoList.Values);
        }

        /// <summary>
        /// 获取活动商店道具信息
        /// </summary>
        /// <param name="_activityShopItemId"></param>
        /// <returns></returns>
        public ActivityShopItemInfo getActivityShopItemInfo(long _activityShopItemId)
        {
            if (_m_lActivityShopItemInfoList == null)
                return null;

            if (_m_lActivityShopItemInfoList.TryGetValue(_activityShopItemId, out ActivityShopItemInfo itemInfo))
                return itemInfo;

            return null;
        }
    }
}