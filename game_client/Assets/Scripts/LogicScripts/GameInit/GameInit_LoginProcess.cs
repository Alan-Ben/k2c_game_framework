using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;
using NPLS2GC.p001_BasicOp;

namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GameInit_LoginProcess : _AGameInitProcess, _INPPGUILoadingBkProcessRefresher
    {
        private static GameInit_LoginProcess _g_instance = new GameInit_LoginProcess();
        public static GameInit_LoginProcess instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_LoginProcess();

                return _g_instance;
            }
        }

        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;
        //当前的处理过程对象
        private ALProcess _m_pProcessObj;

        //最后一次记录的下载信息
        private string _m_sLastDownloadProcessMsg;

        //完成的处理函数
        private Action _m_dDoneDelegate;

        private static readonly string _m_gProcessName = "login_process";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_LoginProcess()
        {
            _m_pAllResProcess = new ALProcessSubNode();
            _m_pProcessObj = null;

            _m_sLastDownloadProcessMsg = string.Empty;

            _m_dDoneDelegate = null;
        }

        public string lastDownloadProcessMsg { get { return _m_sLastDownloadProcessMsg; } }

        /// <summary>
        /// 获取当前操作的文本
        /// </summary>
        public string curOPTxt { get { return _m_sLastDownloadProcessMsg; } }
        /// <summary>
        /// 刷新间隔
        /// </summary>
        public float refreshDuration { get { return 1f; } }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                _m_pAllResProcess };
        }

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected override void _resetData()
        {
            //重置回调，避免init调用的时候回调被处理
            _m_dDoneDelegate = null;

            //整体初始化流程重置，需要断开玩家的相关服务器连接
            //中断过程对象
            if (null != _m_pProcessObj)
                _m_pProcessObj.stopProcess();
            _m_pProcessObj = null;

            //重置进度对象
            _m_pAllResProcess.reset();

            //需要退出相关连接对象
            LSMgr.instance.resetLoginState();
        }

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected override void _dealInit(Action _doneDelegate)
        {
            //设置完成处理函数，如果原来有值先调用掉
            if(null != _m_dDoneDelegate)
                _m_dDoneDelegate();

            //替换回调值，此回调会在正式进入GS的事件中触发
            _m_dDoneDelegate = _doneDelegate;

            //使用底层Process进行初始化流程
            _m_pProcessObj = ALProcess.CreateProcess(_m_gProcessName);

            //首先进行初始化步骤处理
            _m_pProcessObj
                //初始化账号配置
                .addProcess(InternalAccountMgr.instance.init)
                //初始化登录token信息
                .addProcess(LoginTokenSetting.instance.init)

                //进入作弊登录窗口进行处理
                .addProcess(_tryLogin);

            //开始执行
            _m_pProcessObj.dealProcess(new GameInitMonitor(_m_gProcessName, 0));
        }

        /// <summary>
        /// 展示登录方式窗口,不同的登入方式登入
        /// </summary>
        public void otherWayLogin()
        {
            if (!Game.instance.mainCamera.isUseSDK)
            {
                //未使用sdk直接作弊登入方式登入
                doCheatLogin();
            }
            else
            {
                //使用SDK登录
                SDKMgr.instance.quickLogin(_result =>
                {
                    if (_result == null)
                    {
                        Debug.LogError($"SDK登录失败，_result = null");
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_disconnect_tip),
                            TextTranslate.instance.getLanguage(TransKeyConst.login_confirm),
                            otherWayLogin);
                    }
                    else
                    {
                        //进行用户登录操作
                        loginLS(ENPLSGameClientType.SDK, CDNSetting_LoginServerUrlInfo.instance, _result.user_id, _result.token, Game.instance.initLSJsonData());
                    }
                }, (_code, _msg) =>
                {
                    Debug.LogError($"SDK登录失败，code:{_code},msg:{_msg}");
                    _dealSDKLoginFailed();
                });
                
                // //弹出选择登录方式的界面
                // NPGameInit_GameResInit.instance.init(() =>
                // {
                //     NPQueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndLoginWay.instance, NPUINodeTagConst.C_Login_AllWay, false);
                // });   
            }
        }

        //sdk登录失败处理
        private void _dealSDKLoginFailed()
        {
            //sdk登录失败，弹窗重新登录
            NPMesMgr.instance.showOneBtnMes(
                TextTranslate.instance.getLanguage(TransKeyConst.login_loginFailedTipContent_none),
                TextTranslate.instance.getLanguage(TransKeyConst.login_loginFailedTipBtn_none),
                () =>
                {
                    //如果资源还未初始化，则不弹选择账号弹窗，直接重新登录
                    if (GameInit_GameResInit.instance.isDone)
                    {
                        NPGGUIWndLoginWay.instance.setInfo(_dealSDKLoginFailed, false);
                        QueueMgr.instance.addNode_Login_MainUIAddWnd(NPGGUIWndLoginWay.instance, UINodeTagConst.C_Login_AllWay);
                    }
                    else
                    {
                        otherWayLogin();
                    }
                },
                true,
                TextTranslate.instance.getLanguage(TransKeyConst.login_loginFailedTipTitle_none));
        }

        /// <summary>
        /// 尝试登录服务器
        /// </summary>
        protected void _tryLogin()
        {
            //判断最后logintoken是否有效，有效则直接通过token登录
            if(!string.IsNullOrEmpty(LoginTokenSetting.instance.GetLoginUID())
                 && !string.IsNullOrEmpty(LoginTokenSetting.instance.GetLoginToken()))
            {
                //发送埋点-开始token登录LS
                GCommon.sendStepReport(TraceConst.START_TOKEN_LOGIN.setMarkParam(LoginTokenSetting.instance.GetLoginUID(), LoginTokenSetting.instance.GetLoginToken()));
                //直接使用登录处理
                loginLS(ENPLSGameClientType.USER_CHECK_CODE, CDNSetting_LoginServerUrlInfo.instance, LoginTokenSetting.instance.GetLoginUID(), LoginTokenSetting.instance.GetLoginToken(), "");
            }
            else
            {
                //没token走正常渠道登入方式
                otherWayLogin();
            }

            //重置变量
            _m_pProcessObj = null;
        }

        /// <summary>
        /// 处理自动登录操作，默认使用内部的账号系统处理
        /// </summary>
        public void doCheatLogin()
        {
            GameInit_GameResInit.instance.init(() =>
            {
                //这里直接显示登录窗口
                QueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndUseAccount.instance, UINodeTagConst.C_Login_Account, false
                    , false,null, () =>
                    {
                        if (!string.IsNullOrEmpty(InternalAccountMgr.instance.lastLoginName))
                        {
                            InternalAccountInfo accInfo = InternalAccountMgr.instance.getAccountInfo(InternalAccountMgr.instance.lastLoginName);
                            if (null != accInfo)
                            {
                                //设置账号信息
                                NPGGUIWndUseAccount.instance.setAccount(accInfo);
                            }
                        }

                        //设置进入回调处理
                        NPGGUIWndUseAccount.instance.setEnterDelegate(
                            ()=> { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_Account); });
                    });
            });
        }

        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="_process"></param>
        protected void _setProcess(float _process)
        {
            _m_pAllResProcess.setProcess(_process);
        }

        #region 登录服务器的批量登录操作接口

        /// <summary>
        /// 使用对应的帐号登录
        /// </summary>
        /// <param name="_clientType"></param>
        /// <param name="_account"></param>
        /// <param name="_password"></param>
        /// <param name="_customMsg"></param>
        public void loginLS(ENPLSGameClientType _clientType, _ILSLoginServerInfo _loginServerInfo, string _account, string _password, string _customMsg)
        {
            if(null == _loginServerInfo)
                return;
            
            //根据平台查询对应Ip连接
            WCGPlatLoginInfo platInfo = Game.instance.mainCamera.platInfo;
            if (null == platInfo)
            {
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_invalid_platform_data_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }

            //重置登录信息
            LSMgr.instance.resetLoginState();

            //设置进度
            setProcess(0.4f, TextTranslate.instance.getLanguage(TransKeyConst.login_signing_none));//#ui_sys_sign_ing

            //获取完登入需要的数据后再登入
            _loginServerInfo.reqDataDone(() =>
                {
                    //登入ls
                    LSMgr.instance.loginByServerInfo(_loginServerInfo, (int)_clientType, _account, _password, _customMsg,
                        _onLoginSuc,
                        () =>
                        {
                            //发送埋点-登录LS失败
                            GCommon.sendStepReport(TraceConst.LOGIN_LS_FAIL.setMarkParam(_loginServerInfo.defaultIP, _loginServerInfo.defaultPort));

                            switch (_clientType)
                            {
                                //服务器自带token登入失败不twoBtn提示弹窗，就是有可能失败的，直接给他重试
                                case ENPLSGameClientType.USER_CHECK_CODE:
                                    otherWayLogin();
                                    break;
                                //其他登入失败说明真失败，需要先twoBtn弹窗提示是否需要重试
                                default:
                                    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_disconnect_tip), TextTranslate.instance.getLanguage(TransKeyConst.login_confirm), otherWayLogin);
                                    break;
                            }
                        });
                });
        }

        /// <summary>
        /// 登录成功的处理
        /// </summary>
        /// <param name="_clientListener"></param>
        /// <param name="_customRetMsg"></param>
        protected void _onLoginSuc(NPLSClientListener _clientListener, string _customRetMsg)
        {
            //发送埋点-登录LS成功
            GCommon.sendStepReport(TraceConst.LOGIN_LS_SUC.setMarkParam(_clientListener.getConnectIP(), _clientListener.getConnectPort()));

            setProcess(0.6f, TextTranslate.instance.getLanguage(TransKeyConst.login_signsucc_none));//#ui_sys_sign_suc
            Debug.Log("LS Server Login Suc! code: " + _customRetMsg);

            //增加用户数据
            InternalAccountMgr.instance.RecordUser(_clientListener.getUserName(), _clientListener.getUserPassword(), _clientListener.getCustomMsg());
            //设置当前使用用户
            InternalAccountMgr.instance.setCurUseUser(_clientListener.getUserName());
            //保存当前登录回调LoginToken
            LoginTokenSetting.instance.SaveLoginToken(_clientListener.getUserName(), _customRetMsg);

            //如是作弊登录保存最后一次登录信息
            if (_clientListener.getClientType() == 0 || _clientListener.getClientType() == (int)ENPLSGameClientType.CHEAT)
                InternalAccountMgr.instance.setLastLoginTag(NPEnum.ENPLoginWayType.NONE, _clientListener.getUserName());

            //发送消息初始化用户信息
            _clientListener.sendMesByLog(NPLSWriter_001_LoginOp.make_001_001_ReqBasicInfo());
        }

        /// <summary>
        /// 当LS001_001协议返回，初始化用户信息返回
        /// </summary>
        /// <param name="_listener"></param>
        /// <param name="_msg"></param>
        public void onLSRetBasicInfo(NPLSClientListener _listener, NPLS2GC_001_001_RetBasicInfo _msg)
        {
            if(null == _listener || null == _msg)
                return;
            
            //设置信息对象的UId
            Game.instance.setUid(_msg.getUid());

            //设置最新流程操作数据UID
            GameInit_SelectServer.instance.setUid(_msg.getUid());

            //发送协议进入游戏
            _listener.sendMesByLog(NPLSWriter_001_LoginOp.make_001_002_EnterGame());
        }
        
        /// <summary>
        /// 获取LS返回的登录信息时的处理, 001_002返回
        /// </summary>
        public void onGetLSEnterGameRes(NPLS2GC_001_002_EnterGameRes _msg)
        {
            //账号信息管理对象
            InternalAccountMgr.instance.setLastLoginInfo(_msg.getUid(), _msg.getCheckCode());
            //输出log
            Debug.Log_EditorOnly("Get Uid: " + _msg.getUid() + " CheckCode: "
                + _msg.getCheckCode() + " ServerIp: " + _msg.getGateServerIp() + " ServerPort: " + _msg.getGateServerPort());

            setProcess(0.8f, string.Empty);
            //断开登录服务器连接
            LSMgr.instance.logoutFinalLS();

            //设置是否是白名单
            GameSetting.instance.setIsWhite(_msg.getIsWhite());
            
            //发送埋点-开始登录GS
            GCommon.sendStepReport(TraceConst.START_LOGIN_GS);

            //1. 玩家登陆LoginServer，验证账号
            //2. 验证成功，获取对应GateServer连接信息(_msg.getCheckCode())，
            //3. 登陆到GateServer
            //记录连接信息
            NPGSClientListener.login(_msg.getGateServerIp(), _msg.getGateServerPort(), _msg.getUid(), _msg.getCheckCode());

            //初始化condition prefab刷新对象
            NPGSubPrefabActionMgr.instance.Init();
        }

        private int _m_iPingSerialize = 0;
        private int _m_iPingRound = 0;
        /// <summary>
        /// 在GS登录成功时处理
        /// </summary>
        public void onGSLoginSuc()
        {
            //设置进度条
            setProcess(1f, string.Empty);

            //调用回调
            Action doneDelegate = _m_dDoneDelegate;
            _m_dDoneDelegate = null;
            if (null != doneDelegate)
                doneDelegate();

            //发送第三方埋点-用户登录游戏服务器成功-login
            GCommon.sendAllThirdCustomEvent(EThirdCustomEventType.LOGIN);

            //设置ping值序列号
            _m_iPingSerialize = ALSerializeOpMgr.next();
            int pingSerialize = _m_iPingSerialize;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (pingSerialize != _m_iPingSerialize)
                    return;
                _m_iPingRound++;
                //发送埋点-游戏平均PING值
                GCommon.sendStepReport(TraceConst.AVERAGE_PING.setMark($"游戏平均PING值，IP:{NPGSClientListener.instance?.getConnectIP()}，Port:{NPGSClientListener.instance?.getConnectPort()}，round:{_m_iPingRound}，max:{FpsAndPingMgr.instance.maxPingMS}，avg:{FpsAndPingMgr.instance.avgPingMS}"));
            }, 120f);
        }
#endregion

#region 登录进度处理函数
        
        //设置过程进度的处理
        public void setProcess(float _process, string _showText)
        {
            //设置进度条信息
            _m_pAllResProcess.setProcess(_process);

            //设置文本
            _m_sLastDownloadProcessMsg = _showText;
        }

#endregion
    }
}
