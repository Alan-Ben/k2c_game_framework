using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MJSDK_Package
{
    public class MJSDK_Mgr_AppReview
    {
        /// <summary>
        /// 应用内评价
        /// </summary>
        /// <param name="_MJSDK_PayType"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void app_review(E_MJSDK_PayType _MJSDK_PayType,
        Action<string> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibPayMainOrder(_MJSDK_PayType));
            MJSDK.sendMsgToPhonePlatform("mj", "appReview", ht.toJson(),
            new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}
