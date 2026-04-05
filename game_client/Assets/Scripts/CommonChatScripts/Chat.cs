
using System;
using ALPackage;
using ALBasicProtocolPack;
using ChatPackage.Internal;
using JetBrains.Annotations;

namespace ChatPackage
{
    /// <summary>
    /// 聊天包的主要功能汇总
    /// </summary>
    /// <remarks>
    /// <para>你可以在这里找到聊天包拥有的主要功能</para>
    /// <para>主要使用方法：</para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>makeChatData</term>
    ///         <description>
    ///         构建一个<see cref="ChatData"/>，<see cref="ChatData"/>这个类包含了所有聊天包的数据层逻辑，为了使用聊天包的数据层，你需要通过构建这个类来使用这个类的相关接口
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>setMsgInfoSetting</term>
    ///         <description>
    ///         为了正常使用聊天包，聊天包需要知道如何解析所有你有用到的聊天消息，用这个方法注册你想要用哪个类来解析对应的聊天消息
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>setMsgWndSetting</term>
    ///         <description>
    ///         如果你要使用<see cref="GUISubWndChatMsgList"/>来作为聊天列表的显示，你必须先添加对应的msgInfo使用什么资源进行展示，你可以使用这个方法设定这一项
    ///         </description>
    ///     </item>
    /// </list>
    /// <para>你需要注意<b>setMsgInfoSetting</b>和<b>setMsgWndSetting</b>里注册的关于对应消息类型的数据类需要一模一样，否则聊天包会弹出debugOnly的错误提示</para> 
    /// <para>聊天包会对私聊的消息进行本地存档，而聊天室消息则不会（暂时是这样，原因和服务端的结构有关）</para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>clearAllHistory</term>
    ///         <description>
    ///         你可以使用这个方法来清空所有的本地存档消息
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>clearHistory</term>
    ///         <description>
    ///         从聊天包中移除聊天会话，可以移除聊天室和私聊，对于私聊请确保这个会话大概率不会收到新消息了再移除
    ///         </description>
    ///     </item>
    /// </list>
    /// <para>你还可以注册一些额外的委托</para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>sendMsgLogFunc</term>
    ///         <description>
    ///         你可以用这个委托来处理发送网络请求时的日志
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>receiveMsgLogFunc</term>
    ///         <description>
    ///         你可以用这个委托来处理收到网络回包/推送时的日志
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>onNetError</term>
    ///         <description>
    ///         你可以用这个委托来处理网络回包返回的异常码
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    public static class Chat
    {
        /// <summary>
        /// 这个委托用来处理发送网络请求时的日志，会在发送网络请求时被调用
        /// </summary>
        public static Action<_IALProtocolStructure> sendMsgLogFunc;
        /// <summary>
        /// 这个委托用来处理收到网络回包/推送时的日志，会在收到回包或推送时被调用
        /// </summary>
        public static Action<_IALProtocolStructure> receiveMsgLogFunc;
        /// <summary>
        /// 这个委托用来处理网络回包返回的异常码
        /// </summary>
        public static Action<int> onNetError;

        /// <summary>
        /// 生成一个聊天数据包
        /// </summary>
        /// <remarks><see cref="ChatData"/>这个类包含聊天包的所有数据层逻辑，为了使用聊天包的内容，你必须要拿到这个ChatData</remarks>
        /// <param name="_id">客户端的唯一id，游戏服务器会给</param>
        /// <param name="_ip">目标聊天服务器的ip，游戏服务器会给</param>
        /// <param name="_port">目标聊天服务器的端口，游戏服务器也会给</param>
        /// <param name="_checkCode">连接所需的密钥，游戏服务器也会给</param>
        /// <param name="_newChatInfoDelegate">产生新私聊的委托，你需要自己写一个这样的方法注册进来</param>
        [NotNull]
        public static ChatData makeChatData(long _systemId,string _systemTag, string _uid, string _ip, int _port, string _checkCode, ChatDataMgr.OnReceiveNewChatInfo _newChatInfoDelegate)
        {
            return new ChatData(_systemId,_systemTag, _uid, _ip, _port, _checkCode, _newChatInfoDelegate);
        }


        /// <summary>
        /// 清除所有本地存档的历史消息
        /// </summary>
        public static void clearAllHistory()
        {

        }

        /// <summary>
        /// 清除指定的聊天会话的历史消息
        /// </summary>
        /// <param name="_chatInfoId">聊天会话的标识</param>
        public static void clearHistory(string _chatInfoId)
        {
        }
    }    
}