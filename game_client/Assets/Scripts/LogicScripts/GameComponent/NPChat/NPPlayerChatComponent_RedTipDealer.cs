using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class NPPlayerChatComponent
    {
        private class RedTipDealer
        {
            private NPPlayerChatComponent _m_chatComponent;
            
            //红点字典
            [NotNull] private Dictionary<string, _ARedTipNode> _m_dMyNode = new Dictionary<string, _ARedTipNode>(); //是否有未读消息的红点
            //保存频道消息回调，确保可正确移除
            [NotNull] private readonly Dictionary<NPRoomChatInfo, Action<MsgInfo>> _m_dRoomChatMsgHandlers = new Dictionary<NPRoomChatInfo, Action<MsgInfo>>();

            public RedTipDealer(NPPlayerChatComponent _component)
            {
                _m_chatComponent = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                foreach (NPRoomChatInfo roomChat in _m_chatComponent._m_chatRoomList)
                {
                    _addRedTipNode(roomChat);
                }
                //私聊动态红点
                foreach (NPPrivateChatInfo privateChat in _m_chatComponent._m_chatPrivateList)
                {
                    _addRedTipNode(privateChat);
                }
            }
            
            public void clear()
            {
                foreach (_ARedTipNode node in _m_dMyNode.Values)
                {
                    node.setCount(0);
                }
                _m_dMyNode.Clear();
                _m_dRoomChatMsgHandlers.Clear();
            }
            

            /// <summary>
            /// 新的私聊
            /// </summary>
            /// <param name="_privateChat"></param>
            public void onAddPrivateChatInfo(NPPrivateChatInfo _privateChat)
            {
                _addRedTipNode(_privateChat);
            }

            public void onRemovePrivateChatInfo(NPPrivateChatInfo _privateChat)
            {
                _removeRedTipNode(_privateChat);
            }
            /// <summary>
            /// 新的聊天频道
            /// </summary>
            /// <param name="_privateChat"></param>
            public void onAddRoomChatInfo(NPRoomChatInfo _roomChat)
            {
                _addRedTipNode(_roomChat);
            }

            public void onRemoveRoomChatInfo(NPRoomChatInfo _roomChat)
            {
                _removeRedTipNode(_roomChat);
            }
            
            private void _addRedTipNode(NPRoomChatInfo _roomChat)
            {
                if (_roomChat == null)
                    return;
                string creatKey = _createRedKey(RedTipConst.RED_CHAT_CHANLE, _roomChat.id);
                if (_m_dMyNode.ContainsKey(creatKey))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error~重复创建频道的未读红点{_roomChat.id}");
#endif
                    return;
                }
                CommonForceRedTipNode newScoreNode = new CommonForceRedTipNode(creatKey);
                RedTipMgr.instance.addRedTipNodeWithParent(newScoreNode,RedTipConst.RED_CHAT_CHANLE);

                //联盟聊天频道，设置引用红点
                if (_roomChat.type == ENPChatRoomType.GUILD)
                {
                    RedTipMgr.instance.addRedTipNodeWithParent(newScoreNode, RedTipConst.RED_GUILD_CHAT);
                }

                _m_dMyNode[creatKey] = newScoreNode;

                _roomChat.regLoadDoneDelegate(() =>
                {
                    if (_roomChat.isUnread)
                        newScoreNode.setCount(1);
                    else
                        newScoreNode.setCount(0);
                });
                _roomChat.getHistoryList(1, (_) =>
                {
                    if (_roomChat.isUnread)
                        newScoreNode.setCount(1);
                    else
                        newScoreNode.setCount(0);
                });
                Action<MsgInfo> handler = (_chat) =>
                {
                    _onRoomChatChg(_roomChat);
                };
                _m_dRoomChatMsgHandlers[_roomChat] = handler;
                _roomChat.onReceiveMsg += handler;
            }

            private void _removeRedTipNode(NPRoomChatInfo _roomChat)
            {
                if (_roomChat == null)
                    return;
                _ARedTipNode node = _getRedTipNode(_roomChat);
                string creatKey = _createRedKey(RedTipConst.RED_CHAT_CHANLE, _roomChat.id);
                if (null != node)
                {
                    node.setCount(0);
                    RedTipMgr.instance.rmvRedTipNode(node);
                    _m_dMyNode.Remove(creatKey);
                }
                if (_m_dRoomChatMsgHandlers.TryGetValue(_roomChat, out Action<MsgInfo> handler))
                {
                    _roomChat.onReceiveMsg -= handler;
                    _m_dRoomChatMsgHandlers.Remove(_roomChat);
                }
            }

            private void _onRoomChatChg(NPRoomChatInfo _chatInfo)
            {
                if (null == _chatInfo)
                    return;
                
                _ARedTipNode node = _getRedTipNode(_chatInfo);
                if (null != node)
                {
                    node.setCount(_chatInfo.isUnread ? 1 : 0);
                }
            }

            //新增红点节点
            private void _addRedTipNode(NPPrivateChatInfo _privateChat)
            {
                if (_privateChat == null)
                    return;
                string creatKey = _createRedKey(RedTipConst.RED_PRIVATE_CHAT, _privateChat.chatInfoTag);
                if (_m_dMyNode.ContainsKey(creatKey))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error~重复创建私聊的未读红点{_privateChat.chatInfoTag}");
#endif
                    return;
                }
                CommonForceRedTipNode newScoreNode = new CommonForceRedTipNode(creatKey);
                RedTipMgr.instance.addRedTipNodeWithParent(newScoreNode,RedTipConst.RED_PRIVATE_CHAT);
                _m_dMyNode[creatKey] = newScoreNode;

                _privateChat.regLoadDoneDelegate(() =>
                {
                    if (_privateChat.isUnread && !NPPlayer.instance.friendsComp.isShield(_privateChat.userInfo.cid))
                        newScoreNode.setCount(1);
                    else
                        newScoreNode.setCount(0);
                });
                _privateChat.onPrivateChatChg += _onPrivateChatChg;
                
                
                creatKey = _createRedKey(RedTipConst.RED_CHAT_CHANLE, _privateChat.chatInfoTag);
                _ARedTipNode node = null;
                if (!_m_dMyNode.TryGetValue(creatKey, out node))
                {
                    node = new CommonForceRedTipNode(creatKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(node,RedTipConst.RED_CHAT_CHANLE);
                    _m_dMyNode[creatKey] = node;
                }
                RedTipMgr.instance.addRedTipNodeWithParent(newScoreNode,node);
            }


            //移除红点节点
            private void _removeRedTipNode(NPPrivateChatInfo _privateChat)
            {
                if (_privateChat == null)
                    return;
                _ARedTipNode node = _getRedTipNode(_privateChat);
                string creatKey = _createRedKey(RedTipConst.RED_PRIVATE_CHAT, _privateChat.chatInfoTag);
                if (null != node)
                {
                    node.setCount(0);
                    RedTipMgr.instance.rmvRedTipNode(node);
                    _m_dMyNode.Remove(creatKey);
                }
                _privateChat.onPrivateChatChg -= _onPrivateChatChg;
                
            }

            private void _onPrivateChatChg(NPPrivateChatInfo _privateChat)
            {
                if (null == _privateChat)
                    return;
                _ARedTipNode node = _getRedTipNode(_privateChat);
                if (null != node)
                {
                    node.setCount(_privateChat.isUnread  && !NPPlayer.instance.friendsComp.isShield(_privateChat.userInfo.cid) ? 1 : 0);
                }
            }

            //获取对用红点
            private _ARedTipNode _getRedTipNode(NPPrivateChatInfo _privateChat)
            {
                
                string creatKey = _createRedKey(RedTipConst.RED_PRIVATE_CHAT, _privateChat.chatInfoTag);
                if (_m_dMyNode.TryGetValue(creatKey, out _ARedTipNode node))
                {
                    return node;
                }
                return null;
            }
            
            //获取对用红点
            private _ARedTipNode _getRedTipNode(NPRoomChatInfo _roomChat)
            {
                
                string creatKey = _createRedKey(RedTipConst.RED_CHAT_CHANLE, _roomChat.id);
                if (_m_dMyNode.TryGetValue(creatKey, out _ARedTipNode node))
                {
                    return node;
                }
                return null;
            }
                
            //创建唯一key
            private string _createRedKey(long _key, string _otherTag)
            {
                return $"{_key}_{_otherTag}";
            }
            
            /// <summary>
            /// 设置已读
            /// </summary>
            /// <param name="_chatInfo"></param>
            public void setCurChatReaded(_AChatInfo _chatInfo)
            {
                if (_chatInfo == null)
                    return;
                if (_chatInfo is NPPrivateChatInfo)
                {
                    //设置已读
                    NPPrivateChatInfo privateChatInfo = (NPPrivateChatInfo)_chatInfo;
                    privateChatInfo.setHasRead();
                    //保存已读时间戳
                    _m_chatComponent?._m_accountSaver?.addChatReadTimeTag(privateChatInfo.chatInfoTag,privateChatInfo.lastReadTimeTag);
                    
                    _ARedTipNode node = _getRedTipNode(privateChatInfo);
                    if (null == node)
                        return;
                    node.setCount(0);
                }
                
                if (_chatInfo is NPRoomChatInfo)
                {
                    //设置已读
                    NPRoomChatInfo roomChatInfo = (NPRoomChatInfo)_chatInfo;
                    roomChatInfo.setHasRead();
                    //保存已读时间戳
                    _m_chatComponent?._m_accountSaver?.addRoomChatReadTimeTag(roomChatInfo.roomId, roomChatInfo.lastReadTimeTag);
                    
                    _ARedTipNode node = _getRedTipNode(roomChatInfo);
                    if (null == node)
                        return;
                    node.setCount(0);
                }
            }

            /// <summary>
            /// 是否已读
            /// </summary>
            /// <param name="_chatInfo"></param>
            /// <returns></returns>
            public bool getCurChatHasRead(_AChatInfo _chatInfo)
            {
                if (_chatInfo is NPPrivateChatInfo)
                {
                    _ARedTipNode redTipNode = _getRedTipNode((NPPrivateChatInfo) _chatInfo);
                    if (null != redTipNode)
                    {
                        return redTipNode.getCount() <= 0;
                    }
                }
                if (_chatInfo is NPRoomChatInfo)
                {
                    //设置已读
                    NPRoomChatInfo roomChatInfo = (NPRoomChatInfo)_chatInfo;
                    
                    _ARedTipNode node = _getRedTipNode(roomChatInfo);
                    if (null != node)
                    {
                        return node.getCount() <= 0;
                    }
                }
                return true;
            }
        }

        public void setCurChatReaded(_INPChatInfo _chatInfo)
        {
            if (_chatInfo is _AChatInfo)
            {
                setCurChatReaded((_AChatInfo)_chatInfo);
            }
        }
        
        //设置当前聊天对话已读
        public void setCurChatReaded(_AChatInfo _chatInfo)
        {
            if(null == _chatInfo)
                return;
            _m_redTipDealer?.setCurChatReaded(_chatInfo);
        }
        
        //设置当前聊天对话
        public void setCurChatShow(_AChatInfo _chatInfo)
        {
            if(null == _chatInfo)
                return;
            _m_curChatInfo = _chatInfo;
        }
        
        //获取当前聊天对话是否已读
        public bool getCurChatHasRead(_INPChatInfo _chatInfo)
        {
            if (null == _chatInfo)
                return true;
            if (_chatInfo is _AChatInfo)
                return getCurChatHasRead((_AChatInfo)_chatInfo);
            return true;
        }
        
        //获取当前聊天对话是否已读
        public bool getCurChatHasRead(_AChatInfo _chatInfo)
        {
            if (null == _chatInfo)
                return true;
            if (null == _m_redTipDealer)
                return true;
            return _m_redTipDealer.getCurChatHasRead(_chatInfo);
        }
    }
}