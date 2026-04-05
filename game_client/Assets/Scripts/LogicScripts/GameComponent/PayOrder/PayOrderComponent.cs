using System;
using ALPackage;
using CommonEnum;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 支付订单组件
    /// </summary>
    public class PayOrderComponent : _ANPBasicPlayerComponent
    {
        //当前正在进行中的订单
        private PayOrderInfo _m_curPayOrderInfo;

        //构造函数
        public PayOrderComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = {};
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PAY_ORDER; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        protected override void _dealInit()
        {
            setInitDone();
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PayOrderComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_curPayOrderInfo = null;
        }

        /// <summary>
        /// 设置支付订单完成
        /// </summary>
        /// <param name="_orderId"></param>
        public void setOrderPayDone(string _orderId)
        {
            if (_m_curPayOrderInfo == null || _m_curPayOrderInfo.orderId != _orderId)
            {
                //发送埋点，设置支付完成的订单id和当前订单id不一致
                GCommon.sendStepReport(TraceConst.PAY_SET_DONE_ORDERID_DIFFER.setMarkParam(_orderId, _m_curPayOrderInfo?.orderId));
                Debug.LogError($"【PayOrderComponent】setOrderPayDone：设置支付完成的订单id和当前订单id不一致，，setDoneOrderId:{_orderId}，curOrderId:{_m_curPayOrderInfo?.orderId}");
                return;
            }

            //设置支付完成
            _m_curPayOrderInfo.setOrderState(EOrderStatus.PAY_SUCCESS);
            reqClientPayDone(_orderId);
        }

        /// <summary>
        /// 设置支付订单取消
        /// </summary>
        /// <param name="_orderId"></param>
        public void setOrderPayCancel(string _orderId)
        {
            if (_m_curPayOrderInfo == null || _m_curPayOrderInfo.orderId != _orderId)
            {
                //发送埋点，设置支付取消的订单id和当前订单id不一致
                GCommon.sendStepReport(TraceConst.PAY_SET_CANCEL_ORDERID_DIFFER.setMarkParam(_orderId, _m_curPayOrderInfo?.orderId));
                Debug.LogError($"【PayOrderComponent】setOrderPayCancel：设置支付取消的订单id和当前订单id不一致，，setCancelOrderId:{_orderId}，curOrderId:{_m_curPayOrderInfo?.orderId}");
                return;
            }
            //设置支付取消
            _m_curPayOrderInfo.setOrderState(EOrderStatus.PAY_CANCELED);
            reqClientPayCancel(_orderId);
        }

        #region S2C

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="_msg"></param>
        public void onOrderAdd(GS2GC_004_066_OnOrderAdd _msg)
        {
            if (_msg == null || _msg.getOrderInfo() == null)
                return;

            //如果当前订单信息不为空，判断当前订单状态是否是未支付状态
            if (_m_curPayOrderInfo != null && _m_curPayOrderInfo.orderStatus == EOrderStatus.WAIT_PAY)
            {
                //发送埋点，上一个订单未完成就有新的订单了
                GCommon.sendStepReport(TraceConst.PAY_LAST_ORDER_NOT_FINISH.setMarkParam(_m_curPayOrderInfo.orderId, _msg.getOrderInfo().getOrderId()));
                Debug.LogError($"【PayOrderComponent】onOrderAdd：上一个订单未完成就有新的订单了，newOrderId:{_msg.getOrderInfo().getOrderId()}，curOrderId:{_m_curPayOrderInfo.orderId}");
            }

            //设置订单信息
            _m_curPayOrderInfo = new PayOrderInfo(_msg.getOrderInfo());

            //检查订单一段时间内是否完成，否则发送埋点
            string checkOrderId = _m_curPayOrderInfo.orderId;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (_m_curPayOrderInfo != null && 
                    _m_curPayOrderInfo.orderId == checkOrderId && 
                    _m_curPayOrderInfo.orderStatus == EOrderStatus.WAIT_PAY)
                {
                    //发送埋点，订单在一段时间内没有支付完成也没有取消支付，可能在付款界面停留过久
                    GCommon.sendStepReport(TraceConst.PAY_ORDER_TIMEOUT.setMarkParam(_m_curPayOrderInfo.orderId, checkOrderId));
                }
            },60f);
        }

        /// <summary>
        /// 订单变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onOrderChg(GS2GC_004_067_OnOrderChg _msg)
        {
            if (_msg == null || _msg.getOrderInfo() == null)
                return;

            //判断订单变更是否是当前订单
            if (_m_curPayOrderInfo != null && _m_curPayOrderInfo.orderId != _msg.getOrderInfo().getOrderId())
            {
                Debug.LogError($"订单变更不是当前订单，curOrderId:{_m_curPayOrderInfo.orderId}，chgOrderId:{_msg.getOrderInfo().getOrderId()}");
                return;
            }

            if(_m_curPayOrderInfo == null)
                _m_curPayOrderInfo = new PayOrderInfo(_msg.getOrderInfo());
            else
                _m_curPayOrderInfo.updateInfo(_msg.getOrderInfo());
        }


        #endregion

        #region C2S

        /// <summary>
        /// 请求创建支付订单
        /// </summary>
        /// <param name="_giftPackId"></param>
        /// <param name="_extraData"></param>
        /// <param name="_callback"></param>
        /// <param name="_dealErrorCode"></param>
        public void reqCreatePayOrder(long _giftPackId, byte[] _extraData, Action<bool, GS2GC_004_020_RetCreatePayOrder> _callback, Action<int> _dealErrorCode)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_020_ReqCreatePayOrder(_giftPackId, _extraData),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_020_RetCreatePayOrder>(_callback, _dealErrorCode));
        }

        /// <summary>
        /// 通知服务端支付完成
        /// </summary>
        /// <param name="_orderId"></param>
        /// <param name="_callback"></param>
        public void reqClientPayDone(string _orderId, Action<string> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_021_ReqClientPayDone(_orderId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_021_RetClientPayDone>(_msg =>
                {
                    _callback?.Invoke(_orderId);
                }));
        }

        /// <summary>
        /// 通知服务端支付取消
        /// </summary>
        /// <param name="_orderId"></param>
        /// <param name="_callback"></param>
        public void reqClientPayCancel(string _orderId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_026_ReqClientPayCancel(_orderId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_026_RetClientPayCancel>(_msg =>
                {
                    _callback?.Invoke();
                }));
        }

        #endregion
    }
}
