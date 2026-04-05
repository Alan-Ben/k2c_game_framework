
using NPEnum;
using System;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using ALBasicProtocolPack;
using GS2GC.p022_ChatOp;

namespace GOE
{
    /// <summary>
    /// 聊天组件
    /// </summary>
    /// <remarks>
    /// 负责和聊天包的数据接入
    /// </remarks>
    public partial class NPPlayerChatComponent : _ANPBasicPlayerComponent
    {
        // 聊天包的数据对象
        private ChatData _m_chatData;
        // 当前的聊天室对象
        [NotNull] private List<NPRoomChatInfo> _m_chatRoomList;
        //私聊对象列表
        [NotNull] private List<NPPrivateChatInfo> _m_chatPrivateList;

        [NotNull] private ChatServerLoginDataCallback _m_chatServerLoginDataCallback;

        //账号本地存储对象
        private HistoryAccountSaver _m_accountSaver;

        // [NotNull] private Dictionary<long, ChatPlayerInfo> _m_chatPlayerDic;
        private long _m_initSerialize;

        private RedTipDealer _m_redTipDealer;

        private Dictionary<EChatShareType, long> _m_shareDic;
        
        [NotNull] private List<RoomJoinInfo> _m_lNeedJoinRoomList = new List<RoomJoinInfo>();//需要加入的聊天室列表

        /// <summary>
        /// 聊天数据创建的时候，用于可能的短线重连，重新创建数据，或者一开始没连上，等到界面已经显示后才连上的时候刷新界面用
        /// </summary>
        public Action onChatDataCreat;

        public NPPlayerChatComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_chatRoomList = new List<NPRoomChatInfo>();
            _m_chatServerLoginDataCallback = new ChatServerLoginDataCallback(this);
            _m_chatPrivateList = new List<NPPrivateChatInfo>();
            _m_shareDic = new Dictionary<EChatShareType, long>();
            _m_redTipDealer = new RedTipDealer(this);

            _m_lNeedJoinRoomList.Clear();
        }

        //发送分享时间戳
        public Dictionary<EChatShareType,long> shareDic { get { return _m_shareDic; } }
        /// <summary>
        /// 聊天包的数据对象
        /// </summary>
        public HistoryAccountSaver accountSaver { get { return _m_accountSaver; } }
        public ChatData chatData { get { return _m_chatData; } }
        /// <summary>
        /// 聊天室的列表
        /// </summary>
        public List<NPRoomChatInfo> chatRoomList { get { return _m_chatRoomList; } }
        /// <summary>
        /// 当有新的聊天室开启时
        /// </summary>
        public event Action<NPRoomChatInfo> onChatRoomAdd;
        /// <summary>
        /// 当有聊天室关闭时
        /// </summary>
        public event Action<NPRoomChatInfo> onChatRoomRemove;
        /// <summary>
        /// 当有新的私聊开启时
        /// </summary>
        public event Action onPrivateChatAdd;
        /// <summary>
        /// 当有私聊关闭时
        /// </summary>
        public event Action onPrivateChatRemove;

        private ALStepCounter _m_initStepCounter;

        private _AChatInfo _m_curChatInfo;//当前显示的聊天频道
        
