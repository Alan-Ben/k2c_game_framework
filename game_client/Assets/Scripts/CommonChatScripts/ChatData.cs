
using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage.Internal;
using JetBrains.Annotations;

namespace ChatPackage
{
    /// <summary>
    /// 聊天包的数据层对外接口
    /// </summary>
    /// <list type="bullet">
    ///     <item>
    ///         <term>网络相关接口</term>
    ///         <description>
    ///         你可以在这个实例下使用网络相关的内容如连接ip，端口，还包括连接和断开方法，以及对网络情况的事件监听
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>addChatInfo</term>
    ///         <description>
    ///         <para>需要特别注意：</para>
    ///         <para>※对于聊天室，聊天包希望你在外部管理聊天室，当你需要这个聊天室拥有正常收发消息的功能时，你需要把聊天室类添加到聊天包当中，聊天包就可以让这个聊天室开始运作</para>
    ///         <para>※对于私聊，如果收到了新消息，而这个新消息对应的聊天会话没有被加入到聊天包内，就会由聊天包生成一个私聊的会话，来尽量保证不会丢失消息，而你可以控制聊天包如何生成这个新的私聊</para>
    ///         <para>私聊的部分你需要充分理解聊天包是如何运作的再进行相关操作</para>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>removeChatInfo</term>
    ///         <description>
    ///         从聊天包中移除聊天会话，可以移除聊天室和私聊，对于私聊请确保这个会话大概率不会收到新消息了再移除
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>getChatInfo</term>
    ///         <description>
    ///         当然你还可以获得聊天包里的聊天会话数据
    ///         </description>
    ///     </item>
    /// </list>
    public class ChatData
    {
        // 聊天服务端分配给聊天包的唯一id
        private readonly string _m_lUid;
        // 外部传入的客户端uid，
        private readonly string _m_gameClientUid;
        //聊天系统ID
        private long _m_systemId;
        //聊天系统标识
        private readonly string _m_systemTag;
        // 目标聊天服务器的ip
        private readonly string _m_sIp;
        // 目标聊天服务器的端口
        private readonly int _m_iPort;
        // 连接所需的密钥
        private readonly string _m_sCheckCode;
        // 数据管理器
        [NotNull] private readonly ChatDataMgr _m_chatDataMgr;
        // 自定义id的数据管理器
        [NotNull] private readonly Dictionary<long, ChatDataMgr> _m_customChatDataDic;
        // 网络连接实例
        private ChatClient _m_chatClient;
        // 断开之后会进行重连的次数，前3次会在0.5秒后就尝试重连，之后都是间隔3秒后重连
        private int _m_iRetryCount;
        // 用来记录当前重连尝试的次数
        private int _m_iTryLoginCount = 0;
        // 连接序列号，当序列号发生变化的时候，就认为这一次的连接结束了，这时候要中断还在尝试中的重连，否则重连有可能会把新的连接盖掉
        private int _m_iConnectSerialize;

        /// <summary>
        /// 构造方法，你需要提供目标聊天服务器的连接数据，以及一个产生新私聊时的委托方法
        /// </summary>
        /// <param name="_systemId">聊天系统ID，游戏服务器会给</param>
        /// <param name="_systemTag">聊天系统标识，游戏服务器会给</param>
        /// <param name="_uid">客户端的唯一id，暂时为玩家cid</param>
        /// <param name="_ip">目标聊天服务器的ip，游戏服务器会给</param>
        /// <param name="_port">目标聊天服务器的端口，游戏服务器也会给</param>
        /// <param name="_checkCode">连接所需的密钥，游戏服务器也会给</param>
        /// <param name="_newChatInfoDelegate">产生新私聊的委托，你需要自己写一个这样的方法注册进来</param>
        public ChatData(long _systemId,string _systemTag, string _uid, string _ip, int _port, string _checkCode, ChatDataMgr.OnReceiveNewChatInfo _newChatInfoDelegate)
        {
            _m_lUid = ChatUtility.makeChatUid(_systemId, _systemTag , _uid);
            _m_gameClientUid = _uid;
            _m_systemId = _systemId;
            _m_systemTag = _systemTag;
            _m_sIp = _ip;
            _m_iPort = _port;
            _m_sCheckCode = _checkCode;

            _m_chatDataMgr = new ChatDataMgr(this, _newChatInfoDelegate);
            _m_customChatDataDic = new Dictionary<long, ChatDataMgr>();

            __m_bIsEnable = false;
        }

