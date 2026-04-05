using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- 账号操作
    /// </summary>
    public class MJSDK_PhpApiCommonLib_Account
    {

        #region account_login（账号登录）
        /// <summary>
        /// 苹果账号登录
        /// </summary>
        /// <param name="_apple_userinfo">apple 平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_apple(MJSDK_PhpApiCommon_Apple_UserInfo _apple_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_apple_userinfo == null
                || string.IsNullOrEmpty(_apple_userinfo.identityToken)
                || string.IsNullOrEmpty(_apple_userinfo.userID))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_apple --> Parameters are missing,identityToken/userID is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "apple", JsonUtility.ToJson(_apple_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 游客账号登录
        /// </summary>
        /// <param name="_deviceId">设备唯一识别符</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_guest(System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "guest", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 游戏圈账号登录
        /// </summary>
        /// <param name="_2SDK_Gamecenter_AccountLogin">苹果游戏圈平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_gamecenter(MJSDK_PhpApiCommon_Gamecenter_UserInfo _gamecenter_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_gamecenter_userinfo == null
                || string.IsNullOrEmpty(_gamecenter_userinfo.publicKeyUrl)
                || string.IsNullOrEmpty(_gamecenter_userinfo.playerID)
                || string.IsNullOrEmpty(_gamecenter_userinfo.signature)
                || string.IsNullOrEmpty(_gamecenter_userinfo.gc_timestamp)
                || string.IsNullOrEmpty(_gamecenter_userinfo.salt))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_gamecenter --> Parameters are missing, publicKeyUrl/playerID/signature/gc_timestamp/salt is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "gamecenter", JsonUtility.ToJson(_gamecenter_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// Facebook账号登录
        /// </summary>
        /// <param name="_2SDK_Facebook_AccountLogin">Facebook平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_facebook(MJSDK_PhpApiCommon_Facebook_UserInfo _facebook_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_facebook_userinfo == null
                || string.IsNullOrEmpty(_facebook_userinfo.accessToken)
                || string.IsNullOrEmpty(_facebook_userinfo.userID))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_facebook --> Parameters are missing,accessToken/userID is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "facebook", JsonUtility.ToJson(_facebook_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// vk账号登录
        /// </summary>
        /// <param name="_2SDK_vk_AccountLogin">vk平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_vk(MJSDK_PhpApiCommon_VK_UserInfo _vk_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_vk_userinfo == null
                || string.IsNullOrEmpty(_vk_userinfo.user_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_vk--> Parameters are missing,user_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "vk", JsonUtility.ToJson(_vk_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 谷歌账号登录
        /// </summary>
        /// <param name="_2SDK_Google_AccountLogin">google平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_google(MJSDK_PhpApiCommon_Google_UserInfo _google_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_google_userinfo == null
                || string.IsNullOrEmpty(_google_userinfo.accessToken)
                || string.IsNullOrEmpty(_google_userinfo.userID))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_google --> Parameters are missing,accessToken/userID is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "google", JsonUtility.ToJson(_google_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// accountLogin-line：line账号登录
        /// </summary>
        /// <param name="_2SDK_Line_AccountLogin">line平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_line(MJSDK_PhpApiCommon_Line_UserInfo _line_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_line_userinfo == null
                || string.IsNullOrEmpty(_line_userinfo.access_token)
                || string.IsNullOrEmpty(_line_userinfo.user_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_line --> Parameters are missing,access_token/user_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "line", JsonUtility.ToJson(_line_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// accountLogin-twitter：twitter账号登录
        /// </summary>
        /// <param name="_2SDK_Twitter_AccountLogin">推特平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_twitter(MJSDK_PhpApiCommon_Twitter_UserInfo _twitter_userinfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_twitter_userinfo == null
                || string.IsNullOrEmpty(_twitter_userinfo.auth_token)
                || string.IsNullOrEmpty(_twitter_userinfo.token_secret)
                || string.IsNullOrEmpty(_twitter_userinfo.user_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_twitter --> Parameters are missing,auth_token/user_id/token_secret is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "twitter", JsonUtility.ToJson(_twitter_userinfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// accountLogin-qq：qq账号登录
        /// </summary>
        /// <param name="_QQ_UserInfo">qq平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_qq(MJSDK_PhpApiCommon_QQ_UserInfo _QQ_UserInfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_QQ_UserInfo == null
                || string.IsNullOrEmpty(_QQ_UserInfo.access_token)
                || string.IsNullOrEmpty(_QQ_UserInfo.open_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_qq --> Parameters are missing,access_token/open_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "qq", JsonUtility.ToJson(_QQ_UserInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// accountLogin-wx：wx账号登录
        /// </summary>
        /// <param name="_WX_UserInfo">微信平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_wx(MJSDK_PhpApiCommon_WX_UserInfo _WX_UserInfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_WX_UserInfo == null
                || string.IsNullOrEmpty(_WX_UserInfo.code))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_wx --> Parameters are missing,code is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "wx", JsonUtility.ToJson(_WX_UserInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// accountLogin-mobile：手机账号登录
        /// </summary>
        /// <param name="_Mobile_Code">手机验证码信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_mobile(MJSDK_PhpApiCommon_Mobile_Code _Mobile_Code, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_Mobile_Code == null
                || string.IsNullOrEmpty(_Mobile_Code.mobile_code)
                || string.IsNullOrEmpty(_Mobile_Code.mobile))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_mobile --> Parameters are missing,mobile_code/mobile is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "mobile", JsonUtility.ToJson(_Mobile_Code), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// accountLogin-mjacc：mj账号登录
        /// </summary>
        /// <param name="_MJAcc_UserInfo">梦加账号平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_mjacc(MJSDK_PhpApiCommon_MJAcc_UserInfo _MJAcc_UserInfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_MJAcc_UserInfo == null
                || string.IsNullOrEmpty(_MJAcc_UserInfo.mj_token)
                || string.IsNullOrEmpty(_MJAcc_UserInfo.user_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_mjacc --> Parameters are missing,mj_token/user_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "mjacc", JsonUtility.ToJson(_MJAcc_UserInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// accountLogin-taptap：taptap账号登录
        /// </summary>
        /// <param name="_MJAcc_UserInfo">taptap账号平台用户信息</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void accountLogin_taptap(MJSDK_PhpApiCommon_TapTap_UserInfo _taptap_UserInfo, System.Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_PhpApi_CommonLib.checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_taptap_UserInfo == null
                || string.IsNullOrEmpty(_taptap_UserInfo.kid)
                || string.IsNullOrEmpty(_taptap_UserInfo.mac_key)
                || string.IsNullOrEmpty(_taptap_UserInfo.user_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "accountLogin_taptap --> Parameters are missing,kid/mac_key/user_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("accountLogin", "taptap", JsonUtility.ToJson(_taptap_UserInfo), new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_account_login>(_sucDelegate, _failDelegate));
        }

        #endregion
    }
}
