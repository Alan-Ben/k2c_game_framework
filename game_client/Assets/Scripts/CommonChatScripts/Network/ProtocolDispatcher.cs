
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 聊天客户端的消息分发器
    /// </summary>
    /// <remarks>
    /// 这是个单例类，会被<see cref="ChatClient"/>所调用，用来分流处理从服务器收到的数据
    /// </remarks>
    internal class ProtocolDispatcher : ALBasicProtocolDispather
    {
        // 单例的访问器
        [NotNull] internal static ProtocolDispatcher instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new ProtocolDispatcher();
                return _g_instance;
            }
        }
        private static ProtocolDispatcher _g_instance;
        
        // 构造时把所有的处理器都添加
        private ProtocolDispatcher()
        {
            // 添加基础内容的处理器
            RegProtocol(new ProtocolMainDealer_001_BasicOp());
            // 添加有关消息请求的处理器
            RegProtocol(new ProtocolMainDealer_002_MsgOp());
        }
    }
}