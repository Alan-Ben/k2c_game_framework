using ALPackage;
using CommonEnum;
using GOE;
using LitJson;
using MJSDK_Package;
using NPEnum;
using System;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;

namespace GOESDK
{
    /**
     * 支付流程WiKi：https://thoughts.teambition.com/workspaces/6763d4938bcec10018543689/docs/689d3fbbe41af20001af015f
     */

    /// <summary>
    /// 充值相关方法
    /// </summary>
    public abstract partial class _ASDKMgr
    {
        //屏蔽所有输入的序号
        private int _m_iPayMaskSerialize;
        //请求支付序列号
        private long _m_lReqPaySerialize;
        //支付成功回调
        private Action _m_aOnPaySuc;
        //支付失败回调<错误码，错误描述>
        private Action<int, string> _m_aOnPayFail;

        #region 取消支付相关错误码

        //苹果取消支付的错误码
        private const int APPLE_PAY_CANCEL_CODE = 50106;
        //谷歌取消支付的错误码
        private const int GOOGLE_PAY_CANCEL_CODE = 50207;
        //华为取消支付的错误码
        private const int HUAWEI_PAY_CANCEL_CODE = 50303;
        //小米取消支付的错误码
        private const int XIAOMI_PAY_CANCEL_CODE = 50403;
        //三星取消支付的错误码
        private const int SAMSUNG_PAY_CANCEL_CODE = 50503;
        //OneStore取消支付的错误码
        private const int ONESTORE_PAY_CANCEL_CODE = 50603;
        //RuStore取消支付的错误码
        private const int RUSTORE_PAY_CANCEL_CODE = 50904;

        #endregion


        /// <summary>
        /// 请求支付
        /// </summary>
        /// <param name="_payRefId">支付档位表id</param>
        /// <param name="_giftPackId">不同商品配表id</param>
        /// <param name="_goodsName">商品名称</param>
        /// <param name="_sucDelegate">成功回调</param>
        /// <param name="_failDelegate">失败回调</param>
        public void reqPay(long _payRefId, long _giftPackId, string _goodsName, Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //获取支付档位配置信息
            PayRefObj payRef = GRefdataCoreMgr.instance.payRefCore.getRef(_payRefId);

            //发送埋点-点击支付
            GCommon.sendStepReport(TraceConst.PAY_CLICK.setMarkParam(_payRefId, payRef?.sdk_pay_id, _giftPackId));

            //======================================================================
            //====================如果没有使用SDK，则使用GM命令支付====================
            //======================================================================
            if (!isUseSDK)
            {
                _dealGMPay(_payRefId, _giftPackId, _sucDelegate, _failDelegate);
                return;
            }
            //======================================================================

            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("reqPay", _failDelegate))
                return;

            //检查支付档位id是否有效
            if (payRef == null)
            {
                SDKUtil.showSDKLogError($"[reqPay] 未获取到对应的支付档位信息，payRefId：{_payRefId}");
                _failDelegate?.Invoke(SDKMgr.instance.COMMON_ERROR, $"未获取到对应的支付档位信息，payRefId：{_payRefId}");
                return;
            }

            //游戏订单号
            string orderId = null;
            //支付成功回调
            _m_aOnPaySuc = _sucDelegate;
            //支付失败回调 
            _m_aOnPayFail = _failDelegate;
            //取消屏蔽输入
            if(_m_iPayMaskSerialize > 0)
                MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
            //屏蔽所有输入
            _m_iPayMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            //增加本次请求支付序列号
            _m_lReqPaySerialize = ALSerializeOpMgr.next();
            long curReqPaySerialize = _m_lReqPaySerialize;
            //是否使用了代金券
            bool isUseVoucher = false;

