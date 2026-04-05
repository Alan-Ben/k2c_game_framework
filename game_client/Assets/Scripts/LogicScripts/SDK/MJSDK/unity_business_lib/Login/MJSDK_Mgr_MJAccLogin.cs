using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_MJAccLogin
    {
        private static MJSDK_Mgr_MJAccLogin _m_instance;
        public static MJSDK_Mgr_MJAccLogin g_instance
        {
            get
            {
                if (_m_instance == null)
                {
                    _m_instance = new MJSDK_Mgr_MJAccLogin();
                    _m_instance.initData();
                }
                return _m_instance;
            }
        }

        //初始化相关数据
        public void initData()
        {
            _mjTokenDic = new Dictionary<string, string>();
        }


        private Dictionary<string, string> _mjTokenDic;
        //游戏语言
        private string _gameLan;
        //用户信息
        private MJSDK_PhpApiCommon_2Engine_user_login _userInfo;
        //登录参数
        private MJSDK_PhpApiCommon_2SDK_user_autoLogin _loginParam;
        //成功回执
        private Action<E_MJSDK_MJAcc_LoginType, MJSDK_PhpApiCommon_2Engine_user_login> _sucDelegate;
        //失败回执
        private Action<int, string> _failDelegate;
        //防止重复点击
        private bool _isShowCenter = false;


        #region 请求梦加中心
        /// <summary>
        /// 请求梦加账号中心
        /// </summary>
        /// <param name="_2SDK_User_Login">参数内容</param>
        /// <param name="_game_lan">游戏语言</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        public void reqMJAccCenter(MJSDK_PhpApiCommon_2SDK_user_autoLogin user_AutoLogin,
        string game_lan,
        Action<E_MJSDK_MJAcc_LoginType, MJSDK_PhpApiCommon_2Engine_user_login> sucDelegate,
        Action<int, string> failDelegate)
        {

            //防止多次点击
            if (_isShowCenter)
            {
                failDelegate((int)E_MJSDK_MJAcc_LoginFailType.FrequentOperations, "正在处理上个业务请求，请勿重复操作");
                return;
            }

            //数据保存
            _loginParam = user_AutoLogin;
            _gameLan = game_lan;
            _sucDelegate = sucDelegate;
            _failDelegate = failDelegate;
            _isShowCenter = true;

            try
            {
                //第一步 自动登录获取token与绑定信息
                MJSDK_PhpApiCommonLib_UserHandle.user_autoLogin(user_AutoLogin, (MJSDK_PhpApiCommon_2Engine_user_login _valueResult) =>
                {
                    dealUserInfo(_valueResult);
                }, (int _errCode, string _errMsg) =>
                {
                    dealFailData(E_MJSDK_MJAcc_LoginFailType.GuestLoginFail, _errCode, _errMsg);
                });
            }
            catch (Exception ex)
            {
                dealFailData(E_MJSDK_MJAcc_LoginFailType.Exception_Err, (int)E_MJSDK_MJAcc_LoginFailType.Exception_Err, "reqMJAccCenter 请求异常，reason：" + ex.Message);
            }
        }

        /// <summary>
        /// 处理用户数据
        /// </summary>
        /// <param name="_value"></param>
        private void dealUserInfo(MJSDK_PhpApiCommon_2Engine_user_login value)
        {
            try
            {
                _userInfo = value;
                //与当前用户信息是否绑定关系
                bool isBind_mj = false;
                //判定当前用户信息是否绑定梦加账号
                foreach (var item in _userInfo.bind)
                {
                    if (item.type.Equals("mj"))
                    {
                        isBind_mj = true;
                        break;
                    }
                }

                //判断是否绑定状态
                if (isBind_mj)
                {
                    //取出token
                    string mjToken = getMJToken(_userInfo.user_id);
                    //判断token是否为空
                    if (string.IsNullOrEmpty(mjToken))
                    {
                        //无缓存，从SDK中获取
                        MJSDK_PhpApiCommonLib_UserHandle.user_mjServiceLogin(_userInfo.token, (MJSDK_PhpApiCommon_MJAcc_UserInfo mjacc_userInfo) =>
                        {
                            //缓存token
                            saveMJToken(_userInfo.user_id, mjacc_userInfo.mj_token);
                            showMJAccCenter();
                        }, (int errCode, string errMsg) =>
                        {
                            dealFailData(E_MJSDK_MJAcc_LoginFailType.GetMJTokenFail, errCode, errMsg);
                        });
                    }
                    else
                    {
                        showMJAccCenter();
                    }
                }
                else
                {
                    showMJAccCenter();
                }
            }
            catch (Exception ex)
            {
                dealFailData(E_MJSDK_MJAcc_LoginFailType.Exception_Err, (int)E_MJSDK_MJAcc_LoginFailType.Exception_Err, "dealUserInfo 请求异常，reason：" + ex.Message);
            }
        }
        #endregion

        #region 打开梦加中心
        /// <summary>
        /// 打开MJAcc中心
        /// </summary>
        private void showMJAccCenter()
        {
            try
            {
                //生成MJ_MJAcc参数
                MJSDK_MJAcc_2SDK_mjacc_userInfo mjacc_UserInfoParam = new MJSDK_MJAcc_2SDK_mjacc_userInfo();
                mjacc_UserInfoParam.lan = _gameLan;
                mjacc_UserInfoParam.mj_token = getMJToken(_userInfo.user_id);
                //打开MJ_MJAcc账号中心
                MJSDK_MJAccLib.mjacc_userInfo(mjacc_UserInfoParam, (MJSDK_PhpApiCommon_MJAcc_UserInfo mJAcc_UserInfo) =>
                {
                    if (mJAcc_UserInfo == null || mJAcc_UserInfo.operate_type == null || mJAcc_UserInfo.mj_token == null)
                    {
                        dealFailData(E_MJSDK_MJAcc_LoginFailType.Exception_Err, (int)E_MJSDK_MJAcc_LoginFailType.Exception_Err, "梦加账号中心异常，返回对象为空/operate_type为空");
                        return;
                    }

                    //MJ_MJAcc 用户操作为登录，走绑定操作
                    if (mJAcc_UserInfo.operate_type.Equals("1"))
                    {
                        bindMJAccount(mJAcc_UserInfo);
                    }
                    //MJ_MJAcc 用户操作为切换登录，走第三方登录操作
                    else if (mJAcc_UserInfo.operate_type.Equals("2"))
                    {
                        swicthLogin(mJAcc_UserInfo);
                    }
                }, (int errCode, string errMsg) =>
                {

                    //用户关闭梦加账号中心
                    if (errCode == 40703)
                    {
                        dealSucData(E_MJSDK_MJAcc_LoginType.None);
                    }
                    else
                    {
                        dealFailData(E_MJSDK_MJAcc_LoginFailType.MJCenterBusFail, errCode, errMsg);
                    }
                });
            }
            catch (Exception ex)
            {
                dealFailData(E_MJSDK_MJAcc_LoginFailType.Exception_Err, (int)E_MJSDK_MJAcc_LoginFailType.Exception_Err, "showMJAccCenter 请求异常，reason：" + ex.Message);
            }
        }
        #endregion

        #region 梦加账号操作
        /// <summary>
        /// 当前绑定梦加账号
        /// </summary>
        /// <param name="_mJAcc_UserInfo">梦加账号中心用户数据</param>
        private void bindMJAccount(MJSDK_PhpApiCommon_MJAcc_UserInfo mJAcc_UserInfo)
        {
            //走绑定操作
            MJSDK_PhpApiCommonLib_Account.accountLogin_mjacc(mJAcc_UserInfo, (MJSDK_PhpApiCommon_2Engine_account_login account_login) =>
            {
                MJSDK_PhpApiCommon_2SDK_user_accountBind mJSDK_PhpApiCommon_2SDK_User_AccountBind = new MJSDK_PhpApiCommon_2SDK_user_accountBind();
                mJSDK_PhpApiCommon_2SDK_User_AccountBind.token = _userInfo.token;
                mJSDK_PhpApiCommon_2SDK_User_AccountBind.account_token = account_login.account_token;
                //绑定操作
                MJSDK_PhpApiCommonLib_UserHandle.user_accountBind(mJSDK_PhpApiCommon_2SDK_User_AccountBind, (MJSDK_PhpApiCommon_2Engine_user_accountBind _bingInfo) =>
                {
                    //替换绑定数据
                    _userInfo.bind = _bingInfo.bind;
                    dealSucData(E_MJSDK_MJAcc_LoginType.BindAccount);
                }, (int errCode, string errMsg) =>
                {

                    dealFailData(E_MJSDK_MJAcc_LoginFailType.BindMJFail, errCode, errMsg);
                });
            }, (int errCode, string errMsg) =>
            {

                dealFailData(E_MJSDK_MJAcc_LoginFailType.MJAccAccountLoginFail, errCode, errMsg);
            });
        }

        /// <summary>
        /// 切换登录
        /// </summary>
        /// <param name="_mJAcc_UserInfo">梦加账号中心用户数据</param>
        private void swicthLogin(MJSDK_PhpApiCommon_MJAcc_UserInfo mJAcc_UserInfo)
        {
            //走第三方登录操作
            MJSDK_PhpApiCommonLib_Account.accountLogin_mjacc(mJAcc_UserInfo, (MJSDK_PhpApiCommon_2Engine_account_login account_login) =>
            {
                //生成登录参数
                MJSDK_PhpApiCommon_2SDK_user_login login_param = new MJSDK_PhpApiCommon_2SDK_user_login();
                login_param.account_token = account_login.account_token;
                login_param.afid = _loginParam.afid;
                login_param.login_params = _loginParam.login_params;
                login_param.nation = _loginParam.nation;
                //开始登录
                MJSDK_PhpApiCommonLib_UserHandle.user_login(login_param, (MJSDK_PhpApiCommon_2Engine_user_login mj_user_info) =>
                {
                    _userInfo = mj_user_info;
                    //保存mjToken
                    saveMJToken(_userInfo.user_id, mJAcc_UserInfo.mj_token);
                    dealSucData(E_MJSDK_MJAcc_LoginType.SwichAccount);
                }, (int errCode, string errMsg) =>
                {
                    dealFailData(E_MJSDK_MJAcc_LoginFailType.SwitchLogin, errCode, errMsg);
                });
            }, (int errCode, string errMsg) =>
            {
                dealFailData(E_MJSDK_MJAcc_LoginFailType.MJAccAccountLoginFail, errCode, errMsg);
            });
        }
        #endregion


        #region 数据处理
        /// <summary>
        /// 处理内部错误
        /// </summary>
        private void dealFailData(E_MJSDK_MJAcc_LoginFailType loginFailType, int errCode, string errMsg)
        {
            if (_failDelegate != null)
            {
                Hashtable err_ht = new Hashtable();
                err_ht.Add("msg", errMsg);
                err_ht.Add("code", errCode);
                _failDelegate((int)loginFailType, err_ht.toJson());
            }
            else
            {
                MJSDK_Log.mjsdkLog("失败回执对象未赋值，请检查", E_MJSDK_BusType.Error_BusiLoginErr);
            }

            //数据全部清除
            _mjTokenDic.Clear();
            cleanData();
        }

        /// <summary>
        /// 处理成功数据
        /// </summary>
        /// <param name="_LoginType"></param>
        /// <param name="_userInfo"></param>
        private void dealSucData(E_MJSDK_MJAcc_LoginType loginType)
        {
            if (_sucDelegate != null)
            {
                _sucDelegate(loginType, _userInfo);
            }
            else
            {
                MJSDK_Log.mjsdkLog("成功回执对象未赋值，请检查", E_MJSDK_BusType.Error_BusiLoginErr);
            }
            cleanData();
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        private void cleanData()
        {
            _gameLan = "";
            _sucDelegate = null;
            _failDelegate = null;
            _loginParam = null;
            _userInfo = null;
            //打开开关
            _isShowCenter = false;
        }
        #endregion


        #region 缓存token
        /// <summary>
        /// 获取缓存梦加token
        /// </summary>
        private string getMJToken(string userId)
        {
            //通过userId取出 MJToken
            if (_mjTokenDic.Keys.Contains(userId))
            {
                return _mjTokenDic[userId];
            }
            return "";
        }

        /// <summary>
        /// 缓存token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mjToken"></param>
        private void saveMJToken(string userId, string mjToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(mjToken))
            {
                MJSDK_Log.mjsdkLog(" saveMJToken err,reason:userId or mjToken is null!", E_MJSDK_BusType.Error_BusiLoginErr);
            }
            else
            {
                //包含的话，进行替换
                if (_mjTokenDic.Keys.Contains(userId))
                {
                    _mjTokenDic[userId] = mjToken;
                }
                else
                {
                    _mjTokenDic.Add(userId, mjToken);
                }
            }
        }
        #endregion
    }


    /// <summary>
    /// 从梦加账号获取数据成功方式
    /// </summary>
    public enum E_MJSDK_MJAcc_LoginType
    {
        None = 0,        //无任何操作
        BindAccount = 1, //绑定MJ账号     
        SwichAccount = 2,//切换账号
    }


    /// <summary>
    /// 从梦加账号获取数据方式失败类型
    /// </summary>
    public enum E_MJSDK_MJAcc_LoginFailType
    {
        None = 910000,//无
        FrequentOperations = 910001,//操作频繁
        GuestLoginFail = 910002,//获取游客信息失败     
        GetMJTokenFail = 910003,//获取梦加用户Token失败
        MJAccAccountLoginFail = 910004,//梦加账号登录失败
        BindMJFail = 910005,//当前账号绑定MJ账号失败
        SwitchLogin = 910006,//切换登录失败
        MJCenterBusFail = 910007,//梦加账号中心相关业务失败

        Exception_Err = 911000,//异常部分
    }
}
