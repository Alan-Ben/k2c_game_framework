using System;
using System.Collections;
using UnityEngine;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_SubPay
    {
        #region 支付--订阅商品
        /// <summary>
        /// 获取档位本地化
        /// </summary>
        /// <param name="_get_product">参数信息</param>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_sucDelegate">成功回调</param>
        /// <param name="_failDelegate">失败回调</param>
        public static void req_localSubProduct(E_MJSDK_PayType _MJSDK_PayType,
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
                    "req_localSubProduct --> Parameters are missing,_get_product is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("channle_type", JsonUtility.ToJson(_get_product));
            MJSDK.sendMsgToPhonePlatform("mj", "localSubProduct", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_2Engine_ThirdParty_localProduct_list>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 第三方渠道订阅支付
        /// </summary>
        /// <param name="_product_info">商品信息</param>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void req_subPay(E_MJSDK_PayType _MJSDK_PayType,
        MJSDK_PhpApiCommon_2SDK_create_subOrder _product_info,
        Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (_product_info == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "req_subPay --> Parameters are missing,_get_product is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("productInfo", JsonUtility.ToJson(_product_info));
            MJSDK.sendMsgToPhonePlatform("mj", "subPay", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 恢复购买
        ///第一步---------查询可恢复的订单票据--------------
        /// <summary>
        /// 查询可恢复的订单票据
        /// </summary>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void restoreStepOne_selectTransactionId(
            E_MJSDK_PayType _MJSDK_PayType,
            Action<MJSDK_PhpApiCommon_Pay_TransactionId_List> _sucDelegate,
            Action<int, string> _failDelegate)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("pay_type", _MJSDK_PayType.ToString());
            MJSDK.sendMsgToPhonePlatform("mj", "transactionId", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Pay_TransactionId_List>(_sucDelegate, _failDelegate));
        }

        ///第二步---------使用可恢复票据id进行恢复购买--------------
        /// <summary>
        /// 查询可恢复的订单票据
        /// </summary>
        /// <param name="_MJSDK_PayType">第三方渠道</param>
        /// <param name="_transactionId">票据id</param>
        /// <param name="_product_info">apple渠道不可为空</param>
        /// <param name="_sucDelegate">返回验证结果</param>
        /// <param name="_failDelegate"></param>
        public static void restoreStepTwo_restorePay(
            E_MJSDK_PayType _MJSDK_PayType,
            MJSDK_PhpApiCommon_Pay_TransactionId _transactionId,
            Action<MJSDK_PhpApiCommon_2Engine_OrderVerify> _sucDelegate,
            Action<int, string> _failDelegate,
            MJSDK_PhpApiCommon_2SDK_create_subOrder _product_info = null)
        {
            //必要参数不得为空
            if (_transactionId == null)
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "恢复购买请求失败，reason：_transactionId 为空");
                return;
            }
            if ((_MJSDK_PayType == E_MJSDK_PayType.apple || _MJSDK_PayType == E_MJSDK_PayType.google) && _product_info == null)
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "恢复购买请求失败，reason：_product_info 为空");
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            if (_product_info != null)
            {
                ht.Add("productInfo", JsonUtility.ToJson(_product_info));
            }
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            ht.Add("transactionId", JsonUtility.ToJson(_transactionId));
            MJSDK.sendMsgToPhonePlatform("mj", "restorePay", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_OrderVerify>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}