            //创建处理流程对象
            ALProcess payProcessObj = ALProcess.CreateProcess("reqPay");
            payProcessObj
                //1、先检查token是否过期
                .addDelegateProcess(_onDone =>
                { 
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;
                    _checkTokenExpire(_onDone, (_code,_msg)=> { _dealPayFail(null, _giftPackId, _code, _msg); });
                })
                //2、向服务端请求游戏订单号
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;
                    _reqOrderIdProcess(_giftPackId, _orderId => { orderId = _orderId; _onDone?.Invoke(); }, _dealPayFail);
                })
                //3、检查是否使用代金券支付
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    // 检查是否可以弹窗使用代金券
                    _checkPayByVoucher(_payRefId, orderId, _onDone, ()=>
                    {
                        isUseVoucher = true;
                        _onDone?.Invoke();
                    }, (_code, _msg, _needFailTip) => { _dealPayFail(orderId, _giftPackId, _code, _msg, _needFailTip); });
                })
                //4、处理SDK支付
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    //已使用代金券，直接跳过
                    if (isUseVoucher)
                    {
                        _onDone?.Invoke();
                        return;
                    }

                    _reqSDKPayProcess(orderId, payRef.sdk_pay_id, _giftPackId, _goodsName, _onDone, (_code, _msg) => { _dealPayFail(orderId, _giftPackId, _code, _msg); });
                })
                //5、成功回调
                .addProcess(()=>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;
                    _dealPaySuc(orderId, _giftPackId);
                })
                .deal();
        }

        /// <summary>
        /// 向服务端请求游戏订单号
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _reqOrderIdProcess(long _giftPackId, Action<string> _sucDelegate, Action<string, long, int, string, bool> _failDelegate)
        {
            //发送埋点-开始创建支付订单
            GCommon.sendStepReport(TraceConst.PAY_CREATE_ORDERID_START);

            //向服务端请求游戏订单号
            NPPlayer.instance.payOrderComp.reqCreatePayOrder(_giftPackId, null, (_isSuc, _msg) =>
            {
                if (_isSuc)
                {
                    //发送埋点-支付订单创建成功
                    GCommon.sendStepReport(TraceConst.PAY_CREATE_ORDERID_SUC.setMarkParam(_msg.getOrderId(), _giftPackId));

                    _sucDelegate?.Invoke(_msg.getOrderId());
                }
                else
                    _failDelegate?.Invoke(null, _giftPackId, COMMON_ERROR, $"向服务端请求订单号失败, _giftPackId:{_giftPackId}", false);
            }, _errorCode =>
            {
                //发送埋点-支付订单创建失败
                GCommon.sendStepReport(TraceConst.PAY_CREATE_ORDERID_FAIL.setMarkParam(_errorCode, _giftPackId));
            });
        }

        /// <summary>
        /// 检查是否有代金券，有的话弹窗确认是否使用
        /// </summary>
        /// <param name="_payRefId"></param>
        /// <param name="_orderId"></param>
        /// <param name="_dealNoUseVoucher"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _checkPayByVoucher(long _payRefId, string _orderId, Action _dealNoUseVoucher, Action _sucDelegate, Action<int, string, bool> _failDelegate)
        {
            //发送埋点-检查是否拥有代金券
            GCommon.sendStepReport(TraceConst.PAY_CHECK_VOUCHER);

            //获取支付档位配置信息
            PayRefObj payRef = GRefdataCoreMgr.instance.payRefCore.getRef(_payRefId);

            //检查支付档位id是否有效
            if (payRef == null)
            {
                SDKUtil.showSDKLogError($"[_checkPayByVoucher] 未获取到对应的支付档位信息，payRefId：{_payRefId}");
                _failDelegate?.Invoke(COMMON_ERROR, $"未获取到对应的支付档位信息，payRefId：{_payRefId}", true);
                return;
            }

            //是否可以使用网页支付
            bool canUseWebRecharge = GCommon.canUseWebRecharge();
            //是否至少有1个代金券
            bool haveVoucher = payRef.voucher_item != null &&
                              payRef.voucher_item.getItemType() != ENPItemType.NONE &&
                              GCommon.isItemEnough(payRef.voucher_item.getItemType(), payRef.voucher_item.subId, 1, false);

            //弹窗确认使用代金券的条件：拥有代金券或者开启了网页支付
            if (!canUseWebRecharge && !haveVoucher)
            {
                //发送埋点-没有代金券也没有开启网页支付，进入支付流程
                GCommon.sendStepReport(TraceConst.PAY_NO_VOUCHER);

                //没有使用代金券回调
                _dealNoUseVoucher?.Invoke();
            }
            else
            {
                //发送埋点-拥有代金券或者开启了网页支付，弹窗确认是否使用代金券
                GCommon.sendStepReport(TraceConst.PAY_HAVE_VOUCHER);
                //取消输入屏蔽
                MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
                //有代金券，弹窗确认是否使用代金券
                QueueMgr.instance.AddNode(new GNodePayByVoucherConfirm(_payRefId, () =>
                {
                    //点击现金支付
                    //发送埋点-不使用代金券，使用现金支付
                    GCommon.sendStepReport(TraceConst.PAY_NO_USE_VOUCHER);

                    //没有使用代金券回调
                    _dealNoUseVoucher?.Invoke();
                }, ()=>
                {
                    //点击代金券支付
                    //发送埋点-拥有代金券并且使用代金券
                    GCommon.sendStepReport(TraceConst.PAY_START_USE_VOUCHER);

                    //使用代金券
                    _m_iPayMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                    NPGSClientListener.sendRequestByLog(new GC2GS_004_027_ReqOrderVoucherPay(_orderId),
                        new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_027_RetOrderVoucherPay>((_isSuc, _msg) =>
                        {
                            if (_isSuc)
                            {
                                //发送埋点-使用代金券成功
                                GCommon.sendStepReport(TraceConst.PAY_USE_VOUCHER_SUC);
                                _sucDelegate?.Invoke();
                            }
                            else
                            {
                                //发送埋点-使用代金券失败
                                GCommon.sendStepReport(TraceConst.PAY_USE_VOUCHER_FAIL);
                                SDKUtil.showSDKLogError($"[_checkPayByVoucher] 使用代金券支付失败，payRefId：{_payRefId}");
                                _failDelegate?.Invoke(COMMON_ERROR, $"使用代金券支付失败，payRefId：{_payRefId}", false);
                            }
                        }));
                }, () =>
                {
                    //点击取消
                    _failDelegate?.Invoke(GOOGLE_PAY_CANCEL_CODE, "取消支付", true);
                }));
            }
        }

        /// <summary>
        /// 处理SDK支付
        /// </summary>
        /// <param name="_orderId">订单id</param>
        /// <param name="_sdkPayId">后台商品支付id</param>
        /// <param name="_sucDelegate">成功回调</param>
        /// <param name="_failDelegate">失败回调</param>
        private void _reqSDKPayProcess(string _orderId, string _sdkPayId, long _giftPackRefId, string _goodsName, Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送埋点-开始调用SDK支付
            GCommon.sendStepReport(TraceConst.PAY_SDK_START);

            //扩展参数
            JsonData extensionData = new JsonData();

            //创建支付信息
            MJSDK_PhpApiCommon_2SDK_create_order createOrder = new MJSDK_PhpApiCommon_2SDK_create_order();
            createOrder.token = SDKLoginSetting.instance.getTokenData()?.token;//用户登录成功token令牌
            createOrder.platfrom_region = GCommon.getPlatfromRegion().ToString();//区域标识 ID，平台区域ID(平台ID×100+区域ID)
            createOrder.server_id = GameInit_SelectServer.instance.loginServerLogicId.ToString();//服务器id
            createOrder.role_id = NPPlayer.instance.playerInfo.CID.ToString();//角色cid
            createOrder.extension = JsonMapper.ToJson(extensionData); //扩展参数,原样通知到通知地址
            createOrder.pay_type = _getPayTypeStr();//支付类型, 微信：wechat , 支付宝：alipay , 苹果：apple , 谷歌：google ,华为：huaweipay
            createOrder.product_id = _giftPackRefId.ToString(); //充值产品ID（项目组自己的）
            createOrder.app_order_id = _orderId; //游戏订单号（项目组自己的）
            createOrder.product_name = _goodsName; //产品名称（项目组自己的）
            createOrder.sdk_pay_id = _sdkPayId; //档位id
            createOrder.callback_id = CDNSetting_LoginServerUrlInfo.instance.data?.config?.callback_id; //回调地址ID
            createOrder.callback_url = ""; //付款成功后通知地址,如果Callback_id 有填入,callback_url可不用填

            //请求SDK支付
            MJSDK_Mgr_Pay.req_pay(_getPayLibType(), createOrder, _orderVerify =>
            {
                //返回 1：成功 、0：失败
                if (_orderVerify != null && _orderVerify.verify_result == "1")
                    _sucDelegate?.Invoke();
                else
                    _failDelegate?.Invoke(COMMON_ERROR, $"支付订单验证失败，payLibType:{_getPayLibType()}，pay_type:{_getPayTypeStr()}，verify_result：{_orderVerify?.verify_result}，order_id:{_orderVerify?.order_id}");
            }, (_code,_msg)=> { _failDelegate?.Invoke(_code, _msg); });
        }

        /// <summary>
        /// 处理GM命令支付
        /// </summary>
        private void _dealGMPay(long _payRefId, long _giftPackId, Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //游戏订单号
            string orderId = null;
            //支付成功回调
            _m_aOnPaySuc = _sucDelegate;
            //支付失败回调 
            _m_aOnPayFail = _failDelegate;
            //增加本次请求支付序列号
            _m_lReqPaySerialize = ALSerializeOpMgr.next();
            long curReqPaySerialize = _m_lReqPaySerialize;
            //是否使用代金券
            bool isUseVoucher = false;

            //取消屏蔽输入
            if (_m_iPayMaskSerialize > 0)
                MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
            //屏蔽所有输入
            _m_iPayMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();

            //GM命令支付逻辑
            ALProcess payProcessObj = ALProcess.CreateProcess("reqGMPay");
            payProcessObj
                //1、向服务端请求游戏订单号
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    _reqOrderIdProcess(_giftPackId, _orderId => { orderId = _orderId; _onDone?.Invoke(); }, _dealPayFail);
                })
                //2、检查是否使用代金券支付
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    //检查是否可以弹窗使用代金券
                    _checkPayByVoucher(_payRefId, orderId, _onDone, ()=> { isUseVoucher = true; _onDone?.Invoke(); }, (_code, _msg, _needFailTip) => { _dealPayFail(orderId, _giftPackId, _code, _msg, _needFailTip); });
                })
                //3、弹窗确认GM支付
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    //已使用代金券，直接跳过
                    if (isUseVoucher)
                    {
                        _onDone?.Invoke();
                        return;
                    }

                    //取消屏蔽输入
                    MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
                    //弹窗确认支付
                    QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_GM_PAY);
                    NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.sdk_gm_test_purchase_str, _giftPackId),
                        TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                        () =>
                        {
                            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_GM_PAY);
                            _dealPayFail(orderId, _giftPackId, GOOGLE_PAY_CANCEL_CODE, "取消支付");
                        }, TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                        () =>
                        {
                            QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_GM_PAY);
                            //屏蔽所有输入
                            _m_iPayMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                            _onDone?.Invoke();
                        },
                        true,
                        "##仅非SDK包提示##");

                })
                //4、检查GM命令权限
                .addDelegateProcess(_onDone =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    //已使用代金券，直接跳过
                    if (isUseVoucher)
                    {
                        _onDone?.Invoke();
                        return;
                    }

                    //检查是否有GM命令权限
                    CheatMgr.instance.reqGmCommand($"player showearningsspeed", (_str) =>
                    {
                        _onDone?.Invoke();
                    }, () =>
                    {
                        //处理失败
                        _dealPayFail(orderId, _giftPackId, COMMON_ERROR, "没有GM命令权限");
                        //没有GM权限上浮提示
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo("##您没有使用GM权限，请联系管理员进行分配##");
                    });
                })
                //5、成功回调
                .addProcess(() =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    _dealPaySuc(orderId, _giftPackId);
                })
                //6、处理GM命令消耗订单完成支付
                .addProcess(() =>
                {
                    if (!_checkIsSamePayReq(curReqPaySerialize))
                        return;

                    //已使用代金券，直接跳过
                    if (isUseVoucher)
                        return;

                    CheatMgr.instance.reqGmCommand($"order setpay {orderId}");
                })
                .deal();
        }

        /// <summary>
        /// 检查支付是否取消
        /// </summary>
        /// <param name="_code"></param>
        private bool _checkIsPayCancel(int _code)
        {
            return _code == GOOGLE_PAY_CANCEL_CODE || 
                   _code == APPLE_PAY_CANCEL_CODE || 
                   _code == HUAWEI_PAY_CANCEL_CODE || 
                   _code == XIAOMI_PAY_CANCEL_CODE || 
                   _code == SAMSUNG_PAY_CANCEL_CODE || 
                   _code == ONESTORE_PAY_CANCEL_CODE || 
                   _code == RUSTORE_PAY_CANCEL_CODE;
        }

        /// <summary>
        /// 检查当前请求序列号是否和正在进行的支付请求序列号一致
        /// </summary>
        /// <param name="_curReqPaySerialize"></param>
        /// <returns></returns>
        private bool _checkIsSamePayReq(long _curReqPaySerialize)
        {
            if (_curReqPaySerialize != _m_lReqPaySerialize)
            {
                //发送埋点，当前支付请求序列号和正在进行的支付请求序列号不一致
                GCommon.sendStepReport(TraceConst.PAY_REQUEST_SERIALIZE_DIFFER);
                SDKUtil.showSDKLogError($"[reqPay] 当前支付请求序列号和正在进行的支付请求序列号不一致");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 处理支付成功
        /// </summary>
        private void _dealPaySuc(string _orderId, long _giftPackId)
        {
            //发送埋点-支付成功
            GCommon.sendStepReport(TraceConst.PAY_SUCCEED.setMarkParam(_orderId, _giftPackId));
            
            //支付成功日志
            SDKUtil.showSDKDebugLog($"[reqPay] 支付成功，orderId:{_orderId}, giftPackId:{_giftPackId}");

            //取消屏蔽输入
            MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
            _m_iPayMaskSerialize = 0;

            //设置订单支付完成，并通知服务器
            NPPlayer.instance.payOrderComp.setOrderPayDone(_orderId);

            //执行成功回调
            Action onPaySuc = _m_aOnPaySuc;
            _m_aOnPaySuc = null;
            onPaySuc?.Invoke();
        }

        /// <summary>
        /// 处理支付失败
        /// </summary>
        private void _dealPayFail(string _orderId, long _giftPackId, int _code, string _msg, bool _needFailTip = true)
        {
            //取消屏蔽输入
            MainCameraMono.selfInstance.closeAllInputMask(_m_iPayMaskSerialize);
            _m_iPayMaskSerialize = 0;

            //检查是否是支付取消
            bool isPayCancel = _checkIsPayCancel(_code);
            if (isPayCancel)
            {
                //发送埋点-支付取消
                GCommon.sendStepReport(TraceConst.PAY_CANCEL.setMarkParam(_orderId, _giftPackId, _code, _msg));
                //上浮提示
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.pay_cancel_none);
                //支付取消日志
                SDKUtil.showSDKDebugLog($"[reqPay] 支付取消，orderId:{_orderId}, giftPackId:{_giftPackId}，code:{_code}, msg:{_msg}");
            }
            else
            {
                //发送埋点-支付失败
                GCommon.sendStepReport(TraceConst.PAY_FAILED.setMarkParam(_orderId, _giftPackId, _code, _msg));
                //上浮提示，这里不需要上浮提示会有服务器错误码的上浮提示
                if(_needFailTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.pay_failed_num, _code));
                //支付失败日志
                SDKUtil.showSDKLogError($"[reqPay] 支付失败，orderId:{_orderId}, giftPackId:{_giftPackId}，code:{_code}, msg:{_msg}");
            }

            //设置订单支付取消，并通知服务器
            if(!string.IsNullOrEmpty(_orderId))
                NPPlayer.instance.payOrderComp.setOrderPayCancel(_orderId);

            //执行失败回调
            Action<int, string> onPayFail = _m_aOnPayFail;
            _m_aOnPayFail = null;
            onPayFail?.Invoke(_code, _msg);
        }


        #region =============================================通用方法=============================================

        /// <summary>
        /// 检查token是否过期
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _checkTokenExpire(Action _sucDelegate, Action<int, string> _failDelegate)
        {
            //是否有缓存token
            bool haveToken = SDKLoginSetting.instance.hasToken();
            if (haveToken)
            {
                //获取缓存的token
                SDKTokenData tokenData = SDKLoginSetting.instance.getTokenData();
                //检查token是否过期
                MJSDK_PhpApiCommon_2SDK_user_tokenVerfity tokenVerfityInfo = new MJSDK_PhpApiCommon_2SDK_user_tokenVerfity();
                tokenVerfityInfo.token = tokenData.token;
                tokenVerfityInfo.user_id = tokenData.user_id;
                MJSDK_PhpApiCommonLib_UserHandle.user_tokenVerify(tokenVerfityInfo, _result =>
                {
                    //返回1是正常
                    if (_result == "1")
                    {
                        _sucDelegate?.Invoke();
                    }
                    else
                    {
                        //token过期，清除缓存，重新登录获取token
                        SDKLoginSetting.instance.clearToken();
                        SDKMgr.instance.quickLogin((_sdkTokenData) =>
                        {
                            _sucDelegate?.Invoke();
                        }, _failDelegate);
                    }
                }, _failDelegate);
            }
            else
            {
                //没有缓存token，直接登录获取token
                SDKMgr.instance.quickLogin((_sdkTokenData) =>
                {
                    _sucDelegate?.Invoke();
                }, _failDelegate);
            }
        }

        /// <summary>
        /// 获取支付第三方渠道库类型
        /// </summary>
        /// <returns></returns>
        private E_MJSDK_PayType _getPayLibType()
        {
            //如果是苹果，直接返回苹果类型
#if UNITY_IOS
            return E_MJSDK_PayType.apple;
#endif

            //TODO 根据平台类型设置支付第三方渠道库类型
            switch (MainCameraMono.selfInstance.platType )
            {
                case EWCGPlatType.MJ_GOOGLE_TEST_RU:
                case EWCGPlatType.MJ_GOOGLE_TEST_US:
                    return E_MJSDK_PayType.google;
            }

            return E_MJSDK_PayType.google;
        }

        /// <summary>
        /// 获取支付类型
        /// </summary>
        /// <returns></returns>
        private string _getPayTypeStr()
        {
            // 支付类型
            // 微信：wechat , 支付宝：alipay , 苹果：apple , 
            // 谷歌：google ,华为：huaweipay ,小米：mipay ,
            // 三星：samsung, rustore：rustore, onestore：onestore, amazon：amazon

            //如果是苹果，直接返回苹果类型
#if UNITY_IOS
            return "apple";
#endif

            //TODO 根据平台类型设置支付类型
            switch (MainCameraMono.selfInstance.platType )
            {
                case EWCGPlatType.MJ_GOOGLE_TEST_RU:
                case EWCGPlatType.MJ_GOOGLE_TEST_US:
                    return "google";
            }

            return "google";
        }

#endregion
    }
}
