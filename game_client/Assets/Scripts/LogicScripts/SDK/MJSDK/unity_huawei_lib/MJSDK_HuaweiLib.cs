using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK Huawei组件脚本库
    /// </summary>
    public class MJSDK_HuaweiLib
    {
        //组件标识符  --- 库名
        private static string _g_MJSDK_LibName = "MJSDK_Huawei";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 5, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 0);
#endif
        }



        /// <summary>
        /// huawei-inAppPurchases：huawei-huawei应用内支付
        /// </summary>
        /// <param name="_order_info">订单信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_inAppPurchases(MJSDK_PhpApiCommon_Order_Info _order_info, Action<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
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
                    _failDelegate(MJSDK_HuaweiError.C_Unity_Param_Miss, "huawei_inAppPurchases --> Parameters are missing,order_id/sku_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("huawei", "inAppPurchases", JsonUtility.ToJson(_order_info), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// huawei-subPurchases：huawei-huawei订阅支付
        /// </summary>
        /// <param name="_order_info">订单信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_subPurchases(MJSDK_PhpApiCommon_SubOrder_Info _order_info, Action<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_order_info == null
                || string.IsNullOrEmpty(_order_info.sub_id)
                || string.IsNullOrEmpty(_order_info.pay_product_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_GoogleError.C_Unity_Param_Miss, "huawei_subPurchases --> Parameters are missing,sub_id/pay_product_group/pay_product_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("huawei", "subPurchases", JsonUtility.ToJson(_order_info), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// huawei-localProduct：huawei-档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_localProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null ||
                _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_HuaweiError.C_Unity_Param_Miss, "huawei_localProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("huawei", "localProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// huawei-localSubProduct：huawei-订阅档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getSubProduct接口进行获取</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_localSubProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null ||
                _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_HuaweiError.C_Unity_Param_Miss, "huawei_localSubProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("huawei", "localSubProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 恢复购买 - 获取票据编号
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_transactionId(Action<MJSDK_PhpApiCommon_Pay_TransactionId_List> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("huawei", "transactionId", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId_List>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 恢复购买 - 获取票据信息
        /// </summary>
        /// <param name="_transaction_List">恢复购买票据id</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void huawei_transactionInfo(MJSDK_PhpApiCommon_Pay_TransactionId _transactionId, Action<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_transactionId == null
            || _transactionId.original_transaction_id == null
            || _transactionId.transaction_id == null)
            {
                _failDelegate(MJSDK_ApplePayError.C_Unity_Param_Miss, "huawei_transactionInfo --> Parameters are missing,original_transaction_id/transaction_id/_transactionId is null");
                return;
            }
            MJSDK.sendMsgToPhonePlatform("huawei", "transactionInfo", JsonUtility.ToJson(_transactionId), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_HuaweiPay_Purchases_Info>(_sucDelegate, _failDelegate));
        }
    }
}
