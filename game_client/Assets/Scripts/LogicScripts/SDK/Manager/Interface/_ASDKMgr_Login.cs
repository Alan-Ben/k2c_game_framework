using System;
using ALPackage;
using MJSDK_Package;
using GOE;
using NPEnum;
using UnityEngine;

namespace GOESDK
{
    /**
     * 登录流程WiKi：https://thoughts.teambition.com/workspaces/61de348291a8b50041519273/docs/64140596ce5676000142cb64
     */
    public abstract partial class _ASDKMgr
    {
        //登录操作序列号
        private int _m_iLoginSerialize = ALSerializeOpMgr.next();
        //当前SDK登录类型
        private ENPLoginWayType _m_eCurLoginType;
        //登录状态
        private ELoginState _m_eLoginState = ELoginState.NONE;
        //当前登录的成功回调
        private Action<SDKTokenData> _m_dOnLoginSucDelegate;
        //当前登录的失败回调
        private Action<int, string> _m_dOnLoginFailDelegate;
        


        /// <summary>
        /// 根据上次登录类型快速登录
        /// </summary>
        public void quickLogin(Action<SDKTokenData> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送埋点-开始SDK登录
            GCommon.sendStepReport(TraceConst.START_SDK_LOGIN);

            SDKUtil.showSDKDebugLog("[quickLogin] 开始快速登录");
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("quickLogin", (_code,_msg)=>
            {
                //发送埋点-SDK登录失败
                GCommon.sendStepReport(TraceConst.SDK_LOGIN_FAIL);
                _failDelegate?.Invoke(_code, _msg);
            }))
                return;

            //上次登录类型
            ENPLoginWayType type = _getLastSDKLoginType();
            
            //检查登录状态
            if (!_checkLoginState(type, _sucDelegate, _failDelegate))
                return;

            //更新操作序列号
            _m_iLoginSerialize = ALSerializeOpMgr.next();
            //记录当前登录类型
            _m_eCurLoginType = type;
            //注册回调
            _m_dOnLoginSucDelegate += _sucDelegate;
            _m_dOnLoginFailDelegate += _failDelegate;
            //设置登录状态
            _m_eLoginState = ELoginState.LOGING_PROCESS;
            //是否有缓存token
            bool haveToken = SDKLoginSetting.instance.hasToken();
            //缓存当前操作的序列号
            int curOpSerialize = _m_iLoginSerialize;
            //成功回调
            Action<MJSDK_PhpApiCommon_2Engine_user_login> onSuc = (_userLoginInfo) => _onLoginSuc(curOpSerialize, type, _userLoginInfo);
            //失败回调
            Action<int, string> onFail = (_errCode, _errMsg) => _onLoginFail(curOpSerialize, type, _errCode, _errMsg);

            //有缓存token，使用token自动登录，否则普通登录
            if (haveToken)
            {
                SDKUtil.showSDKDebugLog("[quickLogin] 存在token，使用token尝试登录");
                _userAutoLogin(onSuc, (_errCode, _errMsg) =>
                {
                    SDKUtil.showSDKDebugLog("[quickLogin] 使用token登录失败，走普通登录流程,type:", type);
                    //自动登录失败，使用普通登录
                    _userLogin(type, onSuc, onFail);
                });
            }
            else
            {
                SDKUtil.showSDKDebugLog("[quickLogin] 未缓存token,开始普通登录流程,type:", type);
                //没有缓存token使用普通登录
                _userLogin(type, onSuc, onFail);
            }
        }

        /// <summary>
        /// 根据指定类型登录
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void login(ENPLoginWayType _type, Action<SDKTokenData> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送埋点-开始SDK登录
            GCommon.sendStepReport(TraceConst.START_SDK_LOGIN);

            SDKUtil.showSDKDebugLog("[login] 开始登录,type:", _type);
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("login", (_code, _msg) =>
            {
                //发送埋点-SDK登录失败
                GCommon.sendStepReport(TraceConst.SDK_LOGIN_FAIL);
                _failDelegate?.Invoke(_code, _msg);
            }))
                return;

            //检查登录状态
            if (!_checkLoginState(_type, _sucDelegate, _failDelegate))
                return;

            //更新操作序列号
            _m_iLoginSerialize = ALSerializeOpMgr.next();
            //记录当前登录类型
            _m_eCurLoginType = _type;
            //设置登录状态
            _m_eLoginState = ELoginState.LOGING_PROCESS;
            //注册回调
            _m_dOnLoginSucDelegate += _sucDelegate;
            _m_dOnLoginFailDelegate += _failDelegate;
            //缓存当前操作的序列号
            int curOpSerialize = _m_iLoginSerialize;
            //成功回调
            Action<MJSDK_PhpApiCommon_2Engine_user_login> onSuc = (_userLoginInfo) => _onLoginSuc(curOpSerialize, _type, _userLoginInfo);
            //失败回调
            Action<int, string> onFail = (_errCode, _errMsg) => _onLoginFail(curOpSerialize, _type, _errCode, _errMsg);
            
