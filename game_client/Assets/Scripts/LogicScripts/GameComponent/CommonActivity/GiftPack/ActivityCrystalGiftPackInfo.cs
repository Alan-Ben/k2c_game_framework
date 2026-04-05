using System.Collections.Generic;
using Common.CommonFuncObj;

namespace GOE
{
    /// <summary>
    /// 活动钻石礼包信息
    /// </summary>
    public class ActivityCrystalGiftPackInfo
    {
        //礼包组id
        private long _m_lGiftPackGroupId;
        //下一次自动刷新时间戳
        private long _m_lNextRefreshTimeMs;
        //礼包组配置数据
        private CrystalGiftPackGroupRefObj _m_groupRef;
        //活动商店道具列表 <钻石礼包Id,钻石礼包信息>
        private Dictionary<long, ActivityCrystalGiftPackItemInfo> _m_lCrystalGiftPackItemInfoList;

        /// <summary>
        /// 礼包组id
        /// </summary>
        public long giftPackGroupId { get { return _m_lGiftPackGroupId; } }
        /// <summary>
        /// 礼包组配置数据
        /// </summary>
        public CrystalGiftPackGroupRefObj giftPackGroupRef { get { return _m_groupRef; } }
        /// <summary>
        /// 下一次自动刷新时间戳
        /// </summary>
        public long nextRefreshTimeMs { get { return _m_lNextRefreshTimeMs; } }

        public ActivityCrystalGiftPackInfo(long _activityInstanceId, long _crystalGiftPackId)
        {
            _m_lGiftPackGroupId = _crystalGiftPackId;
            _m_groupRef = GRefdataCoreMgr.instance.crystalGiftPackGroupRefCore.getRef(_m_lGiftPackGroupId);
            _m_lCrystalGiftPackItemInfoList = new Dictionary<long, ActivityCrystalGiftPackItemInfo>();

            //获取活动商店道具列表
            GRefdataCoreMgr.instance.crystalGiftPackRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.crystal_gift_pack_group_id == _m_lGiftPackGroupId)
                    _m_lCrystalGiftPackItemInfoList[_ref.id] = new ActivityCrystalGiftPackItemInfo(_activityInstanceId, _ref);
            });
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(CrystalGiftPack_Info _info)
        {
            if(_info == null || _info.getGroupId() != _m_lGiftPackGroupId)
                return;

            _m_lNextRefreshTimeMs = _info.getNextRefreshTimeMs();

            //更新已购买数据
            if (_info.getBuyRecordList() != null && _m_lCrystalGiftPackItemInfoList != null)
            {
                for (int i = 0; i < _info.getBuyRecordList().Count; i++)
                {
                    CrystalGiftPack_BuyRecord tempBuyRecord = _info.getBuyRecordList()[i];
                    if(tempBuyRecord == null)
                        continue;

                    if(_m_lCrystalGiftPackItemInfoList.TryGetValue(tempBuyRecord.getGiftPackId(), out ActivityCrystalGiftPackItemInfo giftPackInfo))
                        giftPackInfo?.updateBuyCount(tempBuyRecord);
                }
            }
        }

        /// <summary>
        /// 更新购买记录
        /// </summary>
        /// <param name="_buyRecord"></param>
        public void updateBuyRecord(CrystalGiftPack_BuyRecord _buyRecord)
        {
            if (_buyRecord == null || _m_lCrystalGiftPackItemInfoList == null)
                return;

            if (_m_lCrystalGiftPackItemInfoList.TryGetValue(_buyRecord.getGiftPackId(), out ActivityCrystalGiftPackItemInfo shopItemInfo))
                shopItemInfo?.updateBuyCount(_buyRecord);
        }

        /// <summary>
        /// 获取钻石礼包列表
        /// </summary>
        /// <returns></returns>
        public void getCrystalGiftPackList(List<ActivityCrystalGiftPackItemInfo> _list)
        {
            if (_list == null || _m_lCrystalGiftPackItemInfoList == null)
                return;

            _list.AddRange(_m_lCrystalGiftPackItemInfoList.Values);
        }

        /// <summary>
        /// 获取钻石礼包信息
        /// </summary>
        /// <param name="_crystalGiftPackId"></param>
        /// <returns></returns>
        public ActivityCrystalGiftPackItemInfo getCrystalGiftPackItemInfo(long _crystalGiftPackId)
        {
            if (_m_lCrystalGiftPackItemInfoList == null)
                return null;

            if (_m_lCrystalGiftPackItemInfoList.TryGetValue(_crystalGiftPackId, out ActivityCrystalGiftPackItemInfo itemInfo))
                return itemInfo;

            return null;
        }

        /// <summary>
        /// 是否有可以购买的免费礼包
        /// </summary>
        /// <returns></returns>
        public bool haveFreeGiftPackCanBuy()
        {
            foreach (KeyValuePair<long, ActivityCrystalGiftPackItemInfo> keyValuePair in _m_lCrystalGiftPackItemInfoList)
            {
                if (keyValuePair.Value != null && keyValuePair.Value.isFree && !keyValuePair.Value.isSellOut)
                    return true;
            }

            return false;
        }
    }
}