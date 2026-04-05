using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /****************
     * 登录服务器的连接对象
     **/
    public class NPLSClientListener : _AALBasicClient
    {
        private static int _m_iReconnectTime = 0;

        //LS服务器管理对象的操作序列号
        private long _m_lssLSMgrSerialize;
        //连接对象反向操作管理器的接口类
        private _LSBasicMgr _m_lsiLSMgrInterface;

        public NPLSClientListener(_LSBasicMgr _lsMgrInterface, string _connectIp, int _port, int _clientType, string _userName, string _password, string _customMsg)
            : base(_connectIp, _port, _clientType, _userName, _password, _customMsg)
        {
            if(null == _lsMgrInterface)
            {
                UnityEngine.Debug.LogError("带入的LSMgrInterface为空！");
                return;
            }

            _m_lssLSMgrSerialize = _lsMgrInterface.stateSerialize;
            _m_lsiLSMgrInterface = _lsMgrInterface;

#if UNITY_EDITOR
            Debug.LogError_EditorOnly_SysTip($"登入的 LS Ip: {_connectIp} : {_port}");
#endif
        }

        public long lsMgrSerialize { get { return _m_lssLSMgrSerialize; } }

        /****************
         * 接收消息的处理函数，此函数在接收线程中处理，如需要更好的处理方式则需要另开线程进行处理
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:48 PM
         */
        public override void receiveMes(byte[] _mes)
        {
            //调用协议分配对象进行处理
            WCGSingleton<NPLSProtocolDispather>.instance.DealProtocol(this, _mes);
        }

        /****************
         * 相关初始化失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:53 PM
         */
        public override void InitFail(DisconnectReason _reason)
        {
            //增加重试次数
            _m_iReconnectTime++;
            if(_m_iReconnectTime < 3)
            {
                //显示初始化连接失败
                Debug.LogError($"初始化连接{getConnectIP()}:{getConnectPort()}失败，1秒后重试登录！");
                //延迟1秒再连接
                ALCommonActionMonoTask.addMonoTask(() => { _m_lsiLSMgrInterface.retryLogin(this); }, 1f);
            }
            else
            {
                _m_iReconnectTime = 0;
                //设置登录失败
                _m_lsiLSMgrInterface.onLoginFail(this);
                //显示提示信息，确认后回到初次进入界面
                //NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage("#*initializefail_tip"), TextTranslate.instance.getLanguage("#1_confirm")
                //    , () =>
                //    {
                //    //延迟1秒再连接
                //    ALCommonActionMonoTask.addMonoTask(() => { login(getConnectIP(), getConnectPort(), getClientType(), getUserName(), getUserPassword(), getCustomMsg()); }, 1f);
                //    });
            }
        }
        /****************
         * 连接服务器失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:53 PM
         */
        public override void ConnectFail(DisconnectReason _reason)
        {
            //增加重试次数
            _m_iReconnectTime++;
            if(_m_iReconnectTime < 2)
            {
                //显示初始化连接失败
                Debug.LogError($"连接服务器失败，1秒后重试登录！, ip: {getConnectIP()}\tport:{getConnectPort()}");

                //延迟1秒再连接
                ALCommonActionMonoTask.addMonoTask(() => { _m_lsiLSMgrInterface.retryLogin(this); }, 1f);
            }
            else if(_m_iReconnectTime < 4)
            {
                //显示初始化连接失败
                Debug.LogError($"连接服务器失败，3秒后重试登录！, ip: {getConnectIP()}\tport:{getConnectPort()}");

                //延迟1秒再连接
                ALCommonActionMonoTask.addMonoTask(() => { _m_lsiLSMgrInterface.retryLogin(this); }, 3f);
            }
            else
            {
                _m_iReconnectTime = 0;
                //设置登录失败
                _m_lsiLSMgrInterface.onLoginFail(this);
                ////显示提示信息，确认后回到初次进入界面
                //NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage("#*connectfail_tip"), TextTranslate.instance.getLanguage("#1_confirm")
                //    , () =>
                //    {
                //    //进入另一视图
                //    NPGUISceneLogin.instance.enterScene();
                //        NPGUISceneLogin.instance.regInitDelegate(
                //            () =>
                //            {
                //            //退出登录等待窗口
                //            endLoginWait();
                //                NPGUISceneLogin.instance.enterView(UEWCGUISceneLoginViewEnum.INPUT_ACCOUNT);
                //            }
                //            );
                //    });
            }
        }
        /***************
         * 登录失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:03 PM
         */
        public override void LoginFail(DisconnectReason _reason)
        {
            /** 登录失败，清除登录缓存 */
            LoginTokenSetting.instance.clearLastLoginInfo();

            _m_iReconnectTime = 0;
            //设置登录失败
            _m_lsiLSMgrInterface.onLoginFail(this);

            ////判断是否白名单登录，是则弹出维护提示
            //if(getClientType() == (int)ENPLSGameClientType.USER_W_LIST
            //    || getClientType() == (int)ENPLSGameClientType.USER_CHECK_CODE_W_LIST)
            //{
            //    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage("#*sever_still_close"), TextTranslate.instance.getLanguage("$*confirm")
            //        , () =>
            //        {
            //        //进入另一视图
            //        NPGUISceneLogin.instance.enterScene();
            //            NPGUISceneLogin.instance.regInitDelegate(
            //                () =>
            //                {
            //                //退出登录等待窗口
            //                endLoginWait();
            //                    NPGUISceneLogin.instance.enterView(UEWCGUISceneLoginViewEnum.INPUT_ACCOUNT);
            //                }
            //                );
            //        });
            //}
            //else
            //{
            //    //登录失败
            //    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage("#*wrongusername_tip"), TextTranslate.instance.getLanguage("$*confirm")
            //        , () =>
            //        {
            //        //进入另一视图
            //        NPGUISceneLogin.instance.enterScene();
            //            NPGUISceneLogin.instance.regInitDelegate(
            //                () =>
            //                {
            //                //退出登录等待窗口
            //                endLoginWait();
            //                    NPGUISceneLogin.instance.enterView(UEWCGUISceneLoginViewEnum.INPUT_ACCOUNT);
            //                }
            //                );
            //        });
            //}
        }
        /***************
         * 登出操作
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:07 PM
         */
        public override void Disconnect(DisconnectReason _reason)
        {
            //设置登录失败
            _m_lsiLSMgrInterface.onLoginFail(this);
            ////断开连接
            //NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage("#*disconnect_tip"), TextTranslate.instance.getLanguage("$*confirm")
            //    , () =>
            //    {
            //    //延迟1秒再连接
            //    ALCommonActionMonoTask.addMonoTask(() => { login(getConnectIP(), getConnectPort(), getClientType(), getUserName(), getUserPassword(), getCustomMsg()); }, 1f);
            //    });
        }
        /***************
         * 登录成功
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:11 PM
         */
        public override void LoginSuc(string _customRetMsg)
        {
            //尝试设置为最终处理对象
            if(!_m_lsiLSMgrInterface.trySetFinalListener(this, _customRetMsg))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"设置为成功服务器失败，断开：{getConnectIP()} : {getConnectPort()}");
#endif
                //失败则直接断开
                logout();
                return;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError_EditorOnly_SysTip($"成功登陆服务器：{getConnectIP()} : {getConnectPort()}");
#endif

            }

            //setLoginWaitText(0.8f, TextTranslate.instance.getLanguage("#ui_sys_sign_suc"));
            //Debug.Log("LS Server Login Suc! code: " + _customRetMsg);
            ////增加用户数据
            //NPAccountMgr.instance.RecordUser(getUserName(), getUserPassword(), getCustomMsg());
            ////设置当前使用用户
            //NPAccountMgr.instance.setCurUseUser(getUserName());
            ////保存当前登录回调LoginToken
            //WCGLoginTokenSetting.instance.SaveLoginToken(getUserName(), _customRetMsg);
            ////如是作弊登录保存最后一次登录信息
            //if(getClientType() == 0 || getClientType() == (int)ENPLSGameClientType.CHEAT)
            //    NPAccountMgr.instance.setLastLoginTag(NPEnum.ENPLoginWayType.NONE, getUserName());
            ////发送消息初始化用户信息
            //sendMesByLog(NPLSWriter_001_LoginOp.make_001_001_ReqBasicInfo());
        }

        public void sendMesByLog(ALBasicProtocolPack._IALProtocolStructure _mes)
        {
            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if (_mes.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetSend(string.Format("<color=green>S -> C: {0} ; </color> {1}", _mes.GetType().Name, GCommon.GetInfoPropertys(_mes)));
                }
                else
                {
                    GCommon.NetWaring($"协议{_mes.GetType().Name}过大，大小：{_mes.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }

            sendMes(_mes);
        }

        /*******************
         * 初始化连接时调用的内部函数
         **/
        protected override bool _onInitConnection()
        {
            return true;
        }
    }
}
