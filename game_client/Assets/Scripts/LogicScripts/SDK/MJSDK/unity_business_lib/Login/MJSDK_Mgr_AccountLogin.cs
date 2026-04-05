using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_AccountLogin
    {
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="_e_MJSDK_AccountType">第三方登录类型</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        /// <param name="_thirdSDK_arg">第三方登录所需参数,判定你选择第三方是否需要参数，请到各个组件接口文档参考xx_userInfo协议说明</param>
        ///  <param name="_thirdSDK_arg">_e_MJSDK_AccountType == E_MJSDK_AccountType.mjacc,_thirdSDK_arg请传入"MJSDK_MJAcc_2SDK_mjacc_userInfo"对象</param>
        public static void req_accountLogin(E_MJSDK_AccountType _e_MJSDK_AccountType,
        Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate,
        Action<int, string> _failDelegate,
        object _thirdSDK_arg = null)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibAccountMainOrder(_e_MJSDK_AccountType));
            if (_thirdSDK_arg != null)
            {
                ht.Add("thirdSDKArg", JsonUtility.ToJson(_thirdSDK_arg));
            }
            MJSDK.sendMsgToPhonePlatform("mj", "accountLogin", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 获取第三方用户信息（缓存）
        /// 获取最近一次使用Business组件中登录/绑定业务中涉及的第三方用户信息。
        /// </summary>
        /// <param name="_e_MJSDK_AccountType">第三方账号类型</param>
        /// <param name="_sucDelegate">成功回执 ，回执字符串，
        /// 使用者根据E_MJSDK_AccountType类型，并使用“JsonUtility.FromJson<T>(_content);”转换对应数据对象。
        /// 如E_MJSDK_AccountType.mj  对象为：MJSDK_PhpApiCommon_MJAcc_UserInfo。。 </param>
        /// <param name="_failDelegate">失败回执</param>
        public static void getThirdPartyUserInfoFromCache(E_MJSDK_AccountType _e_MJSDK_AccountType,
        Action<string> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibAccountMainOrder(_e_MJSDK_AccountType));
            MJSDK.sendMsgToPhonePlatform("mj", "thirdUserInfoFromCache", ht.toJson(),
            new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}
