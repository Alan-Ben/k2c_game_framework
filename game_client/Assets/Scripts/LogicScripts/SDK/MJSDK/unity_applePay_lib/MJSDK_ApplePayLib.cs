using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MJSDK_Package;


namespace MJSDK_Package
{
    /// <summary>
    /// 苹果支付组件
    /// </summary>
    public class MJSDK_ApplePayLib
    {

        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_ApplePay";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 3, 11, 0);
        //初始化判断
        private static bool _g_isInit = false;
        //组件库初始化状态获取
        public static bool g_isInit { get { return _g_isInit; } }

        /// <summary>
        /// 初始化函数 (common组件自动调用)
        /// </summary>
        /// <param name="_platform_ComponentInfo"></param>
        [UnityEngine.Scripting.Preserve]
        public static void init(MJSDK_Basic_ComponentInfo _platform_ComponentInfo)
        {
            if (_g_isInit)
            {
                Debug.Log(_g_MJSDK_LibName + " already init");
                return;
            }

            if (_platform_ComponentInfo == null)
            {
                Debug.Log(_g_MJSDK_LibName + " _platform_ComponentInfo empty");
                return;
            }

            //暂无需要处理的部分
            g_Version.printVersion();

#if UNITY_IOS
            //组件库依赖iOS平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 3, 7, 0);
#endif
        }

        #region 消耗性商品支付
        /// <summary>
        /// apple-localProduct：苹果-档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_localProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null ||
                _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "apple_localProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "localProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 苹果支付 - 苹果应用内支付
        /// </summary>
        /// <param name="_orderInfo"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_inAppPurchases(MJSDK_PhpApiCommon_Order_Info _orderInfo, Action<MJSDK_PhpApiCommon_ApplePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_orderInfo == null || string.IsNullOrEmpty(_orderInfo.order_id) || string.IsNullOrEmpty(_orderInfo.sku_id))
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "applePay_inAppPurchases --> Parameters are missing,order_id/sku_id is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "inAppPurchases", JsonUtility.ToJson(_orderInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_ApplePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 苹果支付成功，关闭订单通道（删除订单归档）
        /// </summary>
        /// <param name="_order_id"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_closeOrder(MJSDK_PhpApiCommon_2Engine_OrderVerify _2Engine_OrderVerify, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_2Engine_OrderVerify.order_id))
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "applePay_inAppPurchases --> Parameters are missing,order_id is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "closeOrder", JsonUtility.ToJson(_2Engine_OrderVerify), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
        #endregion

        #region 订阅型商品支付
        /// <summary>
        /// apple_localSubProduct：苹果-档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_localSubProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null ||
                _product_list.product == null
                || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "apple_localSubProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "localSubProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 苹果支付 - 苹果应用内订阅支付
        /// </summary>
        /// <param name="_orderInfo"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_subPurchases(MJSDK_PhpApiCommon_SubOrder_Info _subOrderInfo, Action<MJSDK_PhpApiCommon_ApplePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_subOrderInfo == null || string.IsNullOrEmpty(_subOrderInfo.sub_id) || string.IsNullOrEmpty(_subOrderInfo.pay_product_id))
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "applePay_subPurchases --> Parameters are missing,sub_id/pay_product_id is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "subPurchases", JsonUtility.ToJson(_subOrderInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_ApplePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 订阅型商品支付-恢复购买
        /// <summary>
        /// 恢复购买 - 获取票据编号
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_transactionId(Action<MJSDK_PhpApiCommon_Pay_TransactionId_List> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("apple", "transactionId", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId_List>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 恢复购买 - 获取票据信息
        /// </summary>
        /// <param name="_transaction_List">恢复购买票据id</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_transactionInfo(MJSDK_PhpApiCommon_Pay_TransactionId _transactionId, Action<MJSDK_PhpApiCommon_ApplePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_transactionId == null
            || _transactionId.original_transaction_id == null
            || _transactionId.transaction_id == null)
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "apple_transactionInfo --> Parameters are missing,original_transaction_id/transaction_id/_transactionId is null");
                return;
            }
            MJSDK.sendMsgToPhonePlatform("apple", "transactionInfo", JsonUtility.ToJson(_transactionId), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_ApplePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 苹果支付-获取漏单信息
        /// <summary>
        /// apple-leakOrder：苹果-获取漏单信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void apple_leakOrder(Action<MJSDK_ApplePay_2Engine_applePay_leakOrder> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("apple", "leakOrder", "", new MJSDK_CallbackDealer_Model<MJSDK_ApplePay_2Engine_applePay_leakOrder>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}
