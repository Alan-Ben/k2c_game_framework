
using System;
using ALPackage;
using ALBasicProtocolPack;
using JetBrains.Annotations;
using GC2GS.p001_BasicOp;
using Common.CommEnum;
using GC2GS.p001_BasicOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 连接聊天服务器的客户端
    /// </summary>
    /// <remarks>
    /// <para>你可以引入Internal包来使用这个类</para>
    /// <para>一个这个类的实例为一个连接，并且断开连接后，这个实例就不能被使用了，具体可以看底层的相关实现<see cref="_AALBasicClient"/></para>
    /// </remarks>
    public sealed class ChatClient : _AALBasicClient
    {
        // 数据管理器，这里是为了把收到的回包数据交给管理器处理
        [NotNull] private readonly ChatDataMgr _m_dataMgr;
        
        // 构造方法，连接相关的传参服务端会给，另外你还需要一个对应的数据管理器
        public ChatClient(string _connectIp, int _port, string _uid, string _token, [NotNull] ChatDataMgr _dataMgr)
            : base(_connectIp, _port, (int)EGSChatClientType.NORMAL_PLAYER, _uid, _token, string.Empty)
        {
            _m_dataMgr = _dataMgr;
        }

        /// <summary>
        /// 当连接失败了的事件
        /// </summary>
        public event Action onConnectFailed;
        /// <summary>
        /// 连接中断的事件
        /// </summary>
        public event Action onDisconnect;
        /// <summary>
        /// 连接成功的事件
        /// </summary>
        public event Action onConnectComplete;
        /// <summary>
        /// 数据管理器
        /// </summary>
        [NotNull] public ChatDataMgr dataMgr { get { return _m_dataMgr; } }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        public void sendMsg(_IALProtocolStructure _protocol)
        {
            // 触发发送消息的日志相关的委托
            Chat.sendMsgLogFunc?.Invoke(_protocol);

#if UNITY_EDITOR
            if (null != _protocol && _protocol.GetFullPackBufSize() > 20 * 1024)
            {
                Debug.LogError($"协议大小超过20k: {_protocol.GetType().Name}，大小：{_protocol.GetFullPackBufSize()}");
            }
#endif
            
            // 调用底层的发送消息方法
            sendMes(_protocol);
        }
        /// <inheritdoc/> 
        public override void ConnectFail(DisconnectReason _reason)
        {
            // 触发对应事件
            onConnectFailed?.Invoke();
        }
        /// <inheritdoc/> 
        public override void Disconnect(DisconnectReason _reason)
        {
            // 触发对应事件
            onDisconnect?.Invoke();
        }
        /// <inheritdoc/> 
        public override void InitFail(DisconnectReason _reason)
        {
            // 触发对应事件
            onConnectFailed?.Invoke();
        }
        /// <inheritdoc/> 
        public override void LoginFail(DisconnectReason _reason)
        {
            // 触发对应事件
            onConnectFailed?.Invoke();
        }
        /// <inheritdoc/> 
        public override void LoginSuc(string _customRetMsg)
        {
            // 触发对应事件
            onConnectComplete?.Invoke();

            // 发送心跳包
            _sendHeart();
        }
        /// <inheritdoc/> 
        public override void receiveMes(byte[] _msg)
        {
            // 交给分发器去处理了
            ProtocolDispatcher.instance.DealProtocol(this, _msg);
        }
        
        /// <inheritdoc/> 
        protected override bool _onInitConnection()
        {
            // 这里没有任何额外的处理，直接返回true
            return true;
        }

        /// <summary>
        /// 当收到了心跳包之后的处理
        /// </summary>
        internal void retHeart()
        {
            // 收到心跳之后，5秒后接着发送心跳
            ALCommonTaskController.CommonActionAddMonoTask(_sendHeart, 5);
        }

        // 发送心跳包
        private void _sendHeart()
        {
            // 调用底层的发送接口
            sendMsg(new GC2GS_001_001_ReqHeartBeat());
        }
    }
}
