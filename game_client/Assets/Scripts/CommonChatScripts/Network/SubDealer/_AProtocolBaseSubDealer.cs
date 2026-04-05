
using ALBasicProtocolPack;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 接受消息的处理类，统一逻辑的处理处
    /// </summary>
    /// <remarks>
    /// <para>这里放着SubDealer的通用处理</para>
    /// </remarks>
    internal abstract class _AProtocolBaseSubDealer<T> : _AALBasicProtocolSubOrderDealer<T>
        where T : _IALProtocolStructure
    {
        /// <summary>
        /// 实现这个方法来处理收到的协议
        /// </summary>
        protected abstract void _dealProtocolByLog(ChatClient _dealer, T _msg);
        /// <summary>
        /// 收到协议之后的处理，这里加入了所有SubDealer都具有的通用逻辑
        /// </summary>
        protected sealed override void _dealProtocol(_IALProtocolDealer _dealer, T _msg)
        {
            // 底层真正处理消息的逻辑
            _dealProtocolByLog(_dealer as ChatClient, _msg);
            // 调用游戏客户端额外输出通讯日志的委托
            Chat.receiveMsgLogFunc?.Invoke(_msg);
            
#if UNITY_EDITOR
            if (null != _msg && _msg.GetFullPackBufSize() > 20 * 1024)
            {
                Debug.LogError($"协议大小超过20k: {_msg.GetType().Name}，大小：{_msg.GetFullPackBufSize()}");
            }
#endif
        }
    }
}