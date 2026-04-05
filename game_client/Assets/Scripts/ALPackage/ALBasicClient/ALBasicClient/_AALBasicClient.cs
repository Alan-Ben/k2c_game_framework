using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

using ALBasicProtocolPack;

namespace ALPackage
{
    /***********
     * 状态枚举
     **/
    public enum ClientStat
    {
        NONE,
        INIT,
        CONNECTED,
        LOGIN,
        DISCONNECTED,
    }

    /***********
     * 断开连接的原因枚举
     **/
    public enum DisconnectReason
    {
        NONE,
        DATA_EMPTY,
        READING_ERROR,
        READING_VERIFY_ERROR,
        DISCONNECTED,
        INIT_FAIL,
    }

    /***********************
     * 基本客户端连接类
     **/
    public abstract class _AALBasicClient : _IALProtocolDealer
    {
        private static int _g_serialize = 1;

        private int _m_iSerialize;
        /** 连接ID */
        private long _m_lSocketID;
        /** 连接服务器的相关信息 */
        private string _m_sConnectIP;
        private int _m_iConnectPort;
        private int _m_iClientType;
        private string _m_sUserName;
        private string _m_sUserPassword;
        private string _m_sCustomMsg;

        /** 是否退出线程 */
        private bool _m_bExit;
        /** 连接使用的线程对象 */
        private Thread _m_tConnectThread;
        private Thread _m_tSendThread;
        /** 使用TCP连接服务器 */
        private TcpClient _m_tcConnectClient;
        /** 客户端状态 */
        private ClientStat _m_eClientStat;
        /** Socket连接的缓存Buf长度 */
        private int _m_iBufLen;

        /// <summary>
        /// 是否允许发送消息，这个标记位需要在创建完发送线程之后设置为有效
        /// 为了避免在创建对象之后马上调用了发送，导致部分消息在发送线程创建之前就加入并进行了发送
        /// </summary>
        private bool _m_bCanSendMes;
        //记录是否本对象主动断开连接，因为如果是本对象主动断开不一定会触发read_error错误，可能进入登录错误的流程。此时不会触发重新连接，而是直接重新登录
        private bool _m_bIsClientLogout;
        //非法发送的消息队列，用于在非法情况下临时缓存的处理，保证消息可以按照顺序发送
        private List<byte[]> _m_lInlegallMes;
        /** 需要的发送消息队列 */
        private List<byte[]> _m_lSendMesList;
        /** 发送消息队列的信号量 */
        private ManualResetEvent _m_evSendMesEvent;

        /** 已经接收到的消息队列 */
        private List<byte[]> _m_lReceivedMesList;

        public _AALBasicClient(string _connectIP, int _connectPort, int _clientType, string _username, string _userPassword, string _customMsg)
        {
            _m_iSerialize = _g_serialize++;
            _m_sConnectIP = _connectIP;
            _m_iConnectPort = _connectPort;
            _m_iClientType = _clientType;
            _m_sUserName = _username;
            _m_sUserPassword = _userPassword;
            _m_sCustomMsg = _customMsg;

            _m_bExit = false;
            _m_tConnectThread = null;
            _m_tSendThread = null;
            _m_tcConnectClient = null;
            _m_eClientStat = ClientStat.NONE;
            _m_iBufLen = 0;

            _m_bCanSendMes = false;
            _m_bIsClientLogout = false;
            _m_lInlegallMes = null;
            _m_lSendMesList = new List<byte[]>();
            _m_evSendMesEvent = new ManualResetEvent(false);
            _m_lReceivedMesList = new List<byte[]>();

            //开启每帧检查对象
            ALMonoTaskMgr.instance.addNextFrameTask(new ALBasicClientMesDealMonoTask(this));
        }
        public _AALBasicClient(_AALBasicClient _obj)
        {
            _m_iSerialize = _g_serialize++;
            _m_sConnectIP = _obj._m_sConnectIP;
            _m_iConnectPort = _obj._m_iConnectPort;
            _m_iClientType = _obj._m_iClientType;
            _m_sUserName = _obj._m_sUserName;
            _m_sUserPassword = _obj._m_sUserPassword;
            _m_sCustomMsg = _obj._m_sCustomMsg;

            _m_bExit = false;
            _m_tConnectThread = null;
            _m_tSendThread = null;
            _m_tcConnectClient = null;
            _m_eClientStat = ClientStat.NONE;
            _m_iBufLen = 0;

            _m_bCanSendMes = false;
            _m_bIsClientLogout = false;
            _m_lInlegallMes = null;
            _m_lSendMesList = new List<byte[]>();
            _m_evSendMesEvent = new ManualResetEvent(false);
            _m_lReceivedMesList = new List<byte[]>();

            //开启每帧检查对象
            ALMonoTaskMgr.instance.addNextFrameTask(new ALBasicClientMesDealMonoTask(this));
        }