        /// <summary>
        /// 当网络连接上了
        /// </summary>
        public event Action onConnected;
        /// <summary>
        /// 当网络连接断开了
        /// </summary>
        public event Action onDisconnected;
        /// <summary>
        /// 当网络连接失败了（不会再尝试连接了）
        /// </summary>
        public event Action onConnectFailed;
        /// <summary>
        /// 聊天服务端分配给聊天包的唯一id
        /// </summary>
        public string uid { get { return _m_lUid; } }
        /// <summary>
        /// 外部传入的客户端uid
        /// </summary>
        public string gameClientUid { get { return _m_gameClientUid; } }
        /// <summary>
        /// 聊天系统ID
        /// </summary>
        public long systemId { get { return _m_systemId; } }
        /// <summary>
        /// 聊天系统标识
        /// </summary>
        public string systemTag { get { return _m_systemTag; } }
        /// <summary>
        /// 目标聊天服务器的ip
        /// </summary>
        public string ip { get { return _m_sIp; } }
        /// <summary>
        /// 目标聊天服务器的端口
        /// </summary>
        public int port { get { return _m_iPort; } }
        /// <summary>
        /// 连接所需的密钥
        /// </summary>
        public string checkCode { get { return _m_sCheckCode; } }
        /// <summary>
        /// 网络连接
        /// </summary>
        public ChatClient chatClient { get { return _m_chatClient; } }
        /// <summary>
        /// 是否已经连接成功了
        /// </summary>
        public bool isConnected { get { return _m_bIsEnable; } }
        // 当前连接是否有效
        private bool _m_bIsEnable
        {
            // 直接放回，get不做特殊的处理
            get { return __m_bIsEnable; }
            // set时有可能会影响连接状态，在这里触发连接状态变更的事件
            set
            {
                // 记录一下赋值前的连接状态
                bool beforeConnection = __m_bIsEnable;
                // 赋值
                __m_bIsEnable = value;
                // 赋值之后再确认一次连接状态有没有发生变化
                if (beforeConnection != __m_bIsEnable)
                {
                    // 发生变化则根据当前的状态触发事件
                    if (__m_bIsEnable)
                        onConnected?.Invoke();
                    else
                        onDisconnected?.Invoke();
                }
            }
        }
        // 配合上面的属性访问器使用的对象，并没有特别的意义
        private bool __m_bIsEnable;

        public void connect(int _netRetryCount = int.MaxValue)
        {
            // 赋值尝试重连次数
            _m_iRetryCount = _netRetryCount;
            // 连接聊天服务器
            _connect(true);
        }

        /// <summary>
        /// 停止聊天包的运作
        /// </summary>
        public void disconnect()
        {
            // 序列号加一，表示之前的连接序列号已经作废
            _m_iConnectSerialize = ALSerializeOpMgr.next();
            // 设置为连接断开
            _m_bIsEnable = false;

            // 如果连接不存在不做处理
            if (_m_chatClient == null)
                return;

            // 把连接断开
            _m_chatClient.logout();
            _m_chatClient.onConnectFailed -= _tryReconnect;
            _m_chatClient.onDisconnect -= _tryReconnect;
            _m_chatClient.onConnectComplete -= _onConnectComplete;
            // 由于底层的设计，每个_AALBasicClient都是一次性使用的，断开了之后就要重新new一个，这里直接把这个作废了
            _m_chatClient = null;
        }