        // /// <summary>
        // /// 当玩家信息发生变化
        // /// </summary>
        // public event Action<NPCommon_ChatPlayerContent> onPlayerInfoChg;

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.CHAT; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        public _AChatInfo curChatInfo{ get { return (_m_curChatInfo == null) ? chatRoomList.GetFirst() : _m_curChatInfo; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _m_initStepCounter = new ALStepCounter();
            _m_initStepCounter.chgTotalStepCount(2);
            _m_initStepCounter.regAllDoneDelegate(setInitDone);
            _reqEmoteGroupInit();
            reqForbidChatInit();
        }

        protected override void _dealInit()
        {
            // 用了canPreInit之后这个方法就没用了（大概）
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _init();
            Chat.onNetError += _onChatNetError;
            Chat.receiveMsgLogFunc += _receiveMsgLog;
            Chat.sendMsgLogFunc += _sendMsgLog;
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerChatComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            Chat.onNetError -= _onChatNetError;
            Chat.receiveMsgLogFunc -= _receiveMsgLog;
            Chat.sendMsgLogFunc -= _sendMsgLog;

            // 序列号++
            _m_initSerialize = ALSerializeOpMgr.next();
            // 清楚聊天包
            if (_m_chatData != null)
            {
                // _m_chatData.getDataMgr().onGetNewMsg -= _onReceiveMsg;
                _m_chatData.onConnected -= _onConnectDone;
                _m_chatData.onConnectFailed -= _tryReconnect;
                _m_chatData.disconnect();
                _m_chatData.clearData();
                _m_chatData = null;
            }
            
            _m_lNeedJoinRoomList.Clear();
            
            _m_redTipDealer?.clear();
            
            _m_shareDic?.Clear();

            _m_lForbidChatList?.Clear();
        }
        
        
        //添加分享时间戳
        public void addChatShareTime(EChatShareType _shareType, long _timeS)
        {
            if (null == _m_shareDic)
                _m_shareDic = new Dictionary<EChatShareType, long>();

            if (_m_shareDic.ContainsKey(_shareType))
                _m_shareDic[_shareType] = _timeS;
            else
                _m_shareDic.Add(_shareType, _timeS);
        }
        
        /// <summary>
        /// 获取指定聊天室
        /// </summary>
        public NPRoomChatInfo getRoomChatInfo(long _id)
        {
            for (int i = 0; i < _m_chatRoomList.Count; i++)
            {
                NPRoomChatInfo roomInfo = _m_chatRoomList[i];
                if (roomInfo == null)
                    continue;

                if (roomInfo.roomId == _id)
                    return roomInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取指定聊天室
        /// </summary>
        public NPRoomChatInfo getRoomChatInfo(ENPChatRoomType _roomType)
        {
            for (int i = 0; i < _m_chatRoomList.Count; i++)
            {
                NPRoomChatInfo roomInfo = _m_chatRoomList[i];
                if (roomInfo == null)
                    continue;

                if (roomInfo.type == _roomType)
                    return roomInfo;
            }

            return null;
        }

        public void getAllPrivateChat(List<_AChatInfo> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_chatPrivateList);
        }

        /// <summary>
        /// 获取所有聊天室
        /// </summary>
        /// <param name="_list"></param>
        public void getAllChatInfo(List<_AChatInfo> _list)
        {
            if (null == _list)
                return;

            _m_chatData?.getAllChatInfo(_list);
        }

        /// <summary>
        /// 获取指定私聊
        /// </summary>
        public NPPrivateChatInfo getPrivateChatInfo(long _cid)
        {
            _AChatInfo chatInfo = _m_chatData?.getChatInfo<_AChatInfo>(_cid.ToString());
            if (null != chatInfo && chatInfo is NPPrivateChatInfo)
                return chatInfo as NPPrivateChatInfo;

            return null;
        }

        // /// <summary>
        // /// 获取玩家数据
        // /// </summary>
        // public NPCommon_ChatPlayerContent getPlayerInfo(long _cid)
        // {
        //     if (_m_chatPlayerDic.TryGetValue(_cid, out ChatPlayerInfo playerInfo) && playerInfo != null)
        //         return playerInfo.info;
        //
        //     return null;
        // }

        private void _createChatData(GS2GC_022_001_RetPlayerChatLogin _sucMsg)
        {
            if (_sucMsg == null)
                return;

            // 如果之前的chatData存在，就销毁掉
            if (_m_chatData != null)
            {
                // _m_chatData.getDataMgr().onGetNewMsg -= _onReceiveMsg;
                
                _m_chatData.onConnected -= _onConnectDone;
                _m_chatData.onConnectFailed -= _tryReconnect;
                _m_chatData.disconnect();
                _m_chatData.clearData();
            }

            _m_chatData = Chat.makeChatData(_sucMsg.getSystemId(), _sucMsg.getSystemTag(), NPPlayer.instance.playerInfo.CID.ToString(), _sucMsg.getIp(), _sucMsg.getPort(), _sucMsg.getCheckCode(), createPrivateChat);
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.TEXT, new NPChatTextDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.EMOTE, new NPChatEmoteDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SHARE_HERO, new NPChatShareHeroDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SHARE_CONSORT, new NPChatShareConsortDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SHARE_CONSORT_CG, new NPChatShareConsortCGDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SHARE_CHILD, new NPChatShareChildDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SHARE_MARS_EXPLORE_MINE, new NPChatShareMarsExploreMineDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SYSTEM, new NPChatSystemDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.COMM_BOX, new NPChatCommonBoxDataSetting());
            // _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.ADULT_MARRY_SERVER_APPLY, new NPChatAdultMarryDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.DINNER_INVITE, new NPChatDinnerInviteSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.GUILD_PRIVATE_INFORM, new ChatGuildPrivateInformMsgSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.GUILD_LOG, new ChatGuildLogMsgSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.GUILD_RECRUIT, new ChatGuildRecruitMsgSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.MIDDAY_DUNGEON_BOX, new NPChatMiddayDungeonBoxSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.EVENING_DUNGEON_BOX, new NPChatEveningDungeonBoxSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.SYSTEM_LOG, new ChatSystemLogMsgSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.ACTIVITY_RANK_BOX, new ChatActivityRankBoxDataSetting());
            _m_chatData.setMsgInfoSetting((int)ENPChatMsgType.GUILD_MARS_MINE_OCCUPY, new ChatGuildMarsMineOccupyMsgSetting());

            // _m_chatData.getDataMgr().onGetNewMsg += _onReceiveMsg;
            _m_chatData.connect(3);
            _m_chatData.onConnected += _onConnectDone;
            _m_chatData.onConnectFailed += _tryReconnect;
            
            //添加本地存储的私聊会话
            HistoryAccountSaver saver = new HistoryAccountSaver(NPPlayer.instance.playerInfo.CID);
            _m_chatPrivateList.Clear();
            if(null != saver)
            {
                saver.init();
                foreach (KeyValuePair<string,string> kv in saver.historyChatInfoTagDic)
                {
                    NPPrivateChatInfo chatInfo = new NPPrivateChatInfo(kv.Key,kv.Value);
                    _m_chatData.addChatInfo(chatInfo);
                    
                    _m_chatPrivateList.Add(chatInfo);
                    //设置已读的时间戳
                    long readTime = 0;
                    if (saver.historyChatReadTimeDic.ContainsKey(kv.Key))
                    {
                        readTime = saver.historyChatReadTimeDic[kv.Key];
                        chatInfo.setLaseReadTime(readTime);
                    }
                    //根据删除的时间戳，设置已读的时间戳，防止之前删除未设置已读
                    if (saver.historyChatRemoveTimeDic.ContainsKey(kv.Key))
                    {
                        readTime = saver.historyChatRemoveTimeDic[kv.Key];
                        if(readTime > chatInfo.lastReadTimeTag)
                            chatInfo.setLaseReadTime(readTime);
                    }
                    //设置是否置顶
                    chatInfo.setIsUpToTop(saver.historyUpToTopList.Contains(kv.Key));
                }
                foreach (KeyValuePair<long,long> kv in saver.historyRoomChatReadTimeDic)
                {
                    NPRoomChatInfo chatInfo = getRoomChatInfo(kv.Key);
                    if(chatInfo == null)
                        continue;
                    
                    //设置已读的时间戳
                    chatInfo.setLaseReadTime(kv.Value);
                }
            }
            _m_accountSaver = saver;

            _m_redTipDealer?.init();
            
            if (null != onChatDataCreat)
            {
                onChatDataCreat();
            }
        }

        private void _tryReconnect()
        {
            _connectToChatServer();
        }

        /// <summary>
        /// 聊天服务器连上后
        /// </summary>
        private void _onConnectDone()
        {
            //请求进入聊天室
            foreach (RoomJoinInfo chatRoomTypeInfo in _m_lNeedJoinRoomList)
            {
                _joinChatRoom(chatRoomTypeInfo.roomType, chatRoomTypeInfo.extId);
            }
        }

        private void _connectToChatServer()
        {
            // 序列号++
            _m_initSerialize = ALSerializeOpMgr.next();
            // 记录现在的序列号
            _m_chatServerLoginDataCallback.recordSerialize();
            
            // 正常流程当需要重连聊天服务器时, 聊天室也需要重连
            foreach (var chatRoom in _m_chatRoomList)
            {
                if(chatRoom == null)
                    continue;
                
                _m_chatData?.removeChatInfo(chatRoom.id);
                onChatRoomRemove?.Invoke(chatRoom);
            }
            _m_chatRoomList.Clear();
            
            // 登录聊天服务器，然后就不管了，就算没登上也不管
            NPGSClientListener.sendRequestByLog(GSWriter_022_ChatOp.make_001_ReqPlayerChatLogin(), _m_chatServerLoginDataCallback);
        }

        //创建一个新的私聊会话
        public NPPrivateChatInfo createPrivateChat(string _chatInfoTag,string _otherUserTag)
        {
            NPPrivateChatInfo chatInfo = getPrivateChatInfo(long.Parse(_otherUserTag));
            if (null != chatInfo && _m_chatPrivateList.Contains(chatInfo)) 
                return chatInfo;
            chatInfo = new NPPrivateChatInfo(_chatInfoTag, _otherUserTag);
            _m_chatPrivateList.Add(chatInfo);

            if (_m_accountSaver != null)
            {
                _m_accountSaver.addChatInfoTag(_chatInfoTag, _otherUserTag);
            }
            _m_redTipDealer?.onAddPrivateChatInfo(chatInfo);
            onPrivateChatAdd?.Invoke();
            return chatInfo;
        }

        //创建一个新的私聊会话
        public NPPrivateChatInfo createPrivateChat(long _cid)
        {
            string chatInfoTag = NPPlayer.instance.playerInfo.CID.ToString() + _cid.ToString();
            NPPrivateChatInfo chatInfo = createPrivateChat(chatInfoTag, _cid.ToString());
            _m_accountSaver?.removeChatRemoveTimeTag(chatInfo.chatInfoTag);
            return chatInfo;
        }

        public void addChatInfo(NPPrivateChatInfo _info)
        {
            _m_chatData?.addChatInfo(_info);
        }
        //发送私聊消息
        public void sendPrivateChatMsg(long _cid, _AMsgDetailInfo _msgInfo, Action _doneAction = null)
        {
            NPPrivateChatInfo chatInfo = getPrivateChatInfo(_cid);
            if(chatInfo == null)
            {
                chatInfo = createPrivateChat(_cid);
                addChatInfo(chatInfo);
                //_m_redTipDealer?.onAddPrivateChatInfo(chatInfo);
            }
            chatInfo.regLoadDoneDelegate(sendMsg);
            _m_accountSaver?.removeChatRemoveTimeTag(chatInfo.chatInfoTag);

            void sendMsg()
            {
                chatInfo.sendMsg(_msgInfo);
                _doneAction?.Invoke();
            }
        }

        //发送频道消息
        public void sendChatMsg(_AMsgDetailInfo _msgInfo, ENPChatRoomType _roomType = ENPChatRoomType.US_SERVER, Action _doneAction = null)
        {
            NPRoomChatInfo chatInfo = getRoomChatInfo(_roomType);
            if (chatInfo == null)
            {
                _doneAction?.Invoke();
                return;
            }

            chatInfo.sendMsg(_msgInfo);
            _doneAction?.Invoke();
        }

        private void _sendMsgLog(_IALProtocolStructure _msg)
        {
            if (_msg == null ||
                (_msg.getMainOrder() == 1 && _msg.getSubOrder() == 1))
                return;

            if (Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if (_msg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetSend($"[ChatPackage]<color=green>S -> C: {_msg.GetType().Name} ; </color> {GCommon.GetInfoPropertys(_msg)}");
                }
                else
                {
                    GCommon.NetWaring($"协议{_msg.GetType().Name}过大，大小：{_msg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }
        }

        private void _receiveMsgLog(_IALProtocolStructure _msg)
        {
            if (_msg == null ||
                (_msg.getMainOrder() == 1 && _msg.getSubOrder() == 1))
                return;

            if (Game.instance.mainCamera.gameSetting.printProtocol)
            {
                if (_msg.GetFullPackBufSize() <= Game.instance.mainCamera.gameSetting.protocolPrintMinSize)
                {
                    GCommon.NetRecv($"[ChatPackage]<color=red>S -> C: {_msg.GetType().Name} ; </color> {GCommon.GetInfoPropertys(_msg)}");
                }
                else
                {
                    GCommon.NetWaring($"协议{_msg.GetType().Name}过大，大小：{_msg.GetFullPackBufSize() / 1024}kb。不打印，注意检查");
                }
            }
        }

        private void _onChatNetError(int _errorCode)
        {
            if (_errorCode != 0)
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
        }

        #region S2C
        /// <summary>
        /// 初始化
        /// </summary>
        private void _init()
        {
            // 先清空，如果后续数据异常也认为是空列表，而不是上一次的数据
            _m_chatRoomList.Clear();
            joinChatRoom(ENPChatRoomType.US_SERVER, 0);//默认加入聊天室
            // 连接到聊天服务器
            _connectToChatServer();
        }

        /// <summary>
        /// 加入聊天频道
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_extId">额外参数，活动组队聊天-活动实例ID，其他-0</param>
        /// <param name="_isFailReTry">失败是否重试</param>
        /// <param name="_retryCount">重试次数</param>
        /// <param name="_retryTime">重试间隔</param>
        private void _joinChatRoom(ENPChatRoomType _type, long _extId, int _retryCount = int.MaxValue, float _retryTime = 3f, bool _isFailReTry = true, bool _eachFailShowErrTip = true)
        {
            // 判断若不在需要加入的聊天室列表中, 则不处理; 若已经连接上聊天服务器, 直接加入指定聊天室, 若没有, 在连上聊天服务器后, 会通过_m_lNeedJoinRoomList来加入
            if(!_m_lNeedJoinRoomList.Contains(new RoomJoinInfo(_type, _extId)) || _m_chatData == null || !_m_chatData.isConnected)
                return;
            
            long opSerialize = _m_initSerialize;
            NPGSClientListener.sendRequestByLog(GSWriter_022_ChatOp.make_002_ReqPlayerJoinChatRoom((int)_type, _extId), new CommonRequestCallbackProtocolDealer<GS2GC_022_002_RetPlayerJoinChatRoom>((_info) =>
            {
                //这里成功不处理，数据会有推送
            }, (_errCode) =>
            {
                // 若每次失败都需要显示错误tip, 则显示错误tip
                if(_eachFailShowErrTip)
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
    
                if (opSerialize != _m_initSerialize)
                    return;
                //重试加入房间
                if (_isFailReTry && _retryCount > 0)
                {
                    ALCommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if (opSerialize != _m_initSerialize)
                            return;
                        _joinChatRoom(_type, _extId, _retryCount - 1, _retryTime, _isFailReTry);
                    },_retryTime);
                }
                else if (!_eachFailShowErrTip)//若到这个判断则说明, 失败后不需要重试 或者 重试次数已经用完, 这时再判断_eachFailShowErrTip, 若为true, 则在上面的判断中就会弹出tip, 这里不需要再弹出; 所以这里只有在判断_eachFailShowErrTip为false的情况下才会弹出错误码
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                }
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_retryCount"></param>
        /// <param name="_retryTime"></param>
        /// <param name="_isFailReTry"></param>
        /// <param name="_eachFailShowErrTip">是否每次失败都需要显示错误tip, 若为false, 只会在最后一次弹出错误码code</param>
        public void joinChatRoom(ENPChatRoomType _type, long _extId, int _retryCount = int.MaxValue, float _retryTime = 3f, bool _isFailReTry = true, bool _eachFailShowErrTip = true)
        {
            RoomJoinInfo roomJoinInfo = new RoomJoinInfo(_type, _extId);
            
            if(!_m_lNeedJoinRoomList.Contains(roomJoinInfo))
                _m_lNeedJoinRoomList.Add(roomJoinInfo);

            _joinChatRoom(_type, _extId, _retryCount, _retryTime, _isFailReTry, _eachFailShowErrTip);
        }
        
        /// <summary>
        /// 当进入了聊天室
        /// </summary>
        public void onChatRoomJoin(GS2GC_022_050_OnChatRoomJoin _msg)
        {
            NPRoomChatInfo roomInfo = getRoomChatInfo(_msg.getRoomId());
            if (roomInfo != null)
                return;
            roomInfo = new NPRoomChatInfo(_msg.getRoomType(), _msg.getRoomId(), _msg.getExtId());
            if (null != _m_accountSaver)
            {
                if (_m_accountSaver.historyRoomChatReadTimeDic.ContainsKey(roomInfo.roomId))
                {
                    roomInfo.setLaseReadTime(_m_accountSaver.historyRoomChatReadTimeDic[roomInfo.roomId]);
                }
            }
            _m_chatRoomList.Add(roomInfo);
            _m_chatData?.addChatInfo(roomInfo);
            _m_redTipDealer?.onAddRoomChatInfo(roomInfo);
            onChatRoomAdd?.Invoke(roomInfo);
        }

        /// <summary>
        /// 客户端主动请求退出聊天室, 若是由于网络问题断开连接, 客户端不应该调用
        /// </summary>
        /// <param name="_type"></param>
        public void quitChatRoom(ENPChatRoomType _type, long _extId)
        {
            // 从_m_lNeedJoinRoomList列表中移除
            _m_lNeedJoinRoomList.Remove(new RoomJoinInfo(_type, _extId));
            
            // 从_m_chatRoomList列表中移除
            NPRoomChatInfo roomInfo = null;
            for (int i = 0; i < _m_chatRoomList.Count; i++)
            {
                roomInfo = _m_chatRoomList[i];
                if (roomInfo == null)
                    continue;
            
                if (roomInfo.type == _type)
                {
                    _m_chatData?.removeChatInfo(roomInfo.id);
                    _m_chatRoomList.RemoveAt(i);
                    _m_redTipDealer?.onRemoveRoomChatInfo(roomInfo);
                    onChatRoomRemove?.Invoke(roomInfo);
                    break;
                }
            }
            
            // 向服务器请求退出聊天室, 不管成功还是失败
            NPGSClientListener.sendRequestByLog(GSWriter_022_ChatOp.make_003_ReqPlayerQuitChatRoom((int)_type, _extId), new CommonRequestCallbackProtocolDealer<GS2GC_022_002_RetPlayerJoinChatRoom>((_info) =>
            {
            }, (_errCode) =>
            {
            }));
        }
        
        /// <summary>
        /// 当聊天室移除了
        /// </summary>
        public void onChatRoomDisconnect(GS2GC_022_051_OnChatRoomDisconnect _msg)
        {
            if (_msg == null)
                return;
            NPRoomChatInfo roomInfo = null;
            bool needRetry = false;
            for (int i = 0; i < _m_chatRoomList.Count; i++)
            {
                roomInfo = _m_chatRoomList[i];
                if (roomInfo == null)
                    continue;
            
                if (roomInfo.roomId == _msg.getRoomId())
                {
                    _m_chatData?.removeChatInfo(roomInfo.id);
                    _m_chatRoomList.RemoveAt(i);
                    onChatRoomRemove?.Invoke(roomInfo);
                    needRetry = true;
                    break;
                }
            }
            //断开了，重新加入
            if (needRetry)
            {
                _joinChatRoom(roomInfo.type, roomInfo.extId);   
            }
        }
        #endregion

        /// <summary>
        /// 获取到私聊登录信息后的处理
        /// </summary>
        private class ChatServerLoginDataCallback : _ANPRequestCallbackProtocolDealer<GS2GC_022_001_RetPlayerChatLogin>
        {
            // 组件的实例，用来调用方法
            [NotNull] private readonly NPPlayerChatComponent _m_comp;
            // 记录下来的操作数，用来废弃相关操作
            private long _m_opSerialize;
            public ChatServerLoginDataCallback([NotNull] NPPlayerChatComponent _comp)
            {
                _m_comp = _comp;
            }

            // 记录下现在的序列号
            public void recordSerialize()
            {
                _m_opSerialize = _m_comp._m_initSerialize;
            }

            protected override GS2GC_022_001_RetPlayerChatLogin _createSucProtocolObj()
            {
                return new GS2GC_022_001_RetPlayerChatLogin();
            }

            protected override void _dealSuc(GS2GC_022_001_RetPlayerChatLogin _sucMsg)
            {
                // 如果序列号不一样了，说明这次请求已经废弃了
                if (_m_opSerialize != _m_comp._m_initSerialize)
                    return;

                // 如果输入值为null，当作失败来处理
                if (_sucMsg == null)
                {
                    _dealFail(-1);
                    return;
                }

                // 用参数创建ChatData
                _m_comp._createChatData(_sucMsg);
            }

            protected override void _dealFail(int _errCode)
            {
                // 如果序列号不一样了，说明这次请求已经废弃了
                if (_m_opSerialize != _m_comp._m_initSerialize)
                    return;

                // 如果失败就尝试重连
                float tryDelay = GRefdataCoreMgr.instance.npGeneral.chat_server_failed_reconnect_delay;
                if (tryDelay <= 0)
                    return;

                ALCommonActionMonoTask.addMonoTask(_m_comp._tryReconnect, tryDelay);
            }
        }
        // /// <summary>
        // /// 聊天的玩家信息
        // /// </summary>
        // private class ChatPlayerInfo
        // {
        //     private readonly long _m_msgId;
        //     [NotNull] private readonly NPCommon_ChatPlayerContent _m_info;
        //     public ChatPlayerInfo(long _msgId, [NotNull] NPCommon_ChatPlayerContent m)
        //     {
        //         _m_msgId = _msgId;
        //         _m_info = m;
        //     }
        //     
        //     public long msgId { get { return _m_msgId; } }
        //     [NotNull] public NPCommon_ChatPlayerContent info { get { return _m_info; } }
        //
        //     /// <summary>
        //     /// 更新玩家信息
        //     /// </summary>
        //     /// <returns>是否有变动</returns>
        //     public bool updatePlayerInfo(long _msgId, NPCommon_ChatPlayerContent _player)
        //     {
        //         if (_msgId < _m_msgId || _player == null || _player.getCid() != _m_info.getCid())
        //             return false;
        //
        //         bool isChg = false;
        //         if (_player.getCName() != _m_info.getCName())
        //         {
        //             isChg = true;
        //             _m_info.setCName(_player.getCName());
        //         }
        //
        //         if (_player.getBubbleId() != _m_info.getBubbleId())
        //         {
        //             isChg = true;
        //             _m_info.setBubbleId(_player.getBubbleId());
        //         }
        //
        //         if (_player.getIconId() != _m_info.getIconId())
        //         {
        //             isChg = true;
        //             _m_info.setIconId(_player.getIconId());
        //         }
        //
        //         if (_player.getIconBgkId() != _m_info.getIconBgkId())
        //         {
        //             isChg = true;
        //             _m_info.setIconBgkId(_player.getIconBgkId());
        //         }
        //         
        //         return isChg;
        //     } 
        // }
        
        /// <summary>
        /// 判断是否置顶
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool getIsUpToTop(_INPChatInfo _info)
        {
            if (null == _info)
                return false;
            NPPrivateChatInfo chatInfo = _info as NPPrivateChatInfo;
            if (null == chatInfo)
                return false;
            return chatInfo.isUpToTop;
        }

        /// <summary>
        /// 设置是否置顶
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isUp"></param>
        public void setUpToTop(_INPChatInfo _info, bool _isUp)
        {
            if (null == _info)
                return;
            NPPrivateChatInfo chatInfo = _info as NPPrivateChatInfo;
            if (null == chatInfo)
                return;
            bool returnTrue = false;
            if (_isUp)
            {
                //判断数量限制
                returnTrue = _m_accountSaver.addUpToTop(chatInfo.chatInfoTag);
            }
            else
            {
                _m_accountSaver.removeUpToTop(chatInfo.chatInfoTag);
            }

            chatInfo.setIsUpToTop(returnTrue);
            
            WinMsg.SendMsg(WinMsgType.ON_CHAT_UP_TO_TOP_CHG);
        }
        
        /// <summary>
        /// 设置是否置顶
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isUp"></param>
        public void setUpToTop(List<_INPChatInfo> _infoList, bool _isUp)
        {
            if (null == _infoList)
                return;
            foreach (_INPChatInfo info in _infoList)
            {
                if(null == info)
                    continue;
                NPPrivateChatInfo chatInfo = info as NPPrivateChatInfo;
                if (null == chatInfo)
                    continue;
                
                bool returnTrue = false;
                if (_isUp)
                {
                    //判断数量限制
                    returnTrue = _m_accountSaver.addUpToTop(chatInfo.chatInfoTag);
                }
                else
                {
                    _m_accountSaver?.removeUpToTop(chatInfo.chatInfoTag);
                }
                chatInfo.setIsUpToTop(returnTrue);

            }
            WinMsg.SendMsg(WinMsgType.ON_CHAT_UP_TO_TOP_CHG);
        }

        /// <summary>
        /// 移除私聊频道列表，不删除记录
        /// </summary>
        /// <param name="_removeList"></param>
        public void removePrivateChannelList(List<_INPChatInfo> _infoList)
        {
            if (null == _infoList)
                return;
            foreach (_INPChatInfo info in _infoList)
            {
                if(null == info)
                    continue;
                NPPrivateChatInfo chatInfo = info as NPPrivateChatInfo;
                if (null == chatInfo)
                    continue;
                //删除同时设置为已读
                setCurChatReaded(info);
                _m_accountSaver?.addChatRemoveTimeTag(chatInfo.chatInfoTag, FpsAndPingMgr.instance.serverTimeTag);
            }
            
            onPrivateChatRemove?.Invoke();
        }
        
        /// <summary>
        /// 移除私聊频道，不删除消息记录
        /// </summary>
        /// <param name="_info"></param>
        public void removePrivateChannel(_INPChatInfo _info)
        {
            if (null == _info)
                return;
            NPPrivateChatInfo chatInfo = _info as NPPrivateChatInfo;
            if (null == chatInfo)
                return;
            //删除同时设置为已读
            setCurChatReaded(_info);
            _m_accountSaver?.addChatRemoveTimeTag(chatInfo.chatInfoTag, FpsAndPingMgr.instance.serverTimeTag);
            onPrivateChatRemove?.Invoke();
        }
    }
}
