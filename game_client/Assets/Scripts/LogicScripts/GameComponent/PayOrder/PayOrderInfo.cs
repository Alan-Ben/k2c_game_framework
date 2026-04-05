using Common.CommonFuncObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 支付订单信息
    /// </summary>
    public class PayOrderInfo
    {
        //订单号
        private string _m_sOrderId;
        //支付id
        private long _m_lPayId;
        //礼包id
        private long _m_lGiftPackId;
        //订单状态
        private CommonEnum.EOrderStatus _m_eOrderStatus;
        //创建时间 ms
        private long _m_lCreateTimeMs;
        //支付时间 ms
        private long _m_lPayTimeMs;


        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId { get { return _m_sOrderId; } }

        /// <summary>
        /// 支付id
        /// </summary>
        public long payId { get { return _m_lPayId; } }

        /// <summary>
        /// 礼包id
        /// </summary>
        public long giftPackId { get { return _m_lGiftPackId; } }

        /// <summary>
        /// 订单状态
        /// </summary>
        public CommonEnum.EOrderStatus orderStatus { get { return _m_eOrderStatus; } }

        /// <summary>
        /// 创建时间 ms
        /// </summary>
        public long createTimeMs { get { return _m_lCreateTimeMs; } }

        /// <summary>
        /// 支付时间 ms
        /// </summary>
        public long payTimeMs { get { return _m_lPayTimeMs; } }


        public PayOrderInfo(Order_Info _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        public void updateInfo(Order_Info _info)
        {
            if (_info == null)
                return;

            _m_sOrderId = _info.getOrderId();
            _m_lPayId = _info.getPayId();
            _m_lGiftPackId = _info.getGiftPackId();
            _m_eOrderStatus = _info.getStatus();
            _m_lCreateTimeMs = _info.getCreateTimeMs();
            _m_lPayTimeMs = _info.getPayTimeMs();

        }

        /// <summary>
        /// 设置订单状态
        /// </summary>
        /// <param name="_orderStatus"></param>
        public void setOrderState(EOrderStatus _orderStatus)
        {
            _m_eOrderStatus = _orderStatus;
        }
    }
}