        public int serialzie { get { return _m_iSerialize; } }
        public long getSocketID() { return _m_lSocketID; }
        public string getConnectIP() { return _m_sConnectIP; }
        public int getConnectPort() { return _m_iConnectPort; }
        public string getUserName() { return _m_sUserName; }
        public string getUserPassword() { return _m_sUserPassword; }
        public int getClientType() { return _m_iClientType; }
        public string getCustomMsg() { return _m_sCustomMsg; }

        public TcpClient getConnectClient() { return _m_tcConnectClient; }

        public ClientStat getClientStat() { return _m_eClientStat; }
        public int getBufLen() { return _m_iBufLen; }

        public bool isExit { get { return _m_bExit; } }

        /**********************
         * 登录操作函数
         **/
        public void login()
        {
            login(1048576);
        }
        public void login(int _socketBufSize)
        {
            //如果还没调用login之前，已经调用了logout，就不能再login了
            if (_m_bExit)
                return;
            //不允许本对象进行2次连接
            if(null != _m_tConnectThread)
                return ;

            //创建缓存BUF
            _m_iBufLen = _socketBufSize;
            //开启线程对象执行对应操作
            _m_tConnectThread = new Thread(_receiveThreadFun);
            _m_tConnectThread.Priority = ThreadPriority.Highest;

            //开启线程
            _m_tConnectThread.Start();
        }

        /*******************
         * 开启发送消息的线程
         **/
        public void initSendThread()
        {
            //开启消息发送线程
            _m_tSendThread = new Thread(_sendThreadFun);
            _m_tSendThread.Priority = ThreadPriority.Highest;

            //开启线程
            _m_tSendThread.Start();
        }

        /***************
         * 主动断开连接的操作
         **/
        public void logout()
        {
            //设置主动退出状态
            _m_bExit = true;

            try
            {
                if(null != _m_tcConnectClient)
                    _m_tcConnectClient.Close();
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogError($"Connect : {_m_sConnectIP}:{_m_iConnectPort} Close Fail: {e}");
            }

            //设置连接对象无效
            _m_tcConnectClient = null;
        }

        /// <summary>
        /// 立即马上断开客户端链接
        /// 此操作会引发相关的错误事件
        /// </summary>
        public void dealClientLogout()
        {
            try
            {
                //设置操作标记
                _m_bIsClientLogout = true;
                if (null != _m_tcConnectClient)
                    _m_tcConnectClient.Dispose();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Connect : {_m_sConnectIP}:{_m_iConnectPort} Close Fail: {e}");
            }
        }

