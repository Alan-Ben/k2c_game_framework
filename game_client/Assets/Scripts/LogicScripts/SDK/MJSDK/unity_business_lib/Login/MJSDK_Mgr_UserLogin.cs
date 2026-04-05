using System;
using System.Collections;
using UnityEngine;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_UserLogin
    {

        #region 用户登录
        /// <summary>
        /// 用户第三方账号登录
        /// </summary>
        /// <param name="_e_MJSDK_AccountLoginType">第三方登录类型</param>
        /// <param name="_2SDK_User_Login">登录参数</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        /// <param name="_thirdSDK_arg">第三方登录所需参数,判定你选择第三方是否需要参数，请到各个组件接口文档参考xx_userInfo协议说明</param>
        ///  <param name="_thirdSDK_arg">_e_MJSDK_AccountType == E_MJSDK_AccountType.mjacc,_thirdSDK_arg请传入"MJSDK_MJAcc_2SDK_mjacc_userInfo"对象</param>
        public static void req_userLogin(E_MJSDK_AccountType _e_MJSDK_AccountLoginType,
        MJSDK_PhpApiCommon_2SDK_user_login _2SDK_User_Login,
        Action<MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate,
        Action<int, string> _failDelegate,
        object _thirdSDK_arg = null)
        {
            //必要参数不得为空
            if (_2SDK_User_Login == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "req_userLogin --> Parameters are missing,_2SDK_User_Login is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibAccountMainOrder(_e_MJSDK_AccountLoginType));
            if (_thirdSDK_arg != null)
            {
                ht.Add("thirdSDKArg", JsonUtility.ToJson(_thirdSDK_arg));
            }
            ht.Add("params", JsonUtility.ToJson(_2SDK_User_Login));
            MJSDK.sendMsgToPhonePlatform("mj", "userLogin", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_login>(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 用户绑定与解绑
        /// <summary>
        /// 账号绑定
        /// </summary>
        /// <param name="_e_MJSDK_AccountLoginType">第三方渠道类型</param>
        /// <param name="_userToken">用户登录令牌</param>
        /// <param name="_isBind">绑定/解绑</param>
        /// <param name="_sucDelegate">成功回执</param> 
        /// <param name="_failDelegate">失败回执</param>
        /// <param name="_thirdSDK_arg">第三方登录所需参数,判定你选择第三方是否需要参数，请到各个组件接口文档参考xx_userInfo协议说明</param>
        ///  <param name="_thirdSDK_arg">_e_MJSDK_AccountType == E_MJSDK_AccountType.mjacc,_thirdSDK_arg请传入"MJSDK_MJAcc_2SDK_mjacc_userInfo"对象</param>
        public static void req_userAccountBind(E_MJSDK_AccountType _e_MJSDK_AccountLoginType,
        string _userToken,
        bool _isBind,
        Action<MJSDK_PhpApiCommon_2Engine_user_accountBind> _sucDelegate,
        Action<int, string> _failDelegate,
        object _thirdSDK_arg = null)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_userToken))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "req_userAccountBind --> Parameters are missing,_userToken is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("mainOrder", MJSDK_Mgr_Mark.getMJSDKLibAccountMainOrder(_e_MJSDK_AccountLoginType));
            if (_thirdSDK_arg != null)
            {
                ht.Add("thirdSDKArg", JsonUtility.ToJson(_thirdSDK_arg));
            }
            ht.Add("token", _userToken);
            ht.Add("isBind", _isBind ? "1" : "0");
            MJSDK.sendMsgToPhonePlatform("mj", "userBind", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_accountBind>(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 用户注销/取消注销
        /// <summary>
        /// 用户注销与取消注销
        /// </summary>
        /// <param name="_isdel"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void req_userDel(string _userToken,
        bool _isdel,
        Action<MJSDK_PhpApiCommon_2Engine_user_del> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_userToken))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss,
                    "req_userDel --> Parameters are missing,_userToken is null");
                }
                return;
            }
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("isdel", _isdel ? "1" : "0");
            ht.Add("token", _userToken);
            MJSDK.sendMsgToPhonePlatform("mj", "userDel", ht.toJson(),
            new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_del>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}
