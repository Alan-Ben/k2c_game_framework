using System;
using System.Collections;
using UnityEngine;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_Pay
    {

        #region 支付--消耗性商品
        /// <summary>
        /// 获取档位本地化
        /// </summary>
        /// <param name="_get_product">参数信息</param>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_sucDelegate">成功回调</param>
        /// <param name="_failDelegate">失败回调</param>
        public static void req_localProduct(E_MJSDK_PayType _MJSDK_PayType,
         MJSDK_PhpApiCommon_2SDK_get_product _get_product,
         Action<MJSDK_2Engine_ThirdParty_localProduct_list> _sucDelegate,
         Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (_get_product == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "req_localProduct --> Parameters are missing,_get_product is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("channle_type", JsonUtility.ToJson(_get_product));
            MJSDK.sendMsgToPhonePlatform("mj", "localProduct", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 第三方渠道支付
        /// </summary>
        /// <param name="_product_info">商品信息</param>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void req_pay(E_MJSDK_PayType _MJSDK_PayType,
        MJSDK_PhpApiCommon_2SDK_create_order _product_info,
        Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (_product_info == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "_product_info --> Parameters are missing,_get_product is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("productInfo", JsonUtility.ToJson(_product_info));
            MJSDK.sendMsgToPhonePlatform("mj", "pay", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}