        /*******************
         * 发送消息的函数
         **/
        public void sendMes(ALBasicProtocolPack._IALProtocolStructure _mes)
        {
            if (null == _mes)
                return;

            //锁定队列后，将消息添加到队列末尾
            lock (_m_lSendMesList)
            {
                //判断是否可发送消息
                if (!_m_bCanSendMes)
                {
                    ALLog.Warning($"Send Mes [{_mes.getMainOrder()}]-[{_mes.getSubOrder()}] when send thread still not inited! mes will be send after [loginMes] sended");
                    //将消息放入临时缓存队列
                    if (null == _m_lInlegallMes)
                        _m_lInlegallMes = new List<byte[]>();

                    _m_lInlegallMes.Add(_mes.makeFullPackage());
                    return;
                }

                //生成长度对象BUF
                int msgLen = _mes.GetFullPackBufSize();
                ALBasicProtocolPack.ALProtocolBuf totalBuf = ALBasicProtocolPack.ALProtocolBuf.allocate(4 + msgLen);
                totalBuf.putInt(msgLen);
                _mes.makeFullPackage(totalBuf);

                //生成完成的数据格式
                _m_lSendMesList.Add(totalBuf.getBuf());

                //重置
                totalBuf.discard();
                totalBuf = null;

                //设置信号量为有信号
                _m_evSendMesEvent.Set();
            }
        }
        public void sendMes(byte[] _mes)
        {
            if (null == _mes)
                return;

            //锁定队列后，将消息添加到队列末尾
            lock (_m_lSendMesList)
            {
                //判断是否可发送消息
                if (!_m_bCanSendMes)
                {
                    ALLog.Warning($"Send Mes when send thread still not inited! mes will be send after [loginMes] sended");
                    //将消息放入临时缓存队列
                    if (null == _m_lInlegallMes)
                        _m_lInlegallMes = new List<byte[]>();

                    _m_lInlegallMes.Add(_mes);

                    return;
                }

                //生成长度对象BUF
                ALBasicProtocolPack.ALProtocolBuf totalBuf = ALBasicProtocolPack.ALProtocolBuf.allocate(4 + _mes.Length);
                totalBuf.putInt(_mes.Length);
                totalBuf.cpyByteBuffer(_mes);

                //生成完成的数据格式
                _m_lSendMesList.Add(totalBuf.getBuf());

                //重置
                totalBuf.discard();
                totalBuf = null;

                //设置信号量为有信号
                _m_evSendMesEvent.Set();
            }
        }

        /// <summary>
        /// 发送登录消息，这个接口单独实现是为了避免_m_bCanSendMes的限制
        /// 并且可以保证作为第一个消息放入队列
        /// </summary>
        /// <param name="_mes"></param>
        public void sendLoginMes(byte[] _mes)
        {
            if (null == _mes)
                return;

            //锁定队列后，将消息添加到队列末尾
            lock (_m_lSendMesList)
            {
                //生成长度对象BUF
                ALBasicProtocolPack.ALProtocolBuf totalBuf = ALBasicProtocolPack.ALProtocolBuf.allocate(4 + _mes.Length);
                totalBuf.putInt(_mes.Length);
                totalBuf.cpyByteBuffer(_mes);

                //生成完成的数据格式
                _m_lSendMesList.Add(totalBuf.getBuf());

                //重置
                totalBuf.discard();
                totalBuf = null;

                //设置信号量为有信号
                _m_evSendMesEvent.Set();
            }
        }

        /// <summary>
        /// 设置可以发送消息，并将原先的消息放入发送队列
        /// </summary>
        /// <param name="_mes"></param>
        public void setCanSendMes()
        {
            //锁定队列后，将消息添加到队列末尾
            lock (_m_lSendMesList)
            {
                //设置可发送消息，使用_m_lSendMesList保证原子操作
                _m_bCanSendMes = true;

                //此时需要将所有临时消息放回队列
                if (null != _m_lInlegallMes)
                {
                    for (int i = 0; i < _m_lInlegallMes.Count; i++)
                    {
                        sendMes(_m_lInlegallMes[i]);
                    }
                    //清空队列
                    _m_lInlegallMes.Clear();
                    _m_lInlegallMes = null;
                }
            }
        }

        /*****************
        * 从接收到的消息队列中取出第一个消息对象
        **/
        public void popRecMsg(List<byte[]> _recList)
        {
            if (null == _recList)
                return;

            _recList.Clear();

            lock (_m_lReceivedMesList)
            {
                if (_m_lReceivedMesList.Count <= 0)
                    return;

                _recList.AddRange(_m_lReceivedMesList);
                _m_lReceivedMesList.Clear();
            }
        }

        /*****************
          * 从消息队列中取出第一个消息对象
          **/
        protected byte[] _popFirstSendMsg()
        {
            byte[] res = null;

            //进行超时处理，避免异常情况导致线程无法退出
            _m_evSendMesEvent.WaitOne(100);

            lock (_m_lSendMesList)
            {
                if (_m_lSendMesList.Count <= 0)
                    return null;

                res = _m_lSendMesList[0];
                _m_lSendMesList.RemoveAt(0);

                //判断队列是否有数据，无则设置信号量为无新型号
                if (_m_lSendMesList.Count <= 0)
                    _m_evSendMesEvent.Reset();
            }

            return res;
        }

