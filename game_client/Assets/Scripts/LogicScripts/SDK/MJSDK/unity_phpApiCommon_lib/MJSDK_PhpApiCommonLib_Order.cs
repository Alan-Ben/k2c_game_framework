using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- 订单相关
    /// </summary>
    public class MJSDK_PhpApiCommonLib_Order
    {
        //订单相关
        #region order

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="_user_token">用户登录令牌</param>
        /// <param name="_pay_type">支付类型</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        public static void order_getProduct(MJSDK_PhpApiCommon_2SDK_get_product _2SDK_Get_Product, Action<MJSDK_PhpApiCommon_Product_List> _sucDelegate, Action<int, string> _failDelegate)
        {

            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_Get_Product == null
                || string.IsNullOrEmpty(_2SDK_Get_Product.token)
                || string.IsNullOrEmpty(_2SDK_Get_Product.pay_type))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "order_getProduct --> Parameters are missing,token/pay_type is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("order", "getProduct", JsonUtility.ToJson(_2SDK_Get_Product), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Product_List>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 创建普通订单
        /// </summary>
        /// <param name="_productInfo">商品信息</param>
        /// <param name="_pay_type">支付类型</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void order_createOrder(MJSDK_PhpApiCommon_2SDK_create_order _productInfo, Action<MJSDK_PhpApiCommon_Order_Info> _sucDelegate, Action<int, string> _failDelegate)
        {

            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_productInfo == null
                || string.IsNullOrEmpty(_productInfo.token)
                || string.IsNullOrEmpty(_productInfo.callback_id)
                || string.IsNullOrEmpty(_productInfo.product_id)
                || string.IsNullOrEmpty(_productInfo.app_order_id)
                || string.IsNullOrEmpty(_productInfo.pay_type)
                || string.IsNullOrEmpty(_productInfo.role_id)
                || string.IsNullOrEmpty(_productInfo.server_id)
                || string.IsNullOrEmpty(_productInfo.platfrom_region))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "order_createOrder --> Parameters are missing,token/callback_id/product_id... is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("order", "createOrder", JsonUtility.ToJson(_productInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Order_Info>(_sucDelegate, _failDelegate));
        }

        #endregion


        #region orderVerify(订单验证)
        /// <summary>
        /// 苹果订单收据验证
        /// </summary>
        /// <param name="_appleOrderInfo">苹果订单信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_apple(MJSDK_PhpApiCommon_ApplePay_Purchases_Info _appleOrderInfo, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_appleOrderInfo == null
                || string.IsNullOrEmpty(_appleOrderInfo.order_id)
                || string.IsNullOrEmpty(_appleOrderInfo.receiptData)
                || string.IsNullOrEmpty(_appleOrderInfo.payment)
                || string.IsNullOrEmpty(_appleOrderInfo.payment_code))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_apple --> Parameters are missing,order_id/receiptData/payment/payment_code/payment/payment_code is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "apple", JsonUtility.ToJson(_appleOrderInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// google订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_google(MJSDK_PhpApiCommon_GooglePay_Purchases_Info _2SDK_OrderVerify_Google, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Google == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.json_data)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.signature)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.payment)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.payment_code)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_google --> Parameters are missing,json_data/signature/payment/payment_code/payment is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "google", JsonUtility.ToJson(_2SDK_OrderVerify_Google), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// huawei订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Huawei"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_huawei(MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info _2SDK_OrderVerify_Huawei, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Huawei == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Huawei.inAppDataSignature)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Huawei.inAppPurchaseData)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Huawei.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_huawei --> Parameters are missing,inAppDataSignature/inAppPurchaseData/orderid is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "huawei", JsonUtility.ToJson(_2SDK_OrderVerify_Huawei), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// xiaomi订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Xiaomi"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_xiaomi(MJSDK_PhpApiCommon_XiaomiPay_Purchases_Info _2SDK_OrderVerify_Xiaomi, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Xiaomi == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Xiaomi.purchaseToken)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Xiaomi.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_xiaomi --> Parameters are missing,purchaseToken/orderid is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "xiaomi", JsonUtility.ToJson(_2SDK_OrderVerify_Xiaomi), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// samsung订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Samsung"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_samsung(MJSDK_PhpApiCommon_SamsungPay_Purchases_Info _2SDK_OrderVerify_Samsung, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Samsung == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Samsung.purchaseID)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Samsung.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_samsung --> Parameters are missing,purchaseID/orderid is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "samsung", JsonUtility.ToJson(_2SDK_OrderVerify_Samsung), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// onestore订单收据验证,不进行服务端验证，服务端走对方服务端通知。客服端默认成功。
        /// </summary>
        /// <param name="_OneStorePay_Purchases_Info"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_onestore(MJSDK_PhpApiCommon_OneStorePay_Purchases_Info _OneStorePay_Purchases_Info, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }
            if (_sucDelegate != null)
            {
                MJSDK_PhpApiCommon_2Engine_OrderVerify mJSDK_PhpApiCommon_2Engine_OrderVerify = new MJSDK_PhpApiCommon_2Engine_OrderVerify();
                mJSDK_PhpApiCommon_2Engine_OrderVerify.order_id = _OneStorePay_Purchases_Info.order_id;
                mJSDK_PhpApiCommon_2Engine_OrderVerify.verify_result = "1";
                _sucDelegate(mJSDK_PhpApiCommon_2Engine_OrderVerify);
            }
        }

        /// <summary>
        /// amazon订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_amazon(MJSDK_PhpApiCommon_AmazonPay_Purchases_Info _2SDK_OrderVerify_Amazon, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Amazon == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Amazon.sku_id)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Amazon.user_id)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Amazon.receipt_id)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Amazon.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_amazon --> Parameters are missing,sku_id/user_id/receipt_id/order_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "amazon", JsonUtility.ToJson(_2SDK_OrderVerify_Amazon), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// ruStore订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_RuStore"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void orderVerify_rustore(MJSDK_PhpApiCommon_RuStorePay_Purchases_Info _2SDK_OrderVerify_RuStore, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_RuStore == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_RuStore.invoiceId)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_RuStore.purchaseToken)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_RuStore.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_rustore --> Parameters are missing,invoiceId/order_id/purchaseToken is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("orderVerify", "rustore", JsonUtility.ToJson(_2SDK_OrderVerify_RuStore), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        #endregion
    }
}
