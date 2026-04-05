
namespace ChatPackage
{
    /// <summary>
    /// 可以用于<see cref="GUISubWndChatMsgList"/>显示的数据类型
    /// </summary>
    /// <remarks>
    /// 在你实现了这个接口之后，你就可以把你的数据传入<see cref="GUISubWndChatMsgList"/>中用于显示
    /// <para>但是在那之前，你要先确保你这个消息是否有使用registerCache注册显示item，更多内容可以看<see cref="Chat"/>类</para>
    /// <para>需要注意：不要和<see cref="_AMsgDetailInfo"/>混淆了，这个类是UI的数据，你还可以自定义UI的数据让列表来显示，而<c>_AMsgDetailInfo</c>是数据层的消息类，这个类表示的都是真正的消息，而不是用来显示的数据</para>
    /// <para>举个例子，假如你的聊天消息列表要插入时间的显示，那你应该创建一个时间的数据类，并实现这个接口。这时就和<c>_AMsgDetailInfo</c>无关了</para>
    /// </remarks>
    public interface _IMsgItemData
    {
        /// <summary>
        /// 消息的类型
        /// </summary>
        int msgType { get; }
        /// <summary>
        /// 是否为自己发送的消息
        /// </summary>
        /// <remarks>
        /// 你可能会想利用这个字段来注册两种类型的item，比如他人发送的消息在左边，自己发送的消息在中间，上面的msgType再加上这个字段，想必已经足够了
        /// </remarks>
        bool isMyMsg { get; }
    }
}