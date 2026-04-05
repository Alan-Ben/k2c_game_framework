using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;

namespace GOE
{
    public class NPGSClientListener : _AALBasicClient
    {
        private static NPGSClientListener _g_instance;
        private static int _g_tryLoginCount = 0;
        private static bool _g_bLocalResLoadOver = false;
        private static string _g_reloginKey;
        private static ENPGSLoginType _g_eLoginType;


        public static NPGSClientListener instance
        {
            get
            {
                return _g_instance;
            }
        }

        /*************
         * 直接发送消息，不进行保存性记录
         **/
        public static void directSendMsg(_IALProtocolStructure _protocol)
        {
            //数据无效则不发送
            //TODO 延迟发送
            if(null == _g_instance)
                return;

            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if(_protocol.getMainOrder() == 1 && (_protocol.getSubOrder() == 2 || _protocol.getSubOrder() == 22))
                {
                    //心跳包不输出.
                }
                else if(_protocol.getMainOrder() == 10 && _protocol.getSubOrder() == 6)
                {
                    //心跳包不输出.
                }
                else
                {
                    if (_protocol.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                    {
                        GCommon.NetSend(string.Format("<color=red>C -> S: {0} ; </color> {1}", _protocol.GetType().Name, GCommon.GetInfoPropertys(_protocol)));
                    }
                    else
                    {
                        GCommon.NetWaring($"协议{_protocol.GetType().Name}过大，大小：{_protocol.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                    }
                }
            }
            
            _sendMsgNP(_protocol);
        }

        /*************
         * 发送消息的通用函数
         **/
        public static void sendRequest(_IALProtocolStructure _protocol, _INPRequestCallbackDealer _dealer)
        {
            //注册回调
            long serialize = NPRequestCallbackMgr.instance.regDealer(_dealer);

            _sendMsg(serialize, _protocol);
        }
        public static void sendRequestByLog(_IALProtocolStructure _protocol, _INPRequestCallbackDealer _dealer)
        {
            //注册回调
            long serialize = NPRequestCallbackMgr.instance.regDealer(_dealer);

            _sendMsgByLog(serialize, _protocol);
        }
        public static void sendMsg(_IALProtocolStructure _protocol)
        {
            _sendMsg(0, _protocol);
        }
        public static void sendMsgByLog(_IALProtocolStructure _protocol)
        {
            _sendMsgByLog(0, _protocol);
        }
        protected static void _sendMsg(long _clientSerialize, _IALProtocolStructure _protocol)
        {
            if(null == _protocol)
                return;

            //UnityEngine.Debug.LogError("send: " + _protocol.getMainOrder() + " - " + _protocol.getSubOrder() + " - "
            //    + (WCGGSMsgDealer.instance.getListFirstSendedMsgSerialize() + WCGGSMsgDealer.instance.sendBackMsgCount));
            //记录到消息处理器中
            int serialzie = NPGSMsgDealer.instance.addSendBackMsg(_clientSerialize, _protocol);

            //数据无效则不发送
            if(null == _g_instance || -1 == serialzie)
                return;

            _sendMsgNP(NPGSWriter_001_BasicOp.make_022_SendSerializeMsg(serialzie, _clientSerialize, _protocol.makeFullPackage()));
        }
        protected static void _sendMsgByLog(long _clientSerialize, _IALProtocolStructure _protocol)
        {
            if(null == _protocol)
                return;

            //UnityEngine.Debug.LogError("send: " + _protocol.getMainOrder() + " - " + _protocol.getSubOrder() + " - "
            //    + (WCGGSMsgDealer.instance.getListFirstSendedMsgSerialize() + WCGGSMsgDealer.instance.sendBackMsgCount));
            //记录到消息处理器中
            int serialzie = NPGSMsgDealer.instance.addSendBackMsg(_clientSerialize, _protocol);

            //数据无效则不发送
            if(null == _g_instance)
                return;

            if(Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if(_protocol.getMainOrder() == 1 && (_protocol.getSubOrder() == 2 || _protocol.getSubOrder() == 22))
                {
                    //心跳包不输出.
                }
                else if(_protocol.getMainOrder() == 10 && _protocol.getSubOrder() == 6)
                {
                    //心跳包不输出.
                }

#if AL_ILRUNTIME
                else if (_protocol is _IALProtocolStructureAdapter.Adapter _protocolAdapter)
                {
                    //ILR里的协议结构体本身会递归，所以不能直接用NPGCommon.GetInfoPropertys打印，不然会死循环
                    //ILR里的协议，先不输出具体字段（因为是runtimeType，直接用is比较，会不相等）
                    if (_protocol.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                    {
                        GCommon.NetSend(string.Format("[Hotfix]<color=green>C -> S: {0} ; </color> {1}", _protocolAdapter.ILInstance.Type.Name, _protocolAdapter.ToString()));
                    }
                    else
                    {
                        GCommon.NetSend(string.Format("[Hotfix]<color=red>C -> S: {0} ; </color> {1}", _protocolAdapter.ILInstance.Type.Name, _protocolAdapter.ToString()));
                    }
                }
#endif
                else
                {
                    if (_protocol.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                    {
                        GCommon.NetSend(string.Format("<color=red>C -> S: {0} ; </color> {1}", _protocol.GetType().Name, GCommon.GetInfoPropertys(_protocol)));
                    }
                    else
                    {
                        GCommon.NetWaring($"协议{_protocol.GetType().Name}过大，大小：{_protocol.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                    }
                }
            }

            _sendMsgNP(NPGSWriter_001_BasicOp.make_022_SendSerializeMsg(serialzie, _clientSerialize, _protocol.makeFullPackage()));
        }

        internal static void setReloginKey(string _key)
        {
            _g_reloginKey = _key;
        }

        public static void login(string _connectIp, int _port, string _uid, string _token)
        {
            if(null != _g_instance)
            {
                _g_instance.logout();
                _g_instance = null;
            }
            _g_eLoginType = ENPGSLoginType.NORMAL;
            _g_instance = new NPGSClientListener(_connectIp, _port, _g_eLoginType, _uid, _token);

            //正常登录先重置消息处理对象
            NPGSMsgDealer.resetMsgDealer();

            _g_instance.login(500 * 1024);
        }
        public void reLogin()
        {
            if(null != _g_instance)
            {
                _g_instance.logout();
                _g_instance = null;
            }

            _g_eLoginType = ENPGSLoginType.WITH_RELOGIN_KEY;
            _g_instance = new NPGSClientListener(getConnectIP(), getConnectPort(), _g_eLoginType, UID, ReloginKey);
            //后续登录流程不进行修改沿用旧的注册流程，避免重复处理导致其他问题的发生
            _g_instance.login(500 * 1024);
        }

        //相关登录信息存储变量
        private string uid;
        private string token;

        private bool _m_bIsEnable;
        private bool _m_bLogined;

        //记录本连接对应的用户数据对象，如果序列号不一致则不处理
        private long _m_lCurNPPlayerSerialize;

        protected NPGSClientListener(string _connectIp, int _port, ENPGSLoginType _clientType, string _uid, string _token)
            : base(_connectIp, _port, (int)_clientType, _uid.ToString(), _token, Game.instance.initGSJsonData())
        {
            uid = _uid;
            token = _token;

#if UNITY_EDITOR
            Debug.LogError_EditorOnly_SysTip($"登入的 GS Ip: {_connectIp} : {_port}");
#endif

            _m_bIsEnable = true;

            _m_lCurNPPlayerSerialize = 0;
        }

        public string UID { get { return uid; } }
        public string Token { get { return token; } }

        public bool isLogined { get { return _m_bLogined; } }

        public static string ReloginKey { get { return _g_reloginKey; } }

        public override void receiveMes(byte[] _msg)
        {
            if(!_m_bIsEnable)
                return;

            //如果无数据则先修正
            if (_m_lCurNPPlayerSerialize == 0 && null != NPPlayer.instance)
                _m_lCurNPPlayerSerialize = NPPlayer.instance.npplayerSerialize;

            //判断序列号是否一致，不一致说明已经重登，则不处理
            if (null != NPPlayer.instance && _m_lCurNPPlayerSerialize != NPPlayer.instance.npplayerSerialize)
                return;

            //判断是否被非安全消息处理
            if(WCGSingleton<NPGSUnsafeProtocolDispather>.instance.DealProtocol(this, _msg))
                return;

            //累加收到的消息数量
            //WCGGSMsgDealer.instance.addReceivedMsgCount();

            //处理消息
            GSProtocolDispather.instance.DealProtocol(this, _msg);
            //UnityEngine.Debug.LogError(_msg[0] + " - " + _msg[1] + " - " + WCGGSMsgDealer.instance.getReceivedMsgCount());

            //发送对应消息处理统计
            sendMes(NPGSWriter_001_BasicOp.make_021_ReceivedMsg(NPGSMsgDealer.instance.getReceivedMsgCount()));
        }

        public override void InitFail(DisconnectReason _reason)
        {
#if UNITY_EDITOR
            Debug.LogError("初始化 gate server失败！");
#endif
            _g_instance = null;
            _m_bIsEnable = false;

            ALCommonActionMonoTask.addMonoTask(() => { login(getConnectIP(), getConnectPort(), UID, Token); }, 3f);
        }

        public override void ConnectFail(DisconnectReason _reason)
        {
            //发送埋点-登录GS服务器失败
            GCommon.sendStepReport(TraceConst.LOGIN_GS_FAIL.setMarkParam(_reason,getConnectIP(),getConnectPort()));

#if UNITY_EDITOR
            Debug.LogError("链接 gate server失败！");
#endif
            _g_instance = null;
            _m_bIsEnable = false;

            if(_g_tryLoginCount < 10)
            {
                _g_tryLoginCount++;
#if UNITY_EDITOR
                Debug.LogError(string.Format("第 {0} 次尝试登录 gate server", _g_tryLoginCount));
#endif
                if(_g_eLoginType == ENPGSLoginType.NORMAL)
                {
                    ALCommonActionMonoTask.addMonoTask(() => { login(getConnectIP(), getConnectPort(), UID, Token); }, 3f);
                }
                else
                {
                    ALCommonActionMonoTask.addMonoTask(() => { reLogin(); }, 3f);
                }

            }
            else
            {
                //登录失败，放弃登录
                if(NPGGUIAddSceneReconnecting.instance.isEntered)
                    NPGGUIAddSceneReconnecting.instance.quitScene();

                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.reconnect_tip), TextTranslate.instance.getLanguage(TransKeyConst.reconnect_confirm), () =>
                {
                    Game.instance.relogin();
                });
            }

        }

        public override void LoginFail(DisconnectReason _reason)
        {
            //发送埋点-登录GS服务器失败
            GCommon.sendStepReport(TraceConst.LOGIN_GS_FAIL.setMarkParam(_reason, getConnectIP(), getConnectPort()));

#if UNITY_EDITOR
            Debug.LogError("登录   gate server 失败！");
#endif
            _g_instance = null;
            _m_bIsEnable = false;

            //登录失败，放弃登录
            if(NPGGUIAddSceneReconnecting.instance.isEntered)
                NPGGUIAddSceneReconnecting.instance.quitScene();


            //如果是在正常连接端口后的重登行为，直接尝试进入服务器
            if (getClientType() == (int)ENPGSLoginType.WITH_RELOGIN_KEY)
            {
                //这里直接重试登录
                Game.instance.reloginByDefault();
            }
            else
            {
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.relogin_fail_tip), TextTranslate.instance.getLanguage(TransKeyConst.login_confirm), () =>
                {
                    Game.instance.relogin();
                });
            }
        }

        public override void Disconnect(DisconnectReason _reason)
        {
#if UNITY_EDITOR
            Debug.LogError("断开 gate server ！");
#endif
            _g_instance = null;
            _m_bIsEnable = false;

#if UNITY_EDITOR
            Debug.LogError("尝试断线重连...");
#endif
            //已经登录过且还没有打开菊花窗口，打开菊花窗口
            if(_m_bLogined && !NPGGUIAddSceneReconnecting.instance.isEntered)
                NPGGUIAddSceneReconnecting.instance.enterScene();
            ALCommonActionMonoTask.addMonoTask(() => { reLogin(); }, 0.1f);

            //设置未登录成功
            _m_bLogined = false;
        }

        public override void LoginSuc(string _customRetMsg)
        {
            //发送埋点-登录GS服务器成功
            GCommon.sendStepReport(TraceConst.LOGIN_GS_SUC.setMarkParam(getConnectIP(), getConnectPort(), _customRetMsg));

#if UNITY_EDITOR
            Debug.Log("登录 gate server 成功！");
#endif
            _m_bIsEnable = true;
            _g_tryLoginCount = 0;
            if(!_m_bLogined)
                _m_bLogined = true;

            //登录成功关闭菊花
            if (NPGGUIAddSceneReconnecting.instance.isEntered)
                NPGGUIAddSceneReconnecting.instance.quitScene();

            //记录序列号
            _m_lCurNPPlayerSerialize = null == NPPlayer.instance ? 0 : NPPlayer.instance.npplayerSerialize;

            //调用登录流程管理对象事件
            GameInit_LoginProcess.instance.onGSLoginSuc();
            
            
            //如果是在正常连接端口后的重登行为，直接尝试进入服务器
            if(getClientType() == (int)ENPGSLoginType.WITH_RELOGIN_KEY)
            {
                //这里直接调用流程的申请进入
                GameInit_SelectServer.instance.reqResumeUs();
            }
        }

        protected override bool _onInitConnection()
        {
            return true;
        }

        internal static void onLocalResLoadOver()
        {
            _g_bLocalResLoadOver = true;

            //所有本地资源加载完成后的处理

            //扩展字体
            if(null != Game.instance.commonFont)
            {
                Game.instance.commonFont.RequestCharactersInTexture(GGameCommonInfo.instance.obj.initFontTextStr);
            }
        }
        public static bool IsLocalResLoadOver
        {
            get
            {
                return _g_bLocalResLoadOver;
            }
        }
        
        /// <summary>
        /// 可以延迟发送消息
        /// </summary>
        /// <param name="_mes"></param>
        private static void _sendMsgNP(ALBasicProtocolPack._IALProtocolStructure _mes)
        {
#if UNITY_EDITOR
            if (Game.instance.mainCamera.gameSetting.isSendMsgDelay)
            {
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (_g_instance != null)
                    {
                        _g_instance.sendMes(_mes);
                    }
                }, 1f);
            }
            else
            {
                _g_instance.sendMes(_mes);
            }
            
            if (null != _mes && _mes.GetFullPackBufSize() > Game.instance.mainCamera.gameSetting.protocolErrorMinSize)
            {
                Debug.LogError($"协议大小超过20k: {_mes.GetType().Name}，大小：{_mes.GetFullPackBufSize()}");
            }
#else
            _g_instance.sendMes(_mes);
#endif
        }
    }
}
