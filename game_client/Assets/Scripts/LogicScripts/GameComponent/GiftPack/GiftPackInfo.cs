using Common.CommonFuncObj;

namespace GOE
{
    /// <summary>
    /// 礼包信息
    /// </summary>
    public class GiftPackInfo
    {
        //礼包id
        private long _m_lGiftPackId;
        //礼包配置
        GiftPackRefObj _m_giftPackRefObj;
        //下次刷新时间
        private long _m_lNextRefreshTimeMs;
        //已购买数量
        private int _m_iHadBuyCount;
        //是否正在刷新信息中
        private bool _m_bIsRefreshing;

        /// <summary>
        /// 礼包id
        /// </summary>
        public long giftPackId { get { return _m_lGiftPackId; } }
        /// <summary>
        /// 礼包配置
        /// </summary>
        public GiftPackRefObj giftPackRef { get { return _m_giftPackRefObj; } }
        /// <summary>
        /// 下次刷新时间
        /// </summary>
        public long nextRefreshTimeMs { get { return _m_lNextRefreshTimeMs; } }
        /// <summary>
        /// 已购买数量
        /// </summary>
        public int hadBuyCount { get { return _m_iHadBuyCount; } }
        /// <summary>
        /// 剩余购买次数
        /// </summary>
        public int leftBuyCount
        {
            get
            {
                if(_m_giftPackRefObj == null)
                    return 0;

                //没有限制购买次数的情况默认返回999
                return _m_giftPackRefObj.isNotLimit ? NPConst.GIFT_PACK_NOT_LIMIT_COUNT : _m_giftPackRefObj.buy_limit_count - _m_iHadBuyCount;
            }
        }
        /// <summary>
        /// 是否正在刷新信息中
        /// </summary>
        public bool isRefreshing { get { return _m_bIsRefreshing; } set { _m_bIsRefreshing = value; } }

        public GiftPackInfo(GiftPack_Info _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        public void updateInfo(GiftPack_Info _info)
        {
            if (_info == null)
                return;

            _m_bIsRefreshing = false;
            _m_lGiftPackId = _info.getGiftPackId();
            _m_giftPackRefObj = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_lGiftPackId);
            _m_lNextRefreshTimeMs = _info.getNextRefreshTimeMs();
            _m_iHadBuyCount = _info.getHadBuyCount();
        }
    }
}