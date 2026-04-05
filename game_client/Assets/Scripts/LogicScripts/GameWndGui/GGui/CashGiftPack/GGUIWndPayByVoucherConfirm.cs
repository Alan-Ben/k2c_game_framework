using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使用代金券购买确认弹窗
    /// </summary>
    public class GGUIWndPayByVoucherConfirm : _ANPGGUIBasicWnd<GGUIMonoPayByVoucherConfirm>
    {
        private static GGUIWndPayByVoucherConfirm _g_instance;
        public static GGUIWndPayByVoucherConfirm instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndPayByVoucherConfirm();
                return _g_instance;
            }
        }

        // 购买档位配置
        private PayRefObj _m_payRefObj;
        // 消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        // 点击现金购买事件
        private Action _m_aOnClickPayByCash;
        // 点击代金券购买事件
        private Action _m_aOnClickPayByVoucher;
        // 点击取消事件
        private Action _m_aOnClickCancel;

        public GGUIWndPayByVoucherConfirm() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPayByVoucherConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPayByVoucherConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onClickCancel);
            ALUGUICommon.uncombineBtnClick(wnd.btnPayByCash, _onClickPayByCash);
            ALUGUICommon.uncombineBtnClick(wnd.btnPayByVoucher, _onClickPayByVoucher);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onClickCancel);
            ALUGUICommon.combineBtnClick(wnd.btnPayByCash, _onClickPayByCash);
            ALUGUICommon.combineBtnClick(wnd.btnPayByVoucher, _onClickPayByVoucher);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_payId"></param>
        /// <param name="_onClickPayByCash"></param>
        /// <param name="_onClickPayByVoucher"></param>
        /// <param name="_onCancel"></param>
        public void setInfo(long _payId, Action _onClickPayByCash, Action _onClickPayByVoucher, Action _onCancel)
        {
            if(wnd == null)
                return;

            _m_aOnClickPayByCash = _onClickPayByCash;
            _m_aOnClickPayByVoucher = _onClickPayByVoucher;
            _m_aOnClickCancel = _onCancel;

            // 代金券道具
            _m_payRefObj = GRefdataCoreMgr.instance.payRefCore.getRef(_payId);
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(_m_payRefObj?.voucher_item);
            }

            // 现金价格
            ALUGUICommon.setLabelTxt(wnd.txtPrice, SDKMgr.instance.getLocalShowPrice(_payId));
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCancel(GameObject _go)
        {
            _m_aOnClickCancel?.Invoke();
        }

        /// <summary>
        /// 点击现金购买
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPayByCash(GameObject _go)
        {
            _m_aOnClickPayByCash?.Invoke();
        }

        /// <summary>
        /// 点击代金券购买
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPayByVoucher(GameObject _go)
        {
            if (_m_payRefObj == null)
                return;

            //代金券不足，判断是否开启网页支付，有开启的话直接去网页支付，没有开启的话提示代金券不足
            if (!GCommon.isItemEnough(_m_payRefObj.voucher_item, false))
            {
                if (GCommon.canUseWebRecharge())
                {
                    //有开启网页支付，直接去网页支付
                    //发送埋点-代金券不足，点击前往网页充值
                    GCommon.sendStepReport(TraceConst.PAY_GO_TO_WEB_RECHARGE);

                    //请求玩家透传参数
                    NPPlayer.instance.questionnaireComp.reqWebEncryptedData(_data =>
                    {
                        //网页地址
                        string address = GRefdataCoreMgr.instance.npGeneral.web_recharge_url;
                        //网页链接
                        string url = $"{address}?data={_data}";
                        //外部浏览器打开网页
                        GCommon.openURLByBrowser(url);
                    });

                    //关闭窗口
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PAY_BY_VOUCHER_CONFIRM);
                }
                else
                {
                    //没有开启网页支付，提示代金券不足
                    if(_m_payRefObj.voucher_item != null)
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_itemNotEnough_itemStr, GCommon.getItemName(_m_payRefObj.voucher_item.getItemType(), _m_payRefObj.voucher_item.subId)));
                }
            }
            else
            {
                //代金券足够，正常走代金券购买流程
                _m_aOnClickPayByVoucher?.Invoke();
            }
        }
    }
}