
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace ChatPackage
{
    /// <summary>
    /// 聊天会话消息的具体内容
    /// </summary>
    /// <remarks>
    /// <para>你需要为你的聊天会用到的每一个消息类型都实现一个这个类的子类，并使用ChatData.setMsgInfoSetting注册到聊天包当中，详情可以看<see cref="Chat"/>中的方法</para>
    /// <para>需要注意：如果你想使用<see cref="GUISubWndChatMsgList"/>来展示消息，你需要理解<see cref="_IMsgItemData"/>和这个类的区别</para>
    /// <para>简而言之，<c>_IMsgItemData</c>是UI用的数据接口，更多内容可以查阅<see cref="_IMsgItemData"/>的注解</para>
    /// </remarks>
    public abstract class _AMsgDetailInfo : _IMsgItemData
    {
        // 消息类型
        private int _m_iMsgType;

        // 用消息类型构建
        protected _AMsgDetailInfo(int _msgType)
        {
            _m_iMsgType = _msgType;
        }

        /// <summary>
        /// 这个消息的类型
        /// </summary>
        public int msgType { get { return _m_iMsgType; } }

        /// <summary>
        /// 是否为自己发送的消息
        /// </summary>
        public abstract bool isMyMsg { get; }
        /// <summary>
        /// 将发送者信息转为byte[]，用来传输，如果没有可以返回null
        /// </summary>
        public abstract byte[] getSenderBytesData();
        /// <summary>
        /// 将消息的内容转为byte[]，用来传输，如果没有可以返回null
        /// </summary>
        public abstract byte[] getContentBytesData();
        /// <summary>
        /// 从消息内容的byte[]和发送者的byte[]中读取数据的内容
        /// </summary>
        /// <param name="_content">消息内容信息</param>
        /// <param name="_sender">发送者信息</param>
        public abstract void readByBytes(byte[] _content, byte[] _sender);
    }
    /// <summary>
    /// 聊天会话消息的具体内容
    /// </summary>
    /// <remarks>
    /// 只有数据内容的聊天消息，没有发送者信息。详细的内容可以查看<see cref="_AMsgDetailInfo"/>
    /// </remarks>
    public abstract class _AMsgDetailInfo<T_CONTENT> : _AMsgDetailInfo
        where T_CONTENT : _IALProtocolStructure, new()
    {
        // 内容的数据
        [NotNull] private readonly T_CONTENT _m_content;
        
        protected _AMsgDetailInfo(int _msgType) : base(_msgType)
        {
            // 构建这两个类
            _m_content = new T_CONTENT();
        }
        
        /// <summary>
        /// 内容的数据
        /// </summary>
        [NotNull] public T_CONTENT content { get { return _m_content; } }
        
        /// <inheritdoc />
        public override byte[] getSenderBytesData()
        {
            return null;
        }
        /// <inheritdoc />
        public override byte[] getContentBytesData()
        {
            return _m_content.makePackage();
        }
        /// <inheritdoc />
        public override void readByBytes(byte[] _content, byte[] _sender)
        {
            _m_content.readPackage(new ALProtocolBuf(_content));
        }
    }
    /// <summary>
    /// 聊天会话消息的具体内容
    /// </summary>
    /// <remarks>
    /// 标准的代有一个数据内容content和一个发送者内容sender的类。详细的内容可以查看<see cref="_AMsgDetailInfo"/>
    /// </remarks>
    public abstract class _AMsgDetailInfo<T_CONTENT, T_SENDER> : _AMsgDetailInfo<T_CONTENT>, _IMsgDetailInfoSender<T_SENDER>
        where T_CONTENT : _IALProtocolStructure, new()
        where T_SENDER : _IALProtocolStructure, new()
    {
        // 发送者的数据
        [NotNull] private readonly T_SENDER _m_sender;
        
        protected _AMsgDetailInfo(int _msgType) : base(_msgType)
        {
            _m_sender = new T_SENDER();
        }
        /// <summary>
        /// 发送者的数据
        /// </summary>
        [NotNull] public T_SENDER sender { get { return _m_sender; } }
        
        /// <inheritdoc />
        public override byte[] getSenderBytesData()
        {
            return _m_sender.makePackage();
        }
        /// <inheritdoc />
        public override void readByBytes(byte[] _content, byte[] _sender)
        {
            base.readByBytes(_content, _sender);
            _m_sender.readPackage(new ALProtocolBuf(_sender));
        }
    }
    
    public interface _IMsgDetailInfoSender<out T_SENDER>
        where T_SENDER : _IALProtocolStructure, new()
    {
        T_SENDER sender { get; }
    }
}