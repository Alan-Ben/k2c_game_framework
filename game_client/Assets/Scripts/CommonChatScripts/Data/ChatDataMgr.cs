
using System;
using System.Collections.Generic;
using Common.CommObj;
using GC2GS.p002_MsgOp;
using JetBrains.Annotations;
using GS2GC.p002_MsgOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 聊天包的数据管理器
    /// </summary>
    /// <remarks>
    /// <para>这个数据类会由<see cref="ChatData"/>构建，内部的public的接口大部分都被<see cref="ChatData"/>类所覆盖，详细的可以查看<see cref="ChatData"/>类</para>
    /// <para>主要方法：</para>
    /// <list type="bullet">
    ///         <item>
    ///             <term>ChatInfo的add，remove，get</term>
    ///             <description>
    ///             一个会话类必须使用这个方法注册进来才可以开始正常运作，你可以在这个类中找到管理注册情况的接口，你也可以使用<see cref="Chat"/>下的相关接口
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>sendRoomMsg</term>
    ///             <description>
    ///             你可以使用这个方法直接往指定的聊天室发送消息，正常这个方法会被<see cref="_ARoomChatInfo"/>调用
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>getRoomChatHistory</term>
    ///             <description>
    ///             你可以使用这个方法直接获取指定聊天室的历史消息，正常这个方法会被<see cref="_ARoomChatInfo"/>调用
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>reqPrivateUnreadMsg</term>
    ///             <description>
    ///             你可以使用这个方法获取私聊的未读列表，但是获取了之后，服务器就会删除这些消息，如果你不做合适的处理你有可能会丢失这些消息
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class ChatDataMgr
    {
        /// <summary>
        /// 当产生新的私聊时构建一个私聊结构的委托
        /// </summary>
        /// <remarks>
        /// 当一个新的聊天对象给你发送消息时，就产生了一个新的私聊，这时会由聊天包来触发这个逻辑，而具体执行的内容就是这个委托的内容
        /// </remarks>
        public delegate _AChatInfo OnReceiveNewChatInfo(string _chatInfoTag, string _otherUserTag);
        
        // 当产生新的私聊时构建一个私聊结构的委托
        private readonly OnReceiveNewChatInfo _m_aOnReceiveNewChatInfo;
        // 聊天id映射聊天会话的字典
        [NotNull] private readonly Dictionary<string, _AChatInfo> _m_id2ChatInfo;
        // 用来对上网络回包的字典，为序列号到回包处理委托的映射
        [NotNull] private readonly Dictionary<long, Action<List<MsgInfo>>> _m_msgListSerializeDelegate;
        // 自增的序列号，给上面的字典生成序列号用
        private long _m_lMsgListSerialize;
        // 聊天的数据包，拥有数据层的主要功能接口
        [NotNull] private readonly ChatData _m_chatData;
        // 聊天数据类型的设置类
        [NotNull] private readonly ChatMsgInfoSettingMgr _m_settingMgr;

        // 使用数据包的实例和产生私聊时的委托来构建这个管理器
        public ChatDataMgr([NotNull] ChatData _chatData, OnReceiveNewChatInfo _delegate)
        {
            // 赋值
            _m_chatData = _chatData;
            _m_aOnReceiveNewChatInfo = _delegate;
            
            // 构建基础成员
            _m_msgListSerializeDelegate = new Dictionary<long, Action<List<MsgInfo>>>();
            _m_id2ChatInfo = new Dictionary<string, _AChatInfo>();
            _m_settingMgr = new ChatMsgInfoSettingMgr();
        }
        
        /// <summary>
        /// 聊天数据包，拥有数据层的主要功能接口
        /// </summary>
        [NotNull] public ChatData chatData { get { return _m_chatData; } }
        /// <summary>
        /// 只要当内存中出现一条新消息就会触发一次
        /// </summary>
        public event Action<MsgInfo> onGetNewMsg;

        /// <summary>
        /// 注册一个聊天会话到聊天数据层
        /// </summary>
        public bool addChatInfo(_AChatInfo _chatInfo)
        {
            // 如果为null就不处理
            if (_chatInfo == null)
                return false;

            // 重复注册预防
            string id = _chatInfo.id;
            if (string.IsNullOrEmpty(id))
            {
                ChatUtility.logError_DebugOnly($"你添加的chatInfo的id无效，id为null或为空字符串");
                return false;
            }
            
            if (_m_id2ChatInfo.ContainsKey(id))
            {
                ChatUtility.logError_DebugOnly($"你已经添加过id为（{id}）的聊天会话了，请不要重复添加");
                return false;
            }

            // 注册到数据层
            _m_id2ChatInfo.Add(id, _chatInfo);
            // 让聊天会话开始加载
            _chatInfo.setDataMgr(this);
            _chatInfo.load();

            return true;
        }
        /// <summary>
        /// 从聊天数据层移除一个会话
        /// </summary>
        /// <returns>返回是否成功</returns>
        public bool removeChatInfo(string _chatInfoId)
        {
            // 尝试获取对应id的聊天会话
            if (!string.IsNullOrEmpty(_chatInfoId) && _m_id2ChatInfo.TryGetValue(_chatInfoId, out _AChatInfo chatInfo) && chatInfo != null)
            {
                // 销毁这个聊天会话
                chatInfo.resetDataMgr();
                chatInfo.discard();
                // 从数据层移除
                return _m_id2ChatInfo.Remove(_chatInfoId);
            }

            // 移除失败
            return false;
        }
        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void clearAllData()
        {
            _m_msgListSerializeDelegate.Clear();
            foreach (_AChatInfo chatInfo in _m_id2ChatInfo.Values)
            {
                if (chatInfo == null)
                    continue;
                
                // 销毁这个聊天会话
                chatInfo.resetDataMgr();
                chatInfo.discard();
            }
            _m_id2ChatInfo.Clear();
        }
        /// <summary>
        /// 获取聊天数据层中的一个会话
        /// </summary>
        public _AChatInfo getChatInfo(string _chatInfoId)
        {
            // 如果key无效就返回null
            if (string.IsNullOrEmpty(_chatInfoId))
                return null;
            
            // 尝试获取
            _m_id2ChatInfo.TryGetValue(_chatInfoId, out _AChatInfo chatInfo);
            return chatInfo;
        }
        /// <summary>
        /// 获取所有会话
        /// </summary>
        public void getAllChatInfo(List<_AChatInfo> _list)
        {
            if (_list == null)
                return;

            _list.AddRange(_m_id2ChatInfo.Values);
        }
        
        /// <summary>
        /// 请求指定房间的历史消息
        /// </summary>
        /// <param name="_roomId">房间id</param>
        /// <param name="_msgId">需要哪一条消息往前的消息，如果要最新的，传入-1即可</param>
        /// <param name="_msgCount">需要多少条消息</param>
        /// <param name="_action">获取成功后的回调</param>
        public void getRoomChatHistory(long _roomId, long _msgId, int _msgCount, Action<List<MsgInfo>> _action)
        {
            // 如果回调都没有，或者一条消息都不要，就直接返回
            if (_action == null || _msgCount <= 0)
            {
                _action?.Invoke(null);
                return;
            }

            // 自增序列号
            long serialize = _m_lMsgListSerialize++;
            // 根据序列号添加到字典中，等到回包时调用
            _m_msgListSerializeDelegate.Add(serialize, _action);
            // 发送获取历史消息请求
            _m_chatData.chatClient?.sendMsg(new GC2GS_002_001_ReqRoomHistory(serialize, _roomId, _msgId, _msgCount));
        }
        /// <summary>
        /// 请求获取指定私聊的未读消息
        /// </summary>
        /// <param name="_targetTag">指定对应的私聊会话</param>
        /// <param name="_action">收到之后的回调</param>
        public void reqPrivateUnreadMsg(string _targetTag, Action<List<MsgInfo>> _action)
        {
            // 如果回调都没有，或者目标私聊id是空的，就直接不处理
            if (_action == null || string.IsNullOrEmpty(_targetTag))
            {
                _action?.Invoke(null);
                return;
            }

            // 自增序列号
            long serialize = _m_lMsgListSerialize++;
            // 根据序列号添加到字典中，等到回包时调用
            _m_msgListSerializeDelegate.Add(serialize, _action);
            // 发送请求
            _m_chatData.chatClient?.sendMsg(new GC2GS_002_005_ReqUnreadPrivateMsgList(serialize, _targetTag));
        }
        /// <summary>
        /// 获取当前所有的未读私聊会话
        /// </summary>
        public void reqPrivateUnreadMsgBrief()
        {
            // 自增序列号
            long serialize = _m_lMsgListSerialize++;
            // 发送请求
            _m_chatData.chatClient?.sendMsg(new GC2GS_002_004_ReqUnreadPrivateMsgUserList(serialize));
        }
        
        /// <summary>
        /// 确认私聊消息，确认后服务端移除数据
        /// </summary>
        public void reqConfirmPrivateMsg(string _senderChatUid, long _confirmMsgId)
        {
            // 自增序列号
            long serialize = _m_lMsgListSerialize++;
            // 发送请求
            _m_chatData.chatClient?.sendMsg(new GC2GS_002_006_ReqConfirmPrivateMsg(serialize,_senderChatUid, _confirmMsgId));
        }
        
        /// <summary>
        /// 设置一个msgType对应的消息解析类
        /// </summary>
        public void setMsgInfoSetting(int _msgType, _AChatDataSetting _setting)
        {
            _m_settingMgr.setMsgInfoSetting(_msgType, _setting);
        }

        /// <summary>
        /// 当收到了聊天室历史消息的回包
        /// </summary>
        internal void retRoomChatHistory(GS2GC_002_001_RetRoomHistory _msg)
        {
            if (_msg == null)
                return;

            // 获取回包中的消息列表
            List<RoomMsg> roomMsgList = _msg.getRoomMsgList();
            if (roomMsgList == null)
                return;

            // 开始构建本地的MsgInfo（服务端返回从新到旧，倒序遍历使结果从旧到新）
            List<MsgInfo> historyMsg = new List<MsgInfo>();
            for (int i = roomMsgList.Count - 1; i >= 0; i--)
            {
                RoomMsg roomMsg = roomMsgList[i];
                if (roomMsg == null)
                    continue;

                MsgInfo msgInfo = _m_settingMgr.createMsgInfo(roomMsg.getMsgId(), roomMsg.getTimeMs(), roomMsg.getMsg());
                if (msgInfo == null)
                {
                    ChatUtility.logError_DebugOnly($"无法构建msgType为（{roomMsg.getMsg()?.getMsgType()}）的消息类，请先使用ChatData.setMsgInfoSetting来先设置");
                    continue;
                }
                historyMsg.Add(msgInfo);
            }
            // 获取客户端发送时给的序列号
            long serialize = _msg.getCallBackSerial();
            // 根据序列号获取对应的委托
            if (_m_msgListSerializeDelegate.TryGetValue(serialize, out Action<List<MsgInfo>> action))
            {
                // 使用委托处理对应的列表
                action?.Invoke(historyMsg);
                // 处理完之后移除对应的序列号
                _m_msgListSerializeDelegate.Remove(serialize);
            }
        }
        /// <summary>
        /// 收到私聊的未读消息回包
        /// </summary>
        internal void retPrivateUnreadMsg(GS2GC_002_005_RetUnreadPrivateMsgList _msg)
        {
            // 获取服务器的消息列表
            List<PrivateChatMsg> privateMsgList = _msg?.getMsgList();
            if (privateMsgList == null)
                return;

            // 构建临时的消息列表（服务端返回从新到旧，倒序遍历使结果从旧到新）
            List<MsgInfo> unreadMsg = new List<MsgInfo>();
            for (int i = privateMsgList.Count - 1; i >= 0; i--)
            {
                PrivateChatMsg privateMsg = privateMsgList[i];
                if (privateMsg == null)
                    continue;

                MsgInfo msgInfo = _m_settingMgr.createMsgInfo(privateMsg.getMsgId(), privateMsg.getTimeMs(), privateMsg.getMsg());
                if (msgInfo == null)
                {
                    ChatUtility.logError_DebugOnly($"无法构建msgType为（{privateMsg.getMsg()?.getMsgType()}）的消息类，请先使用ChatData.setMsgInfoSetting来先设置");
                    continue;
                }
                unreadMsg.Add(msgInfo);
            }
            // 获取发送时客户端生成的序列号
            long serialize = _msg.getCallBackSerial();
            // 使用序列号取出对应的委托
            if (_m_msgListSerializeDelegate.TryGetValue(serialize, out Action<List<MsgInfo>> action))
            {
                // 让委托执行处理
                action?.Invoke(unreadMsg);
                // 处理完之后移除对应的序列号
                _m_msgListSerializeDelegate.Remove(serialize);
            }
        }
        /// <summary>
        /// 当收到了聊天室新消息的推送
        /// </summary>
        internal void onReceiveRoomMsg(GS2GC_002_050_OnReceiveRoomMsg _msg)
        {
            // 处理这一条新消息
            RoomMsg roomMsg = _msg?.getMsg();
            if (roomMsg == null)
                return;

            // 使用服务器给的新消息构建客户端用的MsgInfo
            MsgInfo msgInfo = _m_settingMgr.createMsgInfo(roomMsg.getMsgId(), roomMsg.getTimeMs(), roomMsg.getMsg());
            if (msgInfo == null)
            {
                ChatUtility.logError_DebugOnly($"无法构建msgType为（{roomMsg.getMsg()?.getMsgType()}）的消息类，请先使用ChatData.setMsgInfoSetting来先设置");
                return;
            }

            // 尝试获取对应的聊天室
            _AChatInfo chatInfo = getChatInfo(roomMsg.getRoomId().ToString());
            // 让聊天会话处理收到新消息
            chatInfo?.recieveMsg(msgInfo);
            onGetNewMsg?.Invoke(msgInfo);
        }
        /// <summary>
        /// 获取当前所有未读消息的会话
        /// </summary>
        /// <param name=""></param>
        internal void retPrivateUnreadMsgBrief(GS2GC_002_004_RetUnreadPrivateMsgUserList _brief)
        {
            List<PrivateChatUser> briefInfoList = _brief?.getUserList();
            if (briefInfoList == null || briefInfoList.Count == 0)
                return;

            for (int i = 0; i < briefInfoList.Count; i++)
            {
                PrivateChatUser temp = briefInfoList[i];
                if (temp == null)
                    continue;
                string senderGameClientUid = _getGameClientUid(temp.getSenderChatUid());
                // 尝试获取对应的私聊会话
                _AChatInfo chatInfo = getChatInfo(senderGameClientUid);

                // 如果没有获取到，就尝试生成一个新的私聊会话
                if (chatInfo == null)
                { 
                    // 如果产生新会话的委托没有注册，弹出提示并返回
                    if (_m_aOnReceiveNewChatInfo == null)
                    {
                        ChatUtility.logError_DebugOnly($"当产生了新的私聊时的委托不存在，这样会导致这条消息永远的丢失，你应该在Chat.start的时候传入委托，" +
                                                       $"这个委托应该要在产生新的私聊时能正确返回一个私聊的会话对象");
                        return;
                    }
                    // 让委托产生一个新的会话
                    chatInfo = _m_aOnReceiveNewChatInfo.Invoke(_m_chatData.gameClientUid + senderGameClientUid,senderGameClientUid);
                    // 添加会话到数据层
                    addChatInfo(chatInfo);
                }
            }
        }

        /// <summary>
        /// 拆解chatUid获得游戏内传入的客户端uid
        /// </summary>
        /// <param name="_chatUid"></param>
        /// <returns></returns>
        private string _getGameClientUid(string _chatUid)
        {
            string[] chatUidSplit = ChatUtility.splitChatUid(_chatUid);
            if (chatUidSplit.Length >= 3)
            {
                return chatUidSplit[2];
            }
            return null;
        }

        /// <summary>
        /// 当收到了私聊新消息的推送
        /// </summary>
        internal void onReceivePrivateMsg(GS2GC_002_051_OnReceivePrivateMsg _msg)
        {
            // 处理收到的新消息
            PrivateChatMsg privateChatMsg = _msg?.getMsg();
            if (privateChatMsg == null)
                return;

            // 使用服务器给的新消息构建客户端用的MsgInfo
            MsgInfo msgInfo = _m_settingMgr.createMsgInfo(privateChatMsg.getMsgId(),privateChatMsg.getTimeMs(), privateChatMsg.getMsg());
            if (msgInfo == null)
            {
                ChatUtility.logError_DebugOnly($"无法构建msgType为（{privateChatMsg.getMsg()?.getMsgType()}）的消息类，请先使用ChatData.setMsgInfoSetting来先设置");
                return;
            }
            string senderGameClientUid = _getGameClientUid(privateChatMsg.getSenderChatUid());
            string receiverGameClientUid = _getGameClientUid(privateChatMsg.getReceiverChatUid());
            // 尝试获取对应的私聊会话
            string userTag = receiverGameClientUid;
            string chatInfoTag = senderGameClientUid + receiverGameClientUid;
            if (_m_chatData.uid == privateChatMsg.getReceiverChatUid())//如果自己是接收方，则发送方就是私聊的玩家
            {
                userTag = senderGameClientUid;
                chatInfoTag = receiverGameClientUid + senderGameClientUid;
            }

            _AChatInfo chatInfo = getChatInfo(userTag);

            // 如果没有获取到，就尝试生成一个新的私聊会话
            if (chatInfo == null)
            {
                // 如果产生新会话的委托没有注册，弹出提示并返回
                if (_m_aOnReceiveNewChatInfo == null)
                {
                    ChatUtility.logError_DebugOnly($"当产生了新的私聊时的委托不存在，这样会导致这条消息永远的丢失，你应该在Chat.start的时候传入委托，" +
                                                   $"这个委托应该要在产生新的私聊时能正确返回一个私聊的会话对象");
                    return;
                }

                //todo 让委托产生一个新的会话
                chatInfo = _m_aOnReceiveNewChatInfo.Invoke(chatInfoTag, userTag);
                // 添加会话到数据层
                if (chatInfo != null && addChatInfo(chatInfo))
                {
                    // 再会话加载完之后再处理收到这条消息
                    chatInfo.regLoadDoneDelegate(() =>
                    {
                        chatInfo.recieveMsg(msgInfo); 
                        onGetNewMsg?.Invoke(msgInfo);
                    });
                }
                else
                {
                    ChatUtility.logError_DebugOnly($"产生了新的私聊时的委托构建了一个ChatInfo，但是却添加失败了");
                }
            }
            else
            { 
                // 如果会话本来就存在，就直接处理收到新消息
                chatInfo.recieveMsg(msgInfo);
                onGetNewMsg?.Invoke(msgInfo);
            }
        }
        // 当聊天会话获得新历史消息时
        internal void onChatInfoGetNewHistoryMsgList(List<MsgInfo> _msgList)
        {
            if (onGetNewMsg != null && _msgList != null)
            {
                for (int i = 0; i < _msgList.Count; i++)
                {
                    onGetNewMsg.Invoke(_msgList[i]);
                }
            }
        }
            
        //创建msginfo
        public MsgInfo createMsgInfo(HistorySaverData _data)
        {
            return _m_settingMgr.createMsgInfo(_data);
        }
    }
}