using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    ///MJSDK_SamsungLib
    /// </summary>
    public class MJSDK_AmazonLib
    {
        //组件标识符  --- 库名
        private static string _g_MJSDK_LibName = "MJSDK_Amazon";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 1, 0);
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
        /// Amazon-inAppPurchases：Amazon-Amazon应用内支付
        /// </summary>
        /// <param name="_order_info">订单信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void amazon_inAppPurchases(MJSDK_PhpApiCommon_Order_Info _order_info, Action<MJSDK_PhpApiCommon_AmazonPay_Purchases_Info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_order_info == null
                || string.IsNullOrEmpty(_order_info.order_id)
                || string.IsNullOrEmpty(_order_info.product_type)
                || string.IsNullOrEmpty(_order_info.sku_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AmazonError.C_Unity_Param_Miss, "amazon_inAppPurchases --> Parameters are missing,order_id/sku_id/product_type is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("amazon", "inAppPurchases", JsonUtility.ToJson(_order_info), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_AmazonPay_Purchases_Info>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// amazon-localProduct：amazon-档位信息转换为本地商品信息
        /// </summary>
        /// <param name="_product_list">调用MJSDK_PhpApiCommonLib_Order.order_getProduct接口进行获取</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void amazon_localProduct(MJSDK_PhpApiCommon_Product_List _product_list, Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_product_list == null ||
                _product_list.product == null || _product_list.product.Count == 0)
            {
                _failDelegate(MJSDK_AmazonError.C_Unity_Param_Miss, "amazon_localProduct --> Parameters are missing,_product_info is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("amazon", "localProduct", JsonUtility.ToJson(_product_list), new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }
    }
}