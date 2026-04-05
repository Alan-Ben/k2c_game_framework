using ChatPackage;
using ChatPackage.Internal;
using NPEnum;
using System;
using System.Collections.Generic;
using GS2GC.p022_ChatOp;

namespace GOE
{
    public class NPPrivateChatInfo : _APrivateChatInfo, _INPChatInfo
    {
        private NPCommonSimplePlayerInfo _m_playerInfo;
        private readonly long _m_targetPlayerId;
        private long _m_lastReadTimeTag = 0;
        public Action<NPPrivateChatInfo> onPrivateChatChg;
        private bool _m_isUpToTop = false;//是否置顶

        public NPPrivateChatInfo(string _chatInfoTag, string _otherUserTag) : base(_chatInfoTag,_otherUserTag)
        {
            _m_lastReadTimeTag = 0;
            _m_isUpToTop = false;
            long.TryParse(_otherUserTag, out _m_targetPlayerId);
        }

        public NPCommonSimplePlayerInfo userInfo { get { return _m_playerInfo; } }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool isUpToTop { get => _m_isUpToTop; }
        /// <summary>
        /// 未读数量
        /// </summary>
        public bool isUnread { get => _getIsUnread(); }
        public long lastReadTimeTag { get => _m_lastReadTimeTag; }

        private bool _getIsUnread()
        {
            if(_m_listMsgInfo.Count ==0)
                return false;
            int l = _m_listMsgInfo.Count;
            MsgInfo msgInfo = null;
            for (int i = l - 1; i > -1; i--)
            {
                //过滤自己发出的信息
                msgInfo = _m_listMsgInfo[i];
                if(null == msgInfo)
                    continue;
                if(msgInfo.detailInfo.isMyMsg)
                    continue;
                return _m_lastReadTimeTag < msgInfo.timeMs;
            }
            
            return false;
        }
        
        //获取玩家信息
        protected override void _initByUnreadMsg(Action<List<MsgInfo>> _action)
        {
            regPlayerInfo(() =>
            {
                base._initByUnreadMsg(_action);
            });

            //获取一下最新的历史消息
            getHistoryList(1, (_infoList) =>
            {
            });
        }

        protected override void _onReceiveMsg(MsgInfo _msgInfo)
        {
            base._onReceiveMsg(_msgInfo);
            onPrivateChatChg?.Invoke(this);
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_playerInfo = null;
        }
        
        protected override bool _checkCanSendMsg(bool _needPopTip)
        {
            bool canSend = GRefdataCoreMgr.instance.npGeneral.chat_unlock_condition.IsEnable(null);
            
            //条件不够不能发送
            if (!canSend)
            {
                if(_needPopTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_lock_str));
                return false;
            }

            // 判断是否在cd中
            long timeToLastSend = FpsAndPingMgr.instance.serverTimeTag - lastSendTime;
            long cdMilliseconds = getSendCD() - timeToLastSend;
            if (cdMilliseconds > 0)
            {
                if(_needPopTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_sendInCd_seconds, cdMilliseconds / 1000));
                return false;
            }

            return true;
        }

        protected override void _sendMsg(_AMsgDetailInfo _msgInfo)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_022_ChatOp.make_005_ReqPlayerSendPrivateMsg(_m_targetPlayerId, _msgInfo)
            , new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_022_005_RetPlayerSendPrivateMsg>(_info =>
            {
                //私聊发送消息本地直接记录，服务端不推送
                recieveMsg(new MsgInfo(_info.getMsgId(), FpsAndPingMgr.instance.serverTimeTag, _msgInfo));
                WinMsg.SendMsg(WinMsgType.ON_CHAT_SENDED_PRIVATE_MSG_DONE);
            }));
            WinMsg.SendMsg(WinMsgType.ON_CHAT_SENDED);
        }


        /// <summary>
        /// 获取当前时间
        /// </summary>
        protected override long _getNowTime()
        {
            return FpsAndPingMgr.instance.serverTimeTag;
        }

        public string getChatName()
        {
            if (null != userInfo)
                return userInfo.name;

            return string.Empty;
        }

        public NPGTextureIndex getChatIcon()
        {
            if (null != userInfo)
                return GCommon.getItemTexIcon(ENPItemType.ICON, userInfo.icon);

            return null;
        }

        public string getChatContent()
        {
           MsgInfo info =  _m_listMsgInfo.GetLast();
            string contentStr = string.Empty;
            getHistoryList(1, (_infoList) =>
            {
                if (null != _infoList && _infoList.Count > 0)
                {
                    _INPChatMiniShowInfo showInfo = _infoList[0].detailInfo as _INPChatMiniShowInfo;
                    contentStr = showInfo.getMiniContent();
                }
            });         
            return contentStr;
        }

        /// <summary>
        /// 设置为已读
        /// </summary>
        public void setHasRead()
        {
            _m_lastReadTimeTag = _getNowTime();
        }

        /// <summary>
        /// 最后一次读取消息的时间
        /// </summary>
        /// <param name="_lastReadTime"></param>
        public void setLaseReadTime(long _lastReadTime)
        {
            _m_lastReadTimeTag = _lastReadTime;
        }

        /// <summary>
        /// 设置是否置顶
        /// </summary>
        /// <param name="_isUpToTop"></param>
        public void setIsUpToTop(bool _isUpToTop)
        {
            _m_isUpToTop = _isUpToTop;
        }
        
        //消息时间戳
        public long getChatTimeMS()
        {
            long timeMs = 0;
            getHistoryList(1, (_infoList) =>
            {
                if (null != _infoList && _infoList.Count > 0)
                {
                    timeMs = _infoList[0].timeMs;
                }
            });
            return timeMs;
        }

        //发送间隔 todo
        public long getSendCD()
        {
            return GRefdataCoreMgr.instance.npGeneral.chat_private_send_cd;
        }
        public int getSortId()
        {
            return 1;
        }

        /// <summary>
        /// 请求一次玩家信息
        /// </summary>
        /// <param name="_dealDone"></param>
        public void regPlayerInfo(Action _dealDone)
        {
            GCommon.reqPlayerInfoSer(_m_targetPlayerId,(info) =>
            {
                if (null == info)
                {
                    _dealDone?.Invoke();
                    return;
                }

                _m_playerInfo = new NPCommonSimplePlayerInfo(info.getSomeOneShowInfo());
                _dealDone?.Invoke();
            });
        }
    }
}