        /*********************
         * 当接收消息时进行处理，在未登录前需要进行登录验证
         **/
        protected void _onReceiveMes(byte[] _mes)
        {
            if (ClientStat.LOGIN == _m_eClientStat)
            {
                //客户端状态为登录状态则进行消息处理
                lock (_m_lReceivedMesList)
                {
                    _m_lReceivedMesList.Add(_mes);
                }
            }
            else if (ClientStat.CONNECTED == _m_eClientStat)
            {
                //已连接状态，判断是否验证成功消息
                BasicServer.S2C_BasicClientVerifyResult mes = new BasicServer.S2C_BasicClientVerifyResult();

                try
                {
                    mes.readPackage(_mes);
                }
                catch(Exception )
                {
                    //先获取是否主动退出
                    bool isExit = _m_bExit;

                    //读取出错直接退出
                    logout();
                    //只有在非主动退出才报错调用异常
                    if(!isExit)
                        _onDisconnect(DisconnectReason.READING_VERIFY_ERROR);

                    return;
                }

                //获取SocketID
                _m_lSocketID = mes.getSocketID();

                //设置连接状态，未成功将直接断开
                _m_eClientStat = ClientStat.LOGIN;
                //设置可发送消息
                setCanSendMes();
                //开启任务执行事件
                ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<string>(LoginSuc, mes.getCustomRetMsg()));
            }
            else
            {
                //其他状态不进行处理
            }
        }

