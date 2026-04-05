using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Google组件脚本库
    /// </summary>
    public class MJSDK_GoogleLib
    {
        //组件标识符  --- 库名
        private static string _g_MJSDK_LibName = "MJSDK_Google";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 2, 12, 0);
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

#if UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 2, 0);
#endif
        }

        /// <summary>
        /// 获取google用户信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_userInfo(Action<MJSDK_PhpApiCommon_Google_UserInfo> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("google", "userInfo", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Google_UserInfo>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// google-inAppPurchases：google-google应用内支付
        /// </summary>
        /// <param name="_2SDK_Google_InAppPurchases"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_inAppPurchases(MJSDK_PhpApiCommon_Order_Info _order_info, Action<MJSDK_PhpApiCommon_GooglePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_order_info == null
                || string.IsNullOrEmpty(_order_info.order_id)
                || string.IsNullOrEmpty(_order_info.sku_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_GoogleError.C_Unity_Param_Miss, "google_inAppPurchases --> Parameters are missing,order_id/sku_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("google", "inAppPurchases", JsonUtility.ToJson(_order_info), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_GooglePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// google支付--google订阅支付
        /// </summary>
        /// <param name="_2SDK_Google_InAppPurchases"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_subPurchases(MJSDK_PhpApiCommon_SubOrder_Info _order_info, Action<MJSDK_PhpApiCommon_GooglePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_order_info == null
                || string.IsNullOrEmpty(_order_info.sub_id)
                || string.IsNullOrEmpty(_order_info.pay_product_group)
                || string.IsNullOrEmpty(_order_info.pay_product_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_GoogleError.C_Unity_Param_Miss, "google_subPurchases --> Parameters are missing,sub_id/pay_product_group/pay_product_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("google", "subPurchases", JsonUtility.ToJson(_order_info), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_GooglePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// google-queryPurchases：google检查漏单（项目组如果在start那边（也就是应用启动时）调用，请延迟一秒）
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_queryPurchases(Action<MJSDK_Google_2Engine_google_queryPurchases> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("google", "queryPurchases", "", new MJSDK_CallbackDealer_Model<MJSDK_Google_2Engine_google_queryPurchases>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// google-localProduct：谷歌-档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        [UnityEngine.Scripting.Preserve]
        public static void google_localProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null || _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "google_localProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("google", "localProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));

        }

        /// <summary>
        /// google-localSubProduct：谷歌-订阅档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        [UnityEngine.Scripting.Preserve]
        public static void google_localSubProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null || _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "google_localSubProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("google", "localSubProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));

        }


        /// <summary>
        /// google-appReview：应用内评价
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void google_appReview(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("google", "appReview", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 恢复购买 - 获取票据编号
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_transactionId(Action<MJSDK_PhpApiCommon_Pay_TransactionId_List> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("google", "transactionId", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId_List>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 恢复购买 - 获取票据信息
        /// </summary>
        /// <param name="_transaction_List">恢复购买票据id</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void google_transactionInfo(MJSDK_PhpApiCommon_Pay_TransactionId _transactionId, Action<MJSDK_PhpApiCommon_GooglePay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_transactionId == null
            || _transactionId.original_transaction_id == null
            || _transactionId.transaction_id == null)
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "google_transactionInfo --> Parameters are missing,original_transaction_id/transaction_id/_transactionId is null");
                return;
            }
            MJSDK.sendMsgToPhonePlatform("google", "transactionInfo", JsonUtility.ToJson(_transactionId), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_GooglePay_Purchases_Info>(_sucDelegate, _failDelegate));
        }
    }
}