        /// <summary>
        /// 获取聊天管理类
        /// </summary>
        /// <returns>
        /// 要操作管理类，你需要清楚你在做什么
        /// </returns>
        [NotNull]
        public ChatDataMgr getDataMgr()
        {
            return _m_chatDataMgr;
        }
        /// <summary>
        /// 获取自定义的聊天管理类
        /// </summary>
        /// <returns>
        /// 要操作管理类，你需要清楚你在做什么
        /// </returns>
        public ChatDataMgr getDataMgr(long _id)
        {
            if (_m_customChatDataDic.TryGetValue(_id, out ChatDataMgr dataMgr))
                return dataMgr;

            ChatUtility.logWarning_DebugOnly($"没有找到id为 {_id} 的chatDataMgr，请先使用createCustomDataMgr创建一个chatDataMgr");
            return null;
        }

        /// <summary>
        /// 添加一个聊天会话
        /// </summary>
        /// <param name="_chatInfo">对应的聊天会话</param>
        public bool addChatInfo(_AChatInfo _chatInfo)
        {
            return _m_chatDataMgr.addChatInfo(_chatInfo);
        }

        /// <summary>
        /// 添加一个聊天会话
        /// </summary>
        /// <param name="_id">自定义的dataMgr的id</param>
        /// <param name="_chatInfo">对应的聊天会话</param>
        public bool addChatInfo(long _id, _AChatInfo _chatInfo)
        {
            if (_m_customChatDataDic.TryGetValue(_id, out ChatDataMgr dataMgr) && dataMgr != null)
                return dataMgr.addChatInfo(_chatInfo);

            ChatUtility.logWarning_DebugOnly($"没有找到id为 {_id} 的chatDataMgr，请先使用createCustomDataMgr创建一个chatDataMgr");
            return false;
        }

        /// <summary>
        /// 移除一个聊天会话
        /// </summary>
        /// <param name="_chatInfoId">对应的聊天会话标识</param>
        public void removeChatInfo(string _chatInfoId)
        {
            _m_chatDataMgr.removeChatInfo(_chatInfoId);
        }

        /// <summary>
        /// 移除一个聊天会话
        /// </summary>
        /// <param name="_id">自定义的dataMgr的id</param>
        /// <param name="_chatInfoId">对应的聊天会话标识</param>
        public void removeChatInfo(long _id, string _chatInfoId)
        {
            if (_m_customChatDataDic.TryGetValue(_id, out ChatDataMgr dataMgr) && dataMgr != null)
                dataMgr.removeChatInfo(_chatInfoId);

            ChatUtility.logWarning_DebugOnly($"没有找到id为 {_id} 的chatDataMgr，请先使用createCustomDataMgr创建一个chatDataMgr");
        }

        /// <summary>
        /// 获取所有聊天会话
        /// </summary>
        public void getAllChatInfo(List<_AChatInfo> _list)
        {
            _m_chatDataMgr.getAllChatInfo(_list);
        }

        /// <summary>
        /// 获取对应的聊天会话
        /// </summary>
        /// <typeparam name="T">聊天会话的类型</typeparam>
        /// <param name="_chatInfoId">聊天会话的标识</param>
        public T getChatInfo<T>(string _chatInfoId)
            where T : _AChatInfo
        {
            return _m_chatDataMgr.getChatInfo(_chatInfoId) as T;
        }


        /// <summary>
        /// 获取对应的聊天会话
        /// </summary>
        /// <typeparam name="T">聊天会话的类型</typeparam>
        /// <param name="_id">自定义的dataMgr的id</param>
        /// <param name="_chatInfoId">聊天会话的标识</param>
        public T getChatInfo<T>(long _id, string _chatInfoId)
            where T : _AChatInfo
        {
            if (_m_customChatDataDic.TryGetValue(_id, out ChatDataMgr dataMgr) && dataMgr != null)
                return dataMgr.getChatInfo(_chatInfoId) as T;

            ChatUtility.logWarning_DebugOnly($"没有找到id为 {_id} 的chatDataMgr，请先使用createCustomDataMgr创建一个chatDataMgr");
            return default;
        }

        /// <summary>
        /// 添加对应msgType的设置，指定这个msgType构建的detailInfo和senderInfo
        /// </summary>
        /// <param name="_msgType">表示你想设置哪个msgType的内容</param>
        /// <param name="_setting">指明对应的设置</param>
        public void setMsgInfoSetting(int _msgType, _AChatDataSetting _setting)
        {
            if (_setting == null)
                return;

            // 把设置注册到数据管理器中
            _m_chatDataMgr.setMsgInfoSetting(_msgType, _setting);
        }

