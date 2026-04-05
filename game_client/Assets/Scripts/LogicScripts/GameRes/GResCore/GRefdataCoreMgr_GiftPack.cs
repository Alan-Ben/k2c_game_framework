namespace GOE
{
    /// <summary>
    /// 礼包相关
    /// </summary>
    public partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 获取礼包额外奖励配置
        /// </summary>
        /// <param name="_giftPackId">礼包id</param>
        /// <param name="_buyTimes">购买次数</param>
        /// <returns></returns>
        public GiftPackExtraGainRefObj getGiftPackExtraGainRef(long _giftPackId, long _buyTimes)
        {
            GiftPackExtraGainRefObj targetRef = null;
            giftPackExtraGainRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.gift_pack_id == _giftPackId && _ref.buy_times_range != null && _ref.buy_times_range.inRange(_buyTimes))
                    targetRef = _ref;
            });
            return targetRef;
        }
    }
}