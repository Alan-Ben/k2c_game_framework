
namespace ChatPackage
{
    /// <summary>
    /// 某个msgType对应的设置
    /// </summary>
    /// <remarks>
    /// <para>你需要实现这个类来设置对应的msgType需要用什么detailInfo进行解析，并使用ChatData.setMsgInfoSetting注册到聊天包当中，详情可以查看<see cref="Chat"/></para>
    /// <para>注意：你应该要对每一个数据层会用到的msgType都实现一个这个类，考虑到你可以自定义只用于UI的msgType所以并不是每一个msgType都需要实现这个类，关于这方面可以进一步理解<see cref="_IMsgItemData"/>和<see cref="_AMsgDetailInfo"/>的区别</para>
    /// <para>你需要实现：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>createMsgDetailInfo</term>
    ///             <description>
    ///             你需要在这里构建一个DetailInfo，用于解析这个msgType的content数据
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public abstract class _AChatDataSetting
    {
        // 该设置的聊天类型
        private int _m_iMsgType;

        // 指定该设置对应的msgType
        protected _AChatDataSetting(int _msgType)
        {
            _m_iMsgType = _msgType;
        }
        
        /// <summary>
        /// 该设置对应的聊天消息的类型
        /// </summary>
        public int msgType { get { return _m_iMsgType; } }

        /// <summary>
        /// 构建一个DetailInfo，用于解析这个msgType的content数据
        /// </summary>
        public abstract _AMsgDetailInfo createMsgDetailInfo();
    }
}