        /// <summary>
        /// 添加对应msgType的设置，指定这个msgType构建的detailInfo和senderInfo
        /// </summary>
        /// <param name="_msgType">表示你想设置哪个msgType的内容</param>
        /// <param name="_setting">指明对应的设置</param>
        public void setMsgInfoSetting(long _id, int _msgType, _AChatDataSetting _setting)
        {
            if (_setting == null)
                return;

            if (_m_customChatDataDic.TryGetValue(_id, out ChatDataMgr dataMgr) && dataMgr != null)
                _m_chatDataMgr.setMsgInfoSetting(_msgType, _setting);

            ChatUtility.logWarning_DebugOnly($"没有找到id为 {_id} 的chatDataMgr，请先使用createCustomDataMgr创建一个chatDataMgr");
        }

        public void createCustomDataMgr(long _id, ChatDataMgr.OnReceiveNewChatInfo _newChatInfoDelegate)
        {
            if (_m_customChatDataDic.ContainsKey(_id))
            {
                ChatUtility.logWarning_DebugOnly($"已经存在一个id为 {_id} 的自定义dataMgr了");
                return;
            }

            _m_customChatDataDic.Add(_id, new ChatDataMgr(this, _newChatInfoDelegate));
        }

        public void clearData()
        {
            _m_chatDataMgr.clearAllData();
            foreach (ChatDataMgr dataMgr in _m_customChatDataDic.Values)
            {
                dataMgr?.clearAllData();
            }
            _m_customChatDataDic.Clear();
        }

        // 断开之前的链接并与聊天服务器建立新的链接
        private void _connect(bool _resetTryCount)
        {
            // 序列号加一，表示之前的连接序列号已经作废
            _m_iConnectSerialize = ALSerializeOpMgr.next();

            // 断开之前的连接
            disconnect();

            // 外部接口默认传入true，都当成建立一个新连接，重置重连计数
            if (_resetTryCount)
                _m_iTryLoginCount = 0;

            // 创建新的连接对象
            _m_chatClient = new ChatClient(_m_sIp, _m_iPort, _m_lUid, _m_sCheckCode, _m_chatDataMgr);
            _m_chatClient.onConnectFailed += _tryReconnect;
            _m_chatClient.onDisconnect += _tryReconnect;
            _m_chatClient.onConnectComplete += _onConnectComplete;
            // 开始连接
            _m_chatClient.login(1024 * 100);
        }

        // 尝试重新连接
        private void _tryReconnect()
        {
            // 设置为连接断开
            _m_bIsEnable = false;

            // 记录当前的连接序列号，以防在等待重连的过程中建立了新的连接
            int serialize = _m_iConnectSerialize;
            // 判断是否还有重连次数
            if (_m_iTryLoginCount < _m_iRetryCount)
            {
                // 计数器增加
                _m_iTryLoginCount++;
                // 执行延迟任务，在对应的时间后开始重连
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    // 如果当前的连接已经被废弃了，序列号会发生变化，后续就不再做处理了
                    if (serialize != _m_iConnectSerialize)
                        return;

                    // 尝试进行连接
                    _connect(false);
                }, _getReloginInterval(_m_iTryLoginCount));
            }
            else
            {
                // 如果没有重连次数了，触发委托，交给外部看有没有别的要处理的
                onConnectFailed?.Invoke();
            }
        }

        private void _onConnectComplete()
        {
            // 设置为连接成功
            _m_bIsEnable = true;

            //连接上之后获取一下当前所有未读会话
            _m_chatDataMgr.reqPrivateUnreadMsgBrief();

            ChatUtility.log($"connect succeed, chat server {_m_sIp}:{_m_iPort}");
        }

        // 获取重连的时间间隔，头3次为0.5秒尝试一次，之后都为3秒尝试一次
        private float _getReloginInterval(int _reloginCount)
        {
            return _reloginCount <= 3 ? 0.5f : 3f;
        }
    }
}