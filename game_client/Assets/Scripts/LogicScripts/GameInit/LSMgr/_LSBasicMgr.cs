using UnityEngine;
using System.Collections.Generic;
using System;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 登录服务器操作时，需要在实例对象调用的相关操作接口声明对象
    /// </summary>
    public class _LSBasicMgr
    {
        /// <summary>
        /// 管理器状态枚举
        /// </summary>
        public enum ENPLSMgrState
        {
            NONE,
            LOGIN,
            ENTERED,
        }

        /// <summary>
        /// 登录服务器的信息存储类
        /// </summary>
        public class NPLSLoginServerInfo
        {
            public string connectIp;
            public int port;
            public int clientType;
            public string userName;
            public string password;
            public string customMsg;

            //延后开始登录的时长
            public float delayTime;

            public NPLSLoginServerInfo()
            {
                connectIp = string.Empty;
                port = 0;
                clientType = 0;
                userName = string.Empty;
                password = string.Empty;
                customMsg = string.Empty;

                delayTime = 0f;
            }
            public NPLSLoginServerInfo(string _connectIp, int _port, int _clientType, string _userName, string _password, string _customMsg)
            {
                connectIp = _connectIp;
                port = _port;
                clientType = _clientType;
                userName = _userName;
                password = _password;
                customMsg = _customMsg;

                delayTime = -1f;
            }
            public NPLSLoginServerInfo(string _connectIp, int _port, int _clientType, string _userName, string _password, string _customMsg, float _delayTime)
            {
                connectIp = _connectIp;
                port = _port;
                clientType = _clientType;
                userName = _userName;
                password = _password;
                customMsg = _customMsg;

                delayTime = _delayTime;
            }

            /// <summary>
            /// 从监听对象中获取连接信息
            /// </summary>
            /// <param name="_listener"></param>
            public void setInfo(NPLSClientListener _listener)
            {
                connectIp = _listener.getConnectIP();
                port = _listener.getConnectPort();
                clientType = _listener.getClientType();
                userName = _listener.getUserName();
                password = _listener.getUserPassword();
                customMsg = _listener.getCustomMsg();

                delayTime = 0f;
            }
        }

        //状态
        private ENPLSMgrState _m_eMgrState;
        private long _m_lStateSerialize;
        //所有LS服务器在处理的数据对象列表
        private List<NPLSClientListener> _m_lslListenerList;

        //最终使用的监听对象
        private NPLSClientListener _m_lsFinalUseListener;
        //最终使用的监听对象的登录信息
        private NPLSLoginServerInfo _m_lsiFinalLoginServerLoginInfo;
        //最终使用的登录信息是否有效
        private bool _m_bIsFinalInfoEnable;

        //登录成功处理
        private Action<NPLSClientListener, string> _m_dOnLoginSuc;
        //登录失败处理
        private Action _m_dOnLoginFail;
        //登录完结的时候的处理
        private Action _m_dOnLoginDone;

        protected _LSBasicMgr()
        {
            _m_eMgrState = ENPLSMgrState.NONE;
            _m_lStateSerialize = ALSerializeOpMgr.next();
            _m_lslListenerList = new List<NPLSClientListener>();
            _m_lsFinalUseListener = null;
            _m_lsiFinalLoginServerLoginInfo = new NPLSLoginServerInfo();
            _m_bIsFinalInfoEnable = false;

            _m_dOnLoginSuc = default(Action<NPLSClientListener, string>);
            _m_dOnLoginFail = default(Action);
            _m_dOnLoginDone = default(Action);
        }

        public long stateSerialize { get { return _m_lStateSerialize; } }
        public ENPLSMgrState mgrState { get { return _m_eMgrState; } }
        public NPLSClientListener finalLSListener { get { return _m_lsFinalUseListener; } }

        /// <summary>
        /// 注册回调
        /// </summary>
        /// <param name="_sucDelegate"></param>
        public void regSucDelegate(Action<NPLSClientListener, string> _sucDelegate)
        {
            if(null == _sucDelegate)
                return;

            _m_dOnLoginSuc += _sucDelegate;
        }
        public void regFailDelegate(Action _failDelegate)
        {
            if(null == _failDelegate)
                return;

            _m_dOnLoginFail += _failDelegate;
        }
        public void regDoneDelegate(Action _doneDelegate)
        {
            if(null == _doneDelegate)
                return;

            _m_dOnLoginDone += _doneDelegate;
        }

        public void unregSucDelegate(Action<NPLSClientListener, string> _sucDelegate)
        {
            if(null == _sucDelegate)
                return;

            _m_dOnLoginSuc -= _sucDelegate;
        }
        public void unregFailDelegate(Action _failDelegate)
        {
            if(null == _failDelegate)
                return;

            _m_dOnLoginFail -= _failDelegate;
        }
        public void unregDoneDelegate(Action _doneDelegate)
        {
            if(null == _doneDelegate)
                return;

            _m_dOnLoginDone -= _doneDelegate;
        }

        /// <summary>
        /// 尝试一次登录多个服务器
        /// </summary>
        /// <param name="_loginList"></param>
        public void tryLogin(List<NPLSLoginServerInfo> _loginList, Action<NPLSClientListener, string> _sucDelegate, Action _failDelegate)
        {
            //当已经登录游戏则不处理
            if(_m_eMgrState == ENPLSMgrState.ENTERED)
            {
                ALLog.Sys("在已经进入LS之后调用登录接口");
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            //判断状态尝试展示加载条
            if(_m_eMgrState == ENPLSMgrState.NONE)
                _m_eMgrState = ENPLSMgrState.LOGIN;

            //注册回调
            regSucDelegate(_sucDelegate);
            regFailDelegate(_failDelegate);

            NPLSLoginServerInfo tmpInfo = null;
            for(int i = 0; i < _loginList.Count; i++)
            {
                tmpInfo = _loginList[i];
                if(null == tmpInfo)
                    continue;

                NPLSClientListener listener = new NPLSClientListener(this, tmpInfo.connectIp, tmpInfo.port, tmpInfo.clientType, tmpInfo.userName, tmpInfo.password, tmpInfo.customMsg);
                //添加到数据集
                _regListener(listener);

                //调用登录函数
                if(tmpInfo.delayTime <= 0f)
                {
                    //下一帧调用，避免加一半插入数据
                    ALCommonTaskController.CommonActionAddNextFrameTask(() => { listener.login(1024 * 100);});
                }
                else
                {
                    ALCommonTaskController.CommonActionAddMonoTask(() => { listener.login(1024 * 100);}, tmpInfo.delayTime);
                }
            }
        }
        public void retryLogin(NPLSClientListener _retryListener)
        {
            //当已经登录游戏则不处理
            if(_m_eMgrState == ENPLSMgrState.ENTERED || _retryListener.lsMgrSerialize != _m_lStateSerialize)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"登录已经完成，取消登录服务器重试：{_retryListener.getConnectIP()}");
#endif
                return;
            }

            //创建新对象进行处理
            NPLSClientListener listener =
                new NPLSClientListener(this, _retryListener.getConnectIP(), _retryListener.getConnectPort(), _retryListener.getClientType(), _retryListener.getUserName(), _retryListener.getUserPassword(), _retryListener.getCustomMsg());
            //添加到数据集
            _regListener(listener);

            //下一帧调用，避免加一半插入数据
            ALCommonTaskController.CommonActionAddNextFrameTask(() => { listener.login(1024 * 100);});

            //调用失败接口
            onLoginFail(_retryListener);
        }

        /// <summary>
        /// 根据CDN配置的信息进行登录操作
        /// </summary>
        public void loginByServerInfo(_ILSLoginServerInfo _cdnInfo, int _clientType, string _userName, string _password, string _customMsg, Action<NPLSClientListener, string> _sucDelegate, Action _failDelegate)
        {
            if(null == _cdnInfo)
            {
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            //当已经登录游戏则不处理
            if(_m_eMgrState == ENPLSMgrState.ENTERED)
            {
                ALLog.Sys("在已经进入LS之后调用登录接口");
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            //创建登录列表
            List<NPLSLoginServerInfo> loginList = new List<NPLSLoginServerInfo>();

            string defaultIP = _cdnInfo.defaultIP;
            int defaultPort = _cdnInfo.defaultPort;

            //先增加默认登录服务器，次登录延迟0.1秒处理
            if (!string.IsNullOrEmpty(defaultIP))
            {
                NPLSLoginServerInfo defaultInfo =
                    new NPLSLoginServerInfo(defaultIP, defaultPort, _clientType, _userName, _password, _customMsg, _cdnInfo.defaultDelayLoginTime);

                //添加到队列
                loginList.Add(defaultInfo);
            }

            //尝试添加其他信息
            if(null != _cdnInfo.loopIPList)
            {
                for(int i = 0; i < _cdnInfo.loopIPList.Count; i++)
                {
                    //数据非法则跳出循环
                    if(i >= _cdnInfo.loopPortList.Count || i >= _cdnInfo.loopDelayLoginTimeList.Count)
                        break;

                    NPLSLoginServerInfo defaultInfo =
                        new NPLSLoginServerInfo(_cdnInfo.loopIPList[i], _cdnInfo.loopPortList[i], _clientType, _userName, _password, _customMsg, _cdnInfo.loopDelayLoginTimeList[i]);

                    //添加到队列
                    loginList.Add(defaultInfo);
                }
            }

            //开启登录处理
            tryLogin(loginList, _sucDelegate, _failDelegate);
        }
        
        /// <summary>
        /// 重新使用最后一次登录的信息再次登录LS
        /// </summary>
        public void reLoginLastLS(Action<NPLSClientListener, string> _sucDelegate, Action _failDelegate)
        {
            //当已经登录游戏则不处理，或者最后一次登录信息无效
            if(_m_eMgrState == ENPLSMgrState.ENTERED || !_m_bIsFinalInfoEnable)
            {
                ALLog.Sys("在已经进入LS之后调用登录接口");
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            //创建登录列表
            List<NPLSLoginServerInfo> loginList = new List<NPLSLoginServerInfo>();

            //添加到队列
            loginList.Add(_m_lsiFinalLoginServerLoginInfo);
            //使用过之后设置无效
            _m_bIsFinalInfoEnable = false;

            //开启登录处理
            tryLogin(loginList, _sucDelegate, _failDelegate);
        }

        /// <summary>
        /// 在登录处理失败的时候的处理函数
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_listener"></param>
        public void onLoginFail(NPLSClientListener _listener)
        {
            if(null == _listener)
                return;

            //判断序列号和状态
            if(_listener.lsMgrSerialize != _m_lStateSerialize)
                return;

            //状态如果是进入状态也不可处理
            if(_m_eMgrState == ENPLSMgrState.ENTERED)
                return;

            //判断数据是否在队列内
            if(!_m_lslListenerList.Remove(_listener))
                return;

            //断开连接
            _listener.logout();

            //判断队列是否为空，如果队列为空表示登录失败
            if(_m_lslListenerList.Count <= 0)
            {
                //调用失败处理
                _onLoginFail();
            }
        }

        /// <summary>
        /// 尝试设置最终成功的监听对象
        /// </summary>
        public bool trySetFinalListener(NPLSClientListener _listener, string _customRetMsg)
        {
            //判断序列号和状态
            if(_listener.lsMgrSerialize != _m_lStateSerialize)
                return false;

            //状态如果是进入状态也不可处理
            if(_m_eMgrState == ENPLSMgrState.ENTERED)
                return false;

            //遍历列表，将非本监听对象的其他连接都断开
            NPLSClientListener tmpListener = null;
            for(int i = 0; i < _m_lslListenerList.Count; i++)
            {
                tmpListener = _m_lslListenerList[i];
                if(null == tmpListener)
                    continue;

                //一致则不处理
                if(tmpListener == _listener)
                    continue;

                //断开连接
                tmpListener.logout();
            }
            //清空队列
            _m_lslListenerList.Clear();

#if UNITY_EDITOR
            Debug.LogError_EditorOnly_SysTip($"登录已经完成：{_listener.getConnectIP()}");
#endif
            //设置对象
            _m_lsFinalUseListener = _listener;
            //设置信息
            _m_lsiFinalLoginServerLoginInfo.setInfo(_m_lsFinalUseListener);
            _m_bIsFinalInfoEnable = true;
            //调用事件
            _onLoginSuc(_customRetMsg);

            return true;
        }

        /// <summary>
        /// 发送消息的通用函数
        /// </summary>
        /// <param name="_protocol"></param>
        public bool sendFinalLSMsg(_IALProtocolStructure _protocol)
        {
            if(null == _m_lsFinalUseListener)
                return false;

            _m_lsFinalUseListener.sendMes(_protocol);

            return true;
        }
        public bool sendFinalLSMsgByLog(_IALProtocolStructure _protocol)
        {
            if(null == _m_lsFinalUseListener)
                return false;

            _m_lsFinalUseListener.sendMesByLog(_protocol);

            return true;
        }

        /// <summary>
        /// 断开最后LS的链接
        /// </summary>
        public void logoutFinalLS()
        {
            if(null != _m_lsFinalUseListener)
                _m_lsFinalUseListener.logout();
            _m_lsFinalUseListener = null;
        }

        /// <summary>
        /// 重置登录状态，只有重置之后才可以执行登录操作
        /// </summary>
        public void resetLoginState()
        {
            for(int i = 0; i < _m_lslListenerList.Count; i++)
                _m_lslListenerList[i].logout();

            //还是要释放final
            if(null != _m_lsFinalUseListener)
                _m_lsFinalUseListener.logout();
            _m_lsFinalUseListener = null;

            _m_eMgrState = ENPLSMgrState.NONE;
            _m_lStateSerialize = ALSerializeOpMgr.next();
            _m_lslListenerList.Clear();
        }

        /// <summary>
        /// 在登录失败时的处理
        /// </summary>
        protected void _onLoginFail()
        {
            _m_eMgrState = ENPLSMgrState.NONE;
            _m_lStateSerialize = ALSerializeOpMgr.next();

            //調用函數
            Action preFail = _m_dOnLoginFail;
            _m_dOnLoginSuc = default(Action<NPLSClientListener, string>);
            _m_dOnLoginFail = default(Action);
            if(null != preFail)
                preFail();

            //调用完结函数
            Action preDone = _m_dOnLoginDone;
            _m_dOnLoginDone = null;
            if(null != preDone)
                preDone();
        }

        /// <summary>
        /// 在登录失败时的处理
        /// </summary>
        protected void _onLoginSuc(string _customRetMsg)
        {
            _m_eMgrState = ENPLSMgrState.ENTERED;
            _m_lStateSerialize = ALSerializeOpMgr.next();

            //調用函數
            Action<NPLSClientListener, string> preSuc = _m_dOnLoginSuc;
            _m_dOnLoginSuc = default(Action<NPLSClientListener, string>);
            _m_dOnLoginFail = default(Action);
            if(null != preSuc)
                preSuc(_m_lsFinalUseListener, _customRetMsg);

            //调用完结函数
            Action preDone = _m_dOnLoginDone;
            _m_dOnLoginDone = null;
            if(null != preDone)
                preDone();
        }

        /// <summary>
        /// 添加一个监听对象
        /// </summary>
        /// <param name="_listener"></param>
        protected void _regListener(NPLSClientListener _listener)
        {
            if(null == _listener)
                return;

            _m_lslListenerList.Add(_listener);
        }
        /// <summary>
        /// 添加一个监听对象
        /// </summary>
        /// <param name="_listener"></param>
        protected void _unregListener(NPLSClientListener _listener)
        {
            if(null == _listener)
                return;

            _m_lslListenerList.Remove(_listener);
        }
    }
}