        /***************
         * 在断开连接时调用的内部函数，根据不同状态调用不同的处理函数
         **/
        protected void _onDisconnect(DisconnectReason _reason)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError($"[EDITOR]{GetType()} disconnect reason:{_reason}, {_m_eClientStat}");
#endif
            if(ClientStat.NONE == _m_eClientStat || ClientStat.INIT == _m_eClientStat)
            {
                ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<DisconnectReason>(ConnectFail, _reason));
            }
            else if (ClientStat.CONNECTED == _m_eClientStat)
            {
                if (_reason == DisconnectReason.READING_ERROR || _m_bIsClientLogout)
                {
                    //reading_err 表示读取出错，可能是网络出现问题而不是被服务端踢出
                    //如果_m_bIsClientLogout为true，表示玩家操作的断开，不论报错是什么都会触发连接断开的处理
                    ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<DisconnectReason>(ConnectFail, _reason));
                }
                else
                {
                    ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<DisconnectReason>(LoginFail, _reason));
                }
            }
            else if(null != _m_tcConnectClient)
            {
                ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<DisconnectReason>(Disconnect, _reason));
            }

            //设置状态为断开连接
            _m_eClientStat = ClientStat.DISCONNECTED;
        }

        /****************
         * 接收消息的处理函数，此函数在接收线程中处理，如需要更好的处理方式则需要另开线程进行处理
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:48 PM
         */
        public abstract void receiveMes(byte[] _mes);
        /****************
         * 相关初始化失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:53 PM
         */
        public abstract void InitFail(DisconnectReason _reason);
        /****************
         * 连接服务器失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:04:53 PM
         */
        public abstract void ConnectFail(DisconnectReason _reason);
        /***************
         * 登录失败
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:03 PM
         */
        public abstract void LoginFail(DisconnectReason _reason);
        /***************
         * 登出操作
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:07 PM
         */
        public abstract void Disconnect(DisconnectReason _reason);
        /***************
         * 登录成功
         * 
         * @author alzq.z
         * @time   Feb 19, 2013 10:05:11 PM
         */
        public abstract void LoginSuc(string _customRetMsg);

        /*******************
         * 初始化连接时调用的内部函数
         **/
        protected abstract bool _onInitConnection();

        /**********************
         * 初始化连接对象的操作线程用于控制超时的判断
         **/
        private void _initConnection()
        {
            try
            {
                //创建连接对象
                _m_tcConnectClient = new TcpClient(_m_sConnectIP, _m_iConnectPort);

                //设置状态为已连接状态
                _m_eClientStat = ClientStat.CONNECTED;
                //设置nodelay
                _m_tcConnectClient.NoDelay = true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Connect "+ _m_sConnectIP + ":"+ _m_iConnectPort + " Fail Mes: " + ex.Message);

                //尝试关闭连接
                try
                {
                    if (null != _m_tcConnectClient)
                        _m_tcConnectClient.Close();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Connect : {_m_sConnectIP}:{_m_iConnectPort} Close Fail: {e}");
                }

                _m_tcConnectClient = null;
            }
        }

        /******************
         * 连接服务器以及后续消息处理过程
         **/
        private void _receiveThreadFun()
        {
            //调用初始化函数
            if (!_onInitConnection())
            {
                //开启任务执行初始化失败错误
                ALMonoTaskMgr.instance.addMonoTask(new ALBasicClientOBjDelegateMonoTask<DisconnectReason>(InitFail, DisconnectReason.INIT_FAIL));
                return;
            }

            _m_eClientStat = ClientStat.INIT;
            //创建一个信号量
            ManualResetEvent initEvent = new ManualResetEvent(false);
            //开启超时控制创建连接对象，由于连接对象无法控制，因此借由线程方式进行控制
            Thread connectionCreateThread = new Thread(new ThreadStart(() =>
                                            {
                                                initEvent.Set();
                                                //初始化连接
                                                _initConnection();
                                            }));
            connectionCreateThread.Start();

            //确保线程开始执行
            initEvent.WaitOne();

            Thread.Sleep(500);
            //进行超时判断
            bool isDone = connectionCreateThread.Join(5000);

            //判断连接对象以及状态，不符合预订情况一致认定为无法连接服务器
            if (null == _m_tcConnectClient || ClientStat.CONNECTED != _m_eClientStat)
            {
                //强制终止线程
                if(!isDone)
                    connectionCreateThread.Abort();

                bool isExit = _m_bExit;

                //关闭连接对象
                logout();
                //只有在非正常退出才处理对象无效则连接失败
                if(!isExit)
                    _onDisconnect(DisconnectReason.NONE);
                return;
            }


            //线程内使用的内存变量
            byte[] socketBuf = null;
            /** 对缓存对象进行处理的流对象 */
            MemoryStream socketBufStream = null;
            byte[] tmpStreamBuf = null;

            try
            {
                socketBuf = new byte[_m_iBufLen];
                //创建处理流
                socketBufStream = new MemoryStream(_m_iBufLen + _m_iBufLen);
                tmpStreamBuf = new byte[_m_iBufLen + _m_iBufLen];

                //开启消息发送线程
                initSendThread();

                //发送消息
                BasicServer.C2S_BasicClientVerifyInfo loginMes = new BasicServer.C2S_BasicClientVerifyInfo();
                loginMes.setClientType(_m_iClientType);
                loginMes.setUserName(_m_sUserName);
                loginMes.setUserPassword(_m_sUserPassword);
                loginMes.setCustomMsg(_m_sCustomMsg);

                //单独使用发送登录消息处理，保证状态设置正确
                sendLoginMes(loginMes.makePackage());

                while (!_m_bExit)
                {
                    if (!_m_tcConnectClient.GetStream().CanRead)
                    {
                        //如不可读取则等待后继续下一循环
                        Thread.Sleep(10);
                        continue;
                    }

                    //从连接对象的流中读取数据
                    int readBufLen = _m_tcConnectClient.GetStream().Read(socketBuf, 0, _m_iBufLen);
                    //判断数据是否有效
                    if (readBufLen <= 0)
                    {
                        //数据无效则断开连接，并调用事件函数
                        if (!_m_bExit)
                        {
                            _m_bExit = true;
                            //调用断开连接的内部事件函数
                            _onDisconnect(DisconnectReason.DATA_EMPTY);
                        }

                        //关闭连接
                        try
                        {
                            _m_tcConnectClient.Close();
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogError($"Connect : {_m_sConnectIP}:{_m_iConnectPort} Close Fail: {e}");
                        }

                        return;
                    }
                    else
                    {
                        //将读取到的数据放入流处理对象
                        socketBufStream.Write(socketBuf, 0, readBufLen);
                        //调用函数处理流中读取到的内容
                        _dealBufStream(socketBufStream, tmpStreamBuf);
                    }
                }
            }
            catch (Exception )
            {
                //当捕获到连接错误时
                if (!_m_bExit)
                {
                    _m_bExit = true;
                    //调用断开连接的内部事件函数
                    _onDisconnect(DisconnectReason.READING_ERROR);
                }

                //尝试关闭连接
                try
                {
                    if (null != _m_tcConnectClient)
                    {
                        _m_tcConnectClient.Close();
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Connect : {_m_sConnectIP}:{_m_iConnectPort} Close Fail: {e}");
                }
            }
            finally
            {
                if (null != socketBufStream)
                {
                    socketBufStream.Close();
                    socketBufStream.Dispose();
                }
                socketBufStream = null;
                tmpStreamBuf = null;
                //释放内存
                socketBuf = null;
            }
        }

        /******************
         * 处理缓存处理流中的数据
         **/
        private void _dealBufStream(MemoryStream _socketBufStream, byte[] _tmpStreamBuf)
        {
            int mesLen = 0;
            //处理开始的下标位置
            int readBinPos = 0;
            //消息开始读取的下标位置
            int mesBinPos = 4;

            //记录当前流中的读取指针位置
            long prePos = _socketBufStream.Position;

            //获取流对象的原数组对象
            byte[] streamArray = _socketBufStream.GetBuffer();

            //当流中的数据超出长度信息则表示可以进行处理
            while (prePos > mesBinPos)
            {
                //将前4字节数据读取出
                int binPos = readBinPos;
                mesLen = ALBasicProtocolPack.ALProtocolCommon.getInt(streamArray, ref binPos);

                //根据原先读取指针所在位置判断是否已经完成一个消息的读取
                if (prePos >= (mesLen + mesBinPos))
                {
                    //将对应的内容拷贝出来
                    byte[] mes = new byte[mesLen];
                    Array.Copy(streamArray, mesBinPos, mes, 0, mesLen);
                    //调用消息处理函数
                    _onReceiveMes(mes);

                    //计算下一次循环中的各个计算参数
                    readBinPos = mesBinPos + mesLen;
                    mesBinPos = readBinPos + 4;
                }
                else
                {
                    break;
                }
            }

            //当读取的开始坐标变更表明需要重新拷贝数据到处理流中
            if (readBinPos != 0)
            {
                //重置读取下标
                _socketBufStream.Seek(0, SeekOrigin.Begin);

                //判断是否需要拷贝数据
                if(prePos > readBinPos)
                {
                    int leftLen = (int)(prePos - readBinPos);
                    //将需要重新写入流的数据写入临时数据块中
                    Array.Copy(streamArray, readBinPos, _tmpStreamBuf, 0, leftLen);
                    //将临时块中的数据写入流中
                    _socketBufStream.Write(_tmpStreamBuf, 0, leftLen);
                }
            }
        }


        /***********************
         * 发送消息线程部分
         **/
        private void _sendThreadFun()
        {
            //连接断开则返回
            if (_m_tcConnectClient == null || _m_tcConnectClient.Client == null || !_m_tcConnectClient.Client.Connected)
                return;

            //获取操作流
            NetworkStream sendStream = _m_tcConnectClient.GetStream();

            while (!_m_bExit)
            {
                if(null == _m_tcConnectClient || null == _m_tcConnectClient.Client)
                    break;

                if (!sendStream.CanWrite)
                {
                    //如不可读取则等待后继续下一循环
                    Thread.Sleep(10);
                    continue;
                }

                //取出第一个需要发送的数据
                byte[] sendMes = _popFirstSendMsg();

                //连接断开则跳出循环
                if (null == _m_tcConnectClient || null == _m_tcConnectClient.Client || !_m_tcConnectClient.Client.Connected)
                    break;

                //数据无效则继续循环
                if (null == sendMes)
                    continue;

                try
                {
                    if (null != _m_tcConnectClient && _m_tcConnectClient.Client.Connected)
                    {
                        //发送数据
                        sendStream.Write(sendMes, 0, (int)sendMes.Length);
                    }
                    else
                    {
                        //此时跳出循环
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }
                finally
                {
                    sendMes = null;
                }
            }
        }
    }
}
