using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 使用代金券购买确认弹窗节点
    /// </summary>
    public class GNodePayByVoucherConfirm : UIQueueBaseNode
    {
        //支付id
        private long _m_lPayId;
        //点击现金购买事件
        private Action _m_aOnClickPayByCash;
        //点击代金券购买事件
        private Action _m_aOnClickPayByVoucher;
        //取消事件
        private Action _m_aOnCancel;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //操作序列号
        private long _m_lSerialize;

        public GNodePayByVoucherConfirm(long _payId, Action _onClickPayByCash, Action _onClickPayByVoucher, Action _onCancel) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_PAY_BY_VOUCHER_CONFIRM)
        {
            _m_lPayId = _payId;
            _m_aOnClickPayByCash = _onClickPayByCash;
            _m_aOnClickPayByVoucher = _onClickPayByVoucher;
            _m_aOnCancel = _onCancel;
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }

        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            if (null != _m_wTransBk)
            {
                _m_wTransBk.showWnd();
                _afterTransBkAction();
            }
            else
            {
                _m_lSerialize = ALSerializeOpMgr.next();
                long serialize = _m_lSerialize;
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    GGUIWndPayByVoucherConfirm.instance,
                    () => { QueueMgr.instance.forceCloseNode(this); },
                    () =>
                    {
                        if (serialize != _m_lSerialize)
                            return;

                        _afterTransBkAction();
                    });
            }
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;

            //隐藏窗口
            GGUIWndPayByVoucherConfirm.instance.hideWnd();
        }

        public override void onEnterQueue()
        {
            GGUIWndPayByVoucherConfirm.instance.load();
        }

        public override void onClose()
        {
            //释放窗口
            GGUIWndPayByVoucherConfirm.instance.discard();
            _m_aOnCancel?.Invoke();
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            GGUIWndPayByVoucherConfirm.instance.regLoadDoneDelegate(() =>
            {
                //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(),
                        GGUIWndPayByVoucherConfirm.instance.getGameObj());
                else
                    //将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndPayByVoucherConfirm.instance.getGameObj());

                GGUIWndPayByVoucherConfirm.instance.showWnd();
                GGUIWndPayByVoucherConfirm.instance.setInfo(_m_lPayId, _dealClickPayByCash, _dealClickPayByVoucher,_dealClickCancel);
            });
        }

        // 点击现金购买处理
        private void _dealClickPayByCash()
        {
            _m_aOnCancel = null;
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PAY_BY_VOUCHER_CONFIRM);
            _m_aOnClickPayByCash?.Invoke();
            _m_aOnClickPayByCash = null;
        }

        // 点击代金券购买处理
        private void _dealClickPayByVoucher()
        {
            _m_aOnCancel = null;
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PAY_BY_VOUCHER_CONFIRM);
            _m_aOnClickPayByVoucher?.Invoke();
            _m_aOnClickPayByVoucher = null;
        }

        // 点击取消处理
        private void _dealClickCancel()
        {
            _m_aOnCancel?.Invoke();
            _m_aOnCancel = null;
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PAY_BY_VOUCHER_CONFIRM);
        }
    }
}
