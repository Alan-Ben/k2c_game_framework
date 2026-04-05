using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- 订阅订单相关
    /// </summary>
    public class MJSDK_PhpApiCommonLib_SubOrder
    {
        /// <summary>
        /// 获取订阅商品信息
        /// </summary>
        /// <param name="_user_token">用户登录令牌</param>
        /// <param name="_pay_type">支付类型</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        public static void order_getSubProduct(MJSDK_PhpApiCommon_2SDK_get_product _2SDK_Get_Product, Action<MJSDK_PhpApiCommon_Product_List> _sucDelegate, Action<int, string> _failDelegate)
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
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "order_getSubProduct --> Parameters are missing,token/pay_type is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("order", "getSubProduct", JsonUtility.ToJson(_2SDK_Get_Product), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Product_List>(_sucDelegate, _failDelegate));
        }
        /// <summary>
        /// 创建普通订单
        /// </summary>
        /// <param name="_productInfo">商品信息</param>
        /// <param name="_pay_type">支付类型</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void order_createSubOrder(MJSDK_PhpApiCommon_2SDK_create_subOrder _productInfo, Action<MJSDK_PhpApiCommon_SubOrder_Info> _sucDelegate, Action<int, string> _failDelegate)
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
                || string.IsNullOrEmpty(_productInfo.server_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "order_createSubOrder --> Parameters are missing,token/callback_id/product_id... is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("order", "createSubOrder", JsonUtility.ToJson(_productInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_SubOrder_Info>(_sucDelegate, _failDelegate));
        }


        #region orderVerify(订单验证)
        /// <summary>
        /// 苹果订阅订单收据验证
        /// </summary>
        /// <param name="_appleOrderInfo">苹果订单信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void subOrderVerify_apple(MJSDK_PhpApiCommon_ApplePay_Purchases_Info _appleOrderInfo, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }
            //TODO: 需要验证的参数
            //必要参数不得为空
            if (_appleOrderInfo == null
                || string.IsNullOrEmpty(_appleOrderInfo.order_id)
                || string.IsNullOrEmpty(_appleOrderInfo.receiptData))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "orderVerify_apple --> Parameters are missing,order_id/receiptData/payment/payment_code/payment/payment_code is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("subOrderVerify", "apple", JsonUtility.ToJson(_appleOrderInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// google订阅订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void subOrderVerify_google(MJSDK_PhpApiCommon_GooglePay_Purchases_Info _2SDK_OrderVerify_Google, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Google == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.json_data)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.signature)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.sku_id)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.order_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "subOrderVerify_google --> Parameters are missing,json_data/signature/sku_id/payment is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("subOrderVerify", "google", JsonUtility.ToJson(_2SDK_OrderVerify_Google), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// google订阅订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void subOrderVerify_googleQuery(MJSDK_PhpApiCommon_GooglePay_Purchases_Info _2SDK_OrderVerify_Google, Action<MJSDK_PhpApiCommon_Pay_TransactionId> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_OrderVerify_Google == null
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.json_data)
                || string.IsNullOrEmpty(_2SDK_OrderVerify_Google.signature))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "subOrderVerify_googleQuery --> Parameters are missing,json_data/signature is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("subOrderVerify", "googleQuery", JsonUtility.ToJson(_2SDK_OrderVerify_Google), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// huawei订阅订单收据验证
        /// </summary>
        /// <param name="_2SDK_OrderVerify_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void subOrderVerify_huawei(MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info _2SDK_OrderVerify_Huawei, Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate, Action<int, string> _failDelegate)
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
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "subOrderVerify_huawei --> Parameters are missing,inAppDataSignature/inAppPurchaseData/orderid is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("subOrderVerify", "huawei", JsonUtility.ToJson(_2SDK_OrderVerify_Huawei), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }
        #endregion

        #region 订阅恢复校验
        /// <summary>
        /// 订阅是否可恢复校验
        /// </summary>
        /// <param name="_2SDK_Restored_Info">第三方渠道 支付--票据编号列表 消息结构体</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void order_verifyRestore(MJSDK_PhpApiCommon_Pay_TransactionId_List _pay_TransactionId_List, Action<MJSDK_PhpApiCommon_Pay_TransactionId_List> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_pay_TransactionId_List == null
                || _pay_TransactionId_List.sub_list == null
                || _pay_TransactionId_List.sub_list.Count == 0)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "order_verifyRestore --> Parameters are missing,sub_list is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("order", "verifyRestore", JsonUtility.ToJson(_pay_TransactionId_List), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId_List>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}