using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- 用户操作
    /// </summary>
    public class MJSDK_PhpApiCommonLib_UserHandle
    {

        #region user_handle（用户操作登录）

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="_2SDK_User_Login"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_login(MJSDK_PhpApiCommon_2SDK_user_login _2SDK_User_Login, System.Action<MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_User_Login == null
                || string.IsNullOrEmpty(_2SDK_User_Login.account_token))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_login --> Parameters are missing,account_token is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("user", "login", JsonUtility.ToJson(_2SDK_User_Login), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_login>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 用户自动登录
        /// </summary>
        /// <param name="_user_token">用户登录令牌</param>
        /// <param name="_user_id">用户id</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_autoLogin(MJSDK_PhpApiCommon_2SDK_user_autoLogin _2SDK_User_AutoLogin, System.Action<MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_User_AutoLogin == null
                || string.IsNullOrEmpty(_2SDK_User_AutoLogin.token))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_autoLogin --> Parameters are missing,token is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("user", "autoLogin", JsonUtility.ToJson(_2SDK_User_AutoLogin), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_login>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 用户登录令牌校验
        /// </summary>
        /// <param name="_user_token">用户登录令牌</param>
        /// <param name="_user_id">用户id</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_tokenVerify(MJSDK_PhpApiCommon_2SDK_user_tokenVerfity _2SDK_User_TokenVerfity, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_User_TokenVerfity == null
                || string.IsNullOrEmpty(_2SDK_User_TokenVerfity.user_id)
                || string.IsNullOrEmpty(_2SDK_User_TokenVerfity.token))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_tokenVerfity --> Parameters are missing,token/user_id is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("user", "tokenVerify", JsonUtility.ToJson(_2SDK_User_TokenVerfity), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 账号绑定
        /// </summary>
        /// <param name="_user_token">用户登录令牌</param>
        /// <param name="_account_token">账号登录令牌</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_accountBind(MJSDK_PhpApiCommon_2SDK_user_accountBind _2SDK_User_AccountBind, Action<MJSDK_PhpApiCommon_2Engine_user_accountBind> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_User_AccountBind == null
                || string.IsNullOrEmpty(_2SDK_User_AccountBind.token)
                || string.IsNullOrEmpty(_2SDK_User_AccountBind.account_token))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_accountBind --> Parameters are missing,token/account_token is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("user", "accountBind", JsonUtility.ToJson(_2SDK_User_AccountBind), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_accountBind>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 账号解绑
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_accountUnBind(MJSDK_PhpApiCommon_2SDK_user_accountBind _2SDK_User_AccountBind, Action<MJSDK_PhpApiCommon_2Engine_user_accountBind> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_User_AccountBind == null
                || string.IsNullOrEmpty(_2SDK_User_AccountBind.token)
                || string.IsNullOrEmpty(_2SDK_User_AccountBind.account_token))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_accountUnBind --> Parameters are missing,token/account_token is null");
                }
                return;
            }


            MJSDK.sendMsgToPhonePlatform("user", "accountUnBind", JsonUtility.ToJson(_2SDK_User_AccountBind), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_accountBind>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="_user_token"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_del(string _user_token , Action<MJSDK_PhpApiCommon_2Engine_user_del> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_user_token))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_del --> Parameters are missing,_user_token is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("token", _user_token);

            MJSDK.sendMsgToPhonePlatform("user", "del", ht.toJson(), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_del>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 用户取消注销
        /// </summary>
        /// <param name="_user_token"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_closeDel(string _user_token, Action<MJSDK_PhpApiCommon_2Engine_user_del> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_user_token))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_closeDel --> Parameters are missing,_user_token is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("token", _user_token);

            MJSDK.sendMsgToPhonePlatform("user", "closeDel", ht.toJson(), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_user_del>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// mj用户强登
        /// </summary>
        /// <param name="_user_token"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void user_mjServiceLogin(string _user_token, Action<MJSDK_PhpApiCommon_MJAcc_UserInfo> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_user_token))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "user_mjServiceLogin --> Parameters are missing,_user_token is null");
                return;
            }
            MJSDK.sendMsgToPhonePlatform("user", "mjServiceLogin", _user_token, new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_MJAcc_UserInfo>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}
