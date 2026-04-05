
namespace ChatPackage.Internal
{
    /// <summary>
    /// 聊天会话中的一条消息
    /// </summary>
    /// <remarks>
    /// <para>这个类不包含具体的数据字段，而所有的数据内容会在detailInfo中，如果你需要使用detailInfo，可以根据detailInfo的msgType来转换成具体的类型，详情可以查看<see cref="_AMsgDetailInfo"/></para>
    /// <para>需要注意，这个类都是由服务端的推送或回包产生的，也意味着聊天会话的消息列表只信任来自服务端的消息</para>
    /// <para>主要使用：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>msgId</term>
    ///             <description>
    ///             这条消息的唯一id，会由服务端生成
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>detailInfo</term>
    ///             <description>
    ///             这条消息的具体数据内容，你可以根据内部的msgType来转换成具体的类型
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class MsgInfo
    {
        // 消息的唯一id
        private readonly long _m_lMsgId;
        // 这条消息产生的时间
        private readonly long _m_lTimeMs;
        // 消息的具体内容
        private readonly _AMsgDetailInfo _m_detailInfo;

        // 使用唯一id和具体内容构建
        public MsgInfo(long _msgId, long _timeMS, _AMsgDetailInfo _detailInfo)
        {
            _m_lMsgId = _msgId;
            _m_lTimeMs = _timeMS;
            _m_detailInfo = _detailInfo;
        }

        /// <summary>
        /// 这条消息的唯一id
        /// </summary>
        public long msgId { get { return _m_lMsgId; } }
        /// <summary>
        /// 这条消息产生的时间
        /// </summary>
        public long timeMs { get { return _m_lTimeMs; } }
        /// <summary>
        /// 这条消息的具体数据内容
        /// </summary>
        public _AMsgDetailInfo detailInfo { get { return _m_detailInfo; } }
    }
}