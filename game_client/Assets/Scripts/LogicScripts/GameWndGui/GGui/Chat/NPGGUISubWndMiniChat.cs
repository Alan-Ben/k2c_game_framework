
using System.Collections.Generic;
using System.Text;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 聊天迷你展示兼入口
    /// </summary>
    public class NPGGUISubWndMiniChat : _ANPGGUIBasicSubWnd<NPGGUIMonoMiniChat>
    {
        // 当前显示的消息和消息所属的聊天会话
        private _AChatInfo _m_curShowInfo;
        private MsgInfo _m_curShowMsg;
        
        // 聊天会话的icon
        private NPGGuiWndTexture _m_chatInfoIcon;
        
        // 聊天室处理器列表，用来找出当前最新的消息和消息所属的聊天会话
        [NotNull] private readonly List<ChatInfoReceiveMsgDealer> _m_receiveDealerList;
        private StringBuilder _m_msgContent; 
        
        public NPGGUISubWndMiniChat(NPGGUIMonoMiniChat _wnd) : base(_wnd)
        {
            _m_receiveDealerList = new List<ChatInfoReceiveMsgDealer>(10);
            initWnd();
        }

        protected override void _onShowWnd()
        {
            NPPlayer.instance.chatComp.onChatDataCreat += _onChatDataCreat;
            // 窗口显示时刷新内容
            _reInitChatDealer();
            
            _m_chatInfoIcon?.showWnd();
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.chatComp.onChatDataCreat -= _onChatDataCreat;
            _clearChatDealer();

            _m_chatInfoIcon?.hideWnd();

            _m_curShowInfo = null;
            _m_curShowMsg = null;
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnOpen, _onBtnOpenClick);
            }
            
            _m_chatInfoIcon?.discard();
            _m_chatInfoIcon = null;
            _m_msgContent.Clear();
            _m_msgContent = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnOpen, _onBtnOpenClick);

                if (wnd.rawImgChannelIcon != null)
                    _m_chatInfoIcon = new NPGGuiWndTexture(wnd.rawImgChannelIcon);
            }
            _m_msgContent = new StringBuilder();
        }

        // 点击打开按钮后，打开当前显示的消息所在的聊天会话
        private void _onBtnOpenClick(GameObject _)
        {
            if (wnd == null)
                return;

            GCommon.enterUIMainNodeShow(ESysSceneType.CHAT,new List<string>(){wnd.defaultOpenChannel.ToString()});
        }

        /// <summary>
        /// 聊天数据创建的时候
        /// </summary>
        private void _onChatDataCreat()
        {
            
            _clearChatDealer();
            _reInitChatDealer();
        }

        /// <summary>
        /// 清除数据dealer
        /// </summary>
        private void _clearChatDealer()
        {
            for (int i = 0; i < _m_receiveDealerList.Count; i++)
            {
                // 重置所有处理器
                _m_receiveDealerList[i].reset();
            }
        }

        /// <summary>
        /// 初始化聊天信息dealer
        /// </summary>
        private void _reInitChatDealer()
        {
            if (wnd != null)
            {
                // 监听设置好的对应聊天室
                List<ENPChatRoomType> showTypeList = wnd.showRoomTypes;
                for (int i = 0; i < showTypeList.Count; i++)
                {
                    ENPChatRoomType roomType = showTypeList[i];
                    // 初始化处理器
                    _getReceiveDealer(roomType).init();
                }
            }
        }

        // 当收到了新消息后的处理
        private void onReceiveMsg(_AChatInfo _chatInfo, MsgInfo _newMsg)
        {
            // 判断消息是否可用
            bool isEnable = _newMsg != null && _newMsg.detailInfo is _INPChatMiniShowInfo;

            // 当前有显示，且入参不是可用的消息，忽略
            // 或者入参消息比当前显示旧，也忽略
            if ((_m_curShowMsg != null && !isEnable)
                ||(isEnable && _m_curShowMsg != null && _newMsg.msgId < _m_curShowMsg.msgId))
                return;

            // 更新显示
            _m_curShowInfo = _chatInfo;
            _m_curShowMsg = _newMsg;
            if (wnd != null)
            {
                // 聊天室相关
                NPChatRoomRefObj refObj = (_m_curShowInfo as NPRoomChatInfo)?.baseRefObj;
                NPGTextureIndex texture = refObj?.icon;
                _m_chatInfoIcon?.setTexture(texture);
                ALUGUICommon.setLabelTxt(wnd.txtChannel, TextTranslate.instance.getLanguage(refObj?.name));

                // 迷你聊天文本框相关，聊天室默认有一条系统消息，没有消息就直接清空容错
                _m_msgContent.Clear();
                if (_m_curShowMsg != null)
                {
                    _INPChatMiniShowInfo detailInfo = (_INPChatMiniShowInfo)_m_curShowMsg.detailInfo;
                    _m_msgContent.Append(TextTranslate.instance.getLanguage(TransKeyConst.chat_miniShowSender_name, detailInfo.getMiniSender()));
                    _m_msgContent.Append(detailInfo.getMiniContent());
                }
                // todo:content用混动窗口做
                ALUGUICommon.setLabelTxt(wnd.txtContent, _m_msgContent.ToString());
            }
        }

        [NotNull]
        private ChatInfoReceiveMsgDealer _getReceiveDealer(ENPChatRoomType _roomType)
        {
            for (int i = 0; i < _m_receiveDealerList.Count; i++)
            {
                if (_m_receiveDealerList[i].roomType == _roomType)
                    return _m_receiveDealerList[i];
            }

            ChatInfoReceiveMsgDealer msgDealer = new ChatInfoReceiveMsgDealer(this, _roomType);
            _m_receiveDealerList.Add(msgDealer);
            return msgDealer;
        }

        private class ChatInfoReceiveMsgDealer
        {
            private ENPChatRoomType _m_roomType;
            private NPRoomChatInfo _m_roomChatInfo;
            [NotNull] private NPGGUISubWndMiniChat _m_instance;
            
            public ChatInfoReceiveMsgDealer([NotNull] NPGGUISubWndMiniChat _instance, ENPChatRoomType _type)
            {
                _m_instance = _instance;
                _m_roomType = _type;
            }

            public ENPChatRoomType roomType { get { return _m_roomType; } }

            public void init()
            {
                // 先尝试获取 roomInfo
                // todo: chatComp 里面其实是支持一个 type 有多个 roomInfo 的，这里看看要不要做这一层支持
                _m_roomChatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(_m_roomType);
                
                // 监听 roomInfo 变化事件
                NPPlayer.instance.chatComp.onChatRoomAdd += _onChatRoomAdd;
                NPPlayer.instance.chatComp.onChatRoomRemove += _onChatRoomRemove;
                
                if (_m_roomChatInfo == null)
                    return;

                _m_roomChatInfo.getHistoryList(1, _onInit);
                _m_roomChatInfo.onReceiveMsg += _onReceiveMsg;
            }

            public void reset()
            {
                // 移除 roomInfo 变化事件的监听
                NPPlayer.instance.chatComp.onChatRoomAdd -= _onChatRoomAdd;
                NPPlayer.instance.chatComp.onChatRoomRemove -= _onChatRoomRemove;
                
                if (_m_roomChatInfo == null)
                    return;

                _m_roomChatInfo.onReceiveMsg -= _onReceiveMsg;
                _m_roomChatInfo = null;
            }

            private void _onChatRoomAdd(NPRoomChatInfo _roomChatInfo)
            {
                // 如果本地已经有 roomInfo 了 或
                // 如果新增的数据异常 或
                // 新增的 roomInfo 的类型不是这个 dealer 的类型，就不处理
                if (_m_roomChatInfo != null || _roomChatInfo == null || _roomChatInfo.type != _m_roomType)
                    return;

                // 赋值，绑定事件后，进行初始化
                _m_roomChatInfo = _roomChatInfo;
                _m_roomChatInfo.getHistoryList(1, _onInit);
                _m_roomChatInfo.onReceiveMsg += _onReceiveMsg;
            }

            private void _onChatRoomRemove(NPRoomChatInfo _roomChatInfo)
            {
                // 如果被移除的 roomInfo 不是本地的 roomInfo ，就不处理
                if (_roomChatInfo != _m_roomChatInfo)
                    return;
                
                // 解除事件，并清空赋值
                _m_roomChatInfo.onReceiveMsg -= _onReceiveMsg;
                _m_roomChatInfo = null;
            }

            private void _onReceiveMsg(MsgInfo _msg)
            {
                _m_instance.onReceiveMsg(_m_roomChatInfo, _msg);
            }

            private void _onInit(List<MsgInfo> _historyList)
            {
                _onReceiveMsg(_historyList.GetLast());
            }
        }
    }
}