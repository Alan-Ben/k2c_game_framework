
using System;
using System.Collections.Generic;

using ChatPackage.Internal;
using GOE;
using Unity.Mathematics;

namespace ChatPackage
{
    /// <summary>
    /// 私聊对象的抽象
    /// </summary>
    /// <remarks>
    /// 继承自<c>_AChatInfo</c>，内部含有本地聊天记录保存与读取
    /// <para>更多内容可以阅读<see cref="_AChatInfo"/>来了解</para>
    /// <para>需要注意_initByUnreadMsg方法，_discard方法和_onReceiveMsg方法在这里被重载为有事件的注册和注销，覆盖这三个方法的时候请注意这一点</para>
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
    public abstract class _APrivateChatInfo : _AChatInfo
    {

        //会话唯一标识 自己的token+对方的token
        private string _m_chatInfoTag;

        //聊天对象相关信息
        private string _m_otherUserTag;

        // 使用聊天对象唯一标识来创建私聊
        protected _APrivateChatInfo(string _chatInfoTag, string _otherUserTag)
        {
            _m_chatInfoTag = _chatInfoTag;
            _m_otherUserTag = _otherUserTag;
        }

        public string chatInfoTag { get { return _m_chatInfoTag; } }
        public string otherUserTag { get { return _m_otherUserTag; } }

        /// <inheritdoc/>
        public override string id { get { return _m_otherUserTag; } }

        /// <inheritdoc/>
        protected override void _initByUnreadMsg(Action<List<MsgInfo>> _action)
        {
            onReceiveMsg += _onReceiveMsg;
            string otherUserChatUid = ChatUtility.makeChatUid(dataMgr.chatData.systemId, dataMgr.chatData.systemTag, _m_otherUserTag);
            dataMgr?.reqPrivateUnreadMsg(otherUserChatUid, (_unreadMsgList) =>
            {
                if (_unreadMsgList is { Count: > 0 })
                {
                    // 服务端给过来的消息是从新到旧，客户端内部全部默认是从旧到新，所以需要反转一下
                    _unreadMsgList.Reverse();
                    dataMgr.reqConfirmPrivateMsg(otherUserChatUid, _unreadMsgList.GetLast()?.msgId ?? 0);
                    HistorySaverMgr.instance.saveNewMsg(_m_chatInfoTag, _unreadMsgList);
                    _action?.Invoke(_unreadMsgList);
                }
                else
                    _action?.Invoke(new List<MsgInfo>(0));
            });
        }

        /// <inheritdoc/>
        protected override void _onDiscard()
        {
            onReceiveMsg -= _onReceiveMsg;
        }
        /// <inheritdoc/>
        protected override void _getHistoryList(long _msgId, int _msgCount, Action<List<MsgInfo>> _action)
        {
            if (_action == null)
                return;
            
            HistorySaverMgr.instance.getMsgListBefore(_m_chatInfoTag, _msgId, _msgCount, dataList =>
            {
                if (null == dataList)
                    return;

                List<MsgInfo> infoList = new List<MsgInfo>();
                if (dataList.Count > 0)
                {
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        HistorySaverData temp = dataList[i];
                        if (temp == null)
                            continue;
                        
                        MsgInfo info = dataMgr?.createMsgInfo(temp);
                        if (info != null)
                            infoList.Add(info);
                    }
                }
                
                _action(infoList);
            });
        }
        /// <summary>
        /// 当接收到新消息时的处理
        /// </summary>
        /// <param name="_msgInfo">新消息的内容</param>
        protected virtual void _onReceiveMsg(MsgInfo _msgInfo)
        {
            HistorySaverMgr.instance.saveNewMsg(_m_chatInfoTag, _msgInfo);
        }
    }
}