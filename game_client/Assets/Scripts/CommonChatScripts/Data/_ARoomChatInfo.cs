
using System;
using System.Collections.Generic;
using ChatPackage.Internal;

namespace ChatPackage
{
    /// <summary>
    /// 聊天室的会话类
    /// </summary>
    /// <remarks>
    /// <para>继承自<c>_AChatInfo</c>，对比父类没有新增的功能，更多内容可以看<see cref="_AChatInfo"/></para>
    /// <para>这个类有基本的聊天室功能，包括接收消息，获取聊天记录</para>
    /// <para>你主要需要实现：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>_sendMsg</term>
    ///             <description>
    ///             发送消息的方法，因为聊天服务器把这个部分交给了游戏服务器处理，所以你需要在这里代替聊天包做相应的处理
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public abstract class _ARoomChatInfo : _AChatInfo
    {
        // 聊天室id
        private readonly long _m_lRoomId;

        // 使用聊天室id构造
        public _ARoomChatInfo(long _roomId)
        {
            _m_lRoomId = _roomId;
        }

        /// <inheritdoc/>
        public override string id { get { return _m_lRoomId.ToString(); } }
        /// <summary>
        /// 聊天室id
        /// </summary>
        /// <remarks>
        /// 和id的数据是一样的，一个是string类型，一个是long
        /// </remarks>
        public long roomId { get { return _m_lRoomId; } }

        /// <inheritdoc/>
        protected override void _initByUnreadMsg(Action<List<MsgInfo>> _action)
        {
            _action?.Invoke(null);
        }
        /// <inheritdoc/>
        protected override void _onDiscard()
        {
        }
        /// <inheritdoc/>
        protected override void _getHistoryList(long _msgId, int _msgCount, Action<List<MsgInfo>> _action)
        {
            dataMgr?.getRoomChatHistory(_m_lRoomId, _msgId, _msgCount, _action);
        }
    }
}