            //开始登录
            _userLogin(_type, onSuc, onFail);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void logout()
        {
            //还未登录或者正在登录中不logout
            if (_m_eLoginState == ELoginState.NONE || _m_eLoginState == ELoginState.LOGING_PROCESS)
                return;

            //更新操作序列号
            _m_iLoginSerialize = ALSerializeOpMgr.next();
            //重置登录状态
            _m_eLoginState = ELoginState.NONE;
            //重置登录类型
            _m_eCurLoginType = ENPLoginWayType.NONE;
            //重置回调
            _m_dOnLoginSucDelegate = default(Action<SDKTokenData>);
            _m_dOnLoginFailDelegate = default(Action<int, string>);
        }

        #region SDK登录流程（1、普通登录：先accountLogin再userLogin  2、自动登录：先检查缓存token，未过期则用token调用userAutoLogin）

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _accountLogin(ENPLoginWayType _type, Action<MJSDK_PhpApiCommon_2Engine_account_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            SDKUtil.showSDKDebugLog("[_accountLogin] 开始账号登录,type:", _type);
            switch (_type)
            {
                //游客设备号登录
                case ENPLoginWayType.GUEST:
                    MJSDK_PhpApiCommonLib_Account.accountLogin_guest(_sucDelegate, _failDelegate);
                    break;
                //谷歌账号登录
                case ENPLoginWayType.GOOGLE:
                    MJSDK_GoogleLib.google_userInfo(_userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_google(_userInfo, _sucDelegate, _failDelegate);
                    },_failDelegate);
                    break;
                //facebook账号登录
                case ENPLoginWayType.FACEBOOK:
                    MJSDK_FacebookLib.facebook_userInfo(_userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_facebook(_userInfo, _sucDelegate, _failDelegate);
                    }, _failDelegate);
                    break;
                //vk账号登录
                case ENPLoginWayType.VK:
                    MJSDK_VKLib.vk_userInfo(_userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_vk(_userInfo, _sucDelegate, _failDelegate);
                    },_failDelegate);
                    break;
                //苹果账号登录
                case ENPLoginWayType.IOS:
                    MJSDK_AppleLoginLib.apple_userInfo(_userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_apple(_userInfo, _sucDelegate, _failDelegate);
                    }, _failDelegate);
                    break;
                //游戏圈账号登录
                case ENPLoginWayType.GAMECENTER:
                    MJSDK_GameCenterLib.gamecenter_userInfo(_userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_gamecenter(_userInfo, _sucDelegate, _failDelegate);
                    },_failDelegate);
                    break;
                //梦加账号登录
                case ENPLoginWayType.MJ:
                    MJSDK_MJAcc_2SDK_mjacc_userInfo mjUserInfo = new MJSDK_MJAcc_2SDK_mjacc_userInfo();
                    mjUserInfo.lan = GameSetting.instance.getCurrentLanguage().toPHPLanguageCode();
                    mjUserInfo.mj_token = null;
                    MJSDK_MJAccLib.mjacc_userInfo(mjUserInfo, _userInfo =>
                    {
                        MJSDK_PhpApiCommonLib_Account.accountLogin_mjacc(_userInfo, _sucDelegate, _failDelegate);
                    },_failDelegate);
                    break;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _userLogin(ENPLoginWayType _type, Action<MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            SDKUtil.showSDKDebugLog("[_userLogin] 开始用户登录,type:", _type);
            _accountLogin(_type, _accountInfo =>
            {
                //构造登录消息结构体
                MJSDK_PhpApiCommon_2SDK_user_login userLoginInfo = new MJSDK_PhpApiCommon_2SDK_user_login();
                userLoginInfo.account_token = _accountInfo.account_token;
                userLoginInfo.afid = "";
                userLoginInfo.login_params = "";
                userLoginInfo.nation = "";

                //用户登录
                MJSDK_PhpApiCommonLib_UserHandle.user_login(userLoginInfo, _sucDelegate, _failDelegate);
            }, _failDelegate);
        }

        /// <summary>
        /// 用户自动登录
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        private void _userAutoLogin(Action<MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate, Action<int, string> _failDelegate)
        {
            SDKUtil.showSDKDebugLog($"[_userAutoLogin] 开始用户自动登录");
            if (!SDKLoginSetting.instance.hasToken())
            {
                if (_failDelegate != null)
                    _failDelegate(COMMON_ERROR, "用户登录token为空");
                return;
            }

            //获取缓存的token
            SDKTokenData tokenData = SDKLoginSetting.instance.getTokenData();

            //检查token是否过期
            MJSDK_PhpApiCommon_2SDK_user_tokenVerfity tokenVerfityInfo = new MJSDK_PhpApiCommon_2SDK_user_tokenVerfity();
            tokenVerfityInfo.token = tokenData.token;
            tokenVerfityInfo.user_id = tokenData.user_id;
            MJSDK_PhpApiCommonLib_UserHandle.user_tokenVerify(tokenVerfityInfo, _result =>
            {
                //返回1是正常
                if (_result == "1")
                {
                    SDKUtil.showSDKDebugLog("[_userAutoLogin] token有效，开始用户自动登录");
                    //用户自动登录
                    MJSDK_PhpApiCommon_2SDK_user_autoLogin userAutoLoginInfo = new MJSDK_PhpApiCommon_2SDK_user_autoLogin();
                    userAutoLoginInfo.nation = "";
                    userAutoLoginInfo.afid = "";
                    userAutoLoginInfo.login_params = "";
                    userAutoLoginInfo.token = tokenData.token;
                    MJSDK_PhpApiCommonLib_UserHandle.user_autoLogin(userAutoLoginInfo, _sucDelegate, _failDelegate);
                }
                else
                {
                    SDKUtil.showSDKDebugLog("[_userAutoLogin] token无效，不自动登录");
                    if (_failDelegate != null)
                        _failDelegate(COMMON_ERROR, null);
                }
            }, _failDelegate);
        }

        #endregion

    
        #region 获取与保存登录类型

        /// <summary>
        /// 获取上次登录类型
        /// </summary>
        /// <returns></returns>
        private ENPLoginWayType _getLastSDKLoginType()
        {
            ENPLoginWayType lastLoginType = SDKLoginSetting.instance.getSDKLoginType();
            return lastLoginType;
        }

        /// <summary>
        /// 保存当前登录类型
        /// </summary>
        /// <param name="_type"></param>
        private void _setLastSDKLoginType(ENPLoginWayType _type)
        {
            _m_eCurLoginType = _type;
            SDKLoginSetting.instance.setSDKLoginType(_type);
        }

        #endregion


        #region 登录成功失败处理

        /// <summary>
        /// 登录成功
        /// </summary>
        /// <param name="_userLoginInfo"></param>
        private void _onLoginSuc(int _opSerialize, ENPLoginWayType _type, MJSDK_PhpApiCommon_2Engine_user_login _userLoginInfo)
        {
            SDKUtil.showSDKDebugLog("[_onLoginSuc] SDK登录成功，type:", _type);
            //先判断能操作序列号
            if (_opSerialize != _m_iLoginSerialize)
                return;

            if (_userLoginInfo == null || string.IsNullOrEmpty(_userLoginInfo.user_id) || string.IsNullOrEmpty(_userLoginInfo.token))
            {
                SDKUtil.showSDKLogError($"[_onLoginSuc] SDK登录成功，但是未取到玩家信息 type:{_type}");
                _onLoginFail(_opSerialize, _type, COMMON_ERROR, "未取到登录成功玩家信息");
                return;
            }

            string bindList = "";
            if (_userLoginInfo.bind != null)
            {
                for (int i = 0; i < _userLoginInfo.bind.Count; i++)
                {
                    if(_userLoginInfo.bind[i] == null)
                        continue;

                    bindList += _userLoginInfo.bind[i].type;
                    if(i != _userLoginInfo.bind.Count - 1)
                        bindList += ",";
                }
            }
            SDKUtil.showSDKDebugLog($"[_onLoginSuc] SDK登录成功，相关token信息：",
                "\n==========================token信息==========================",
                "\nuser_id:", _userLoginInfo.user_id,
                "\ntoken:", _userLoginInfo.token,
                "\ntoken_expiry:", _userLoginInfo.token_expiry,
                "\nbind:", bindList,
                "\nuser_pay:", _userLoginInfo.user_pay,
                "\ntoken_sign:", _userLoginInfo.token_sign,
                "\n============================================================");

            //重置登录序列号
            _m_iLoginSerialize = ALSerializeOpMgr.next();
            //缓存登录令牌及玩家信息
            SDKLoginSetting.instance.setTokenData(_userLoginInfo);
            //缓存登录类型
            _setLastSDKLoginType(_type);
            //设置登录状态
            _m_eLoginState = ELoginState.LOGIN_SUC;
            //解析已绑定列表
            _analyseBindList(_userLoginInfo.bind);
            //重置失败回调
            _m_dOnLoginFailDelegate = default(Action<int, string>);

            //调用成功处理
            Action<SDKTokenData> sucDelegate = _m_dOnLoginSucDelegate;
            _m_dOnLoginSucDelegate = default(Action<SDKTokenData>);
            if (null != sucDelegate)
                sucDelegate(new SDKTokenData(_userLoginInfo));

            //发送埋点-SDK登录成功
            GCommon.sendStepReport(TraceConst.SDK_LOGIN_SUC.setMarkParam(_type, _userLoginInfo.user_id, _userLoginInfo.token));
        }

        /// <summary>
        /// 登录失败
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_msg"></param>
        private void _onLoginFail(int _opSerialize, ENPLoginWayType _type, int _code, string _msg)
        {
            SDKUtil.showSDKDebugLog("[_onLoginFail] SDK登录失败，type:", _type,",code:", _code,",msg:", _msg);
            //先判断能操作序列号
            if (_opSerialize != _m_iLoginSerialize)
                return;

            //重置登录序列号
            _m_iLoginSerialize = ALSerializeOpMgr.next();
            //设置登录状态
            _m_eLoginState = ELoginState.NONE;
            //重置成功回调
            _m_dOnLoginSucDelegate = default(Action<SDKTokenData>);

            //调用失败处理
            Action<int, string> failDelegate = _m_dOnLoginFailDelegate;
            _m_dOnLoginFailDelegate = default(Action<int, string>);
            if (null != failDelegate)
                failDelegate(_code, $"{_type}登录失败，{_msg}");

            //发送埋点-SDK登录失败
            GCommon.sendStepReport(TraceConst.SDK_LOGIN_FAIL.setMarkParam(_type));
        }

        #endregion


        #region 相关检查方法

        /// <summary>
        /// 检查当前登录状态
        /// </summary>
        /// <returns>是否继续登录</returns>
        private bool _checkLoginState(ENPLoginWayType _loginType, Action<SDKTokenData> _onSuc, Action<int, string> _onFail)
        {
            SDKUtil.showSDKDebugLog("[_checkLoginState] 检查登录状态，type:", _loginType,",state:",_m_eLoginState);
            switch (_m_eLoginState)
            {
                //初始状态
                case ELoginState.NONE:
                    return true;
                //正在登录
                case ELoginState.LOGING_PROCESS:
                    //如果是当前正在登录的类型，注册回调
                    if (_m_eCurLoginType == _loginType)
                    {
                        //注册回调
                        _m_dOnLoginSucDelegate += _onSuc;
                        _m_dOnLoginFailDelegate += _onFail;
                    }
                    else
                    {
                        //类型不同，直接失败处理
                        if (_onFail != null)
                            _onFail(COMMON_ERROR, "重复登录时其他SDK登录正在进行中");

                        //发送埋点-SDK登录失败
                        GCommon.sendStepReport(TraceConst.SDK_LOGIN_FAIL.setMarkParam(_loginType));
                    }
                    return false;
                //已经登录
                case ELoginState.LOGIN_SUC:
                    if (_m_eCurLoginType != _loginType)
                    {
                        logout();
                        return true;
                    }
                    else
                    {
                        //此时登录成功且状态一致，这样直接调用成功然后返回
                        if (null != _onSuc)
                            _onSuc(SDKLoginSetting.instance.getTokenData());

                        //发送埋点-SDK登录成功
                        SDKTokenData tokenData = SDKLoginSetting.instance.getTokenData();
                        if(tokenData != null)
                            GCommon.sendStepReport(TraceConst.SDK_LOGIN_SUC.setMarkParam(_loginType, tokenData.user_id, tokenData.token));

                        return false;
                    }
            }

            return true;
        }

        /// <summary>
        /// 判断是否可以使用sign in with apple，这个功能只能在iOS 13及以上才支持
        /// </summary>
        /// <returns></returns>
        public bool checkCanUseAppleLogin()
        {
#if !UNITY_IOS
            return false;
#endif
            int startIndex = 0;
            string deviceOS = SystemInfo.operatingSystem;//参考返回内容：iOS 13.5.1 或 iPhone OS 8.4 或 iPadOS 15.3
            string checkIOS = deviceOS.Substring(0, 4);

            if (checkIOS.Equals("iOS "))
                startIndex = 4;
            else if (checkIOS.Equals("iPad"))
                startIndex = 7;
            else
                startIndex = 10;

            if (string.IsNullOrEmpty(deviceOS))
            {
                return false;
            }
            else
            {
                int length = deviceOS.Length - startIndex;
                if (length <= 0)
                    return false;

                //去掉系统版本前的iOS和空格
                string version = deviceOS.Substring(startIndex, length);
                //拆分系统版本号
                string[] versionArray = version.Split('.');
                //判断第一位版本号是否大于等于13
                if (versionArray.Length > 0 && int.TryParse(versionArray[0], out int versionNum))
                {
                    if (versionNum >= 13)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion




        /// <summary>
        /// 登录状态
        /// </summary>
        private enum ELoginState
        {
            NONE,//未登录
            LOGING_PROCESS,//正在登录
            LOGIN_SUC,//已经登录
        }
    }
}
