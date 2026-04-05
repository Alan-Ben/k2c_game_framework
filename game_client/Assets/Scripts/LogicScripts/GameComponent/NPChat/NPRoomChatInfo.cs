using System;
using ChatPackage;
using ChatPackage.Internal;
using Common.NpChatObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class NPRoomChatInfo : _ARoomChatInfo, _INPChatInfo
    {
        private NPChatRoomRefObj _m_baseRefObj;
        private long _m_lastReadTimeTag = 0;
        private ENPChatRoomType _m_roomType;
        private long _m_extId;//扩展id，比如组队频道的活动实例id

        public NPRoomChatInfo(int _roomType, long _roomId, long _extId) : base(_roomId)
        {
            _m_roomType = (ENPChatRoomType)_roomType;
            _m_extId = _extId;
            _m_lastReadTimeTag = 0;
            _m_baseRefObj = GRefdataCoreMgr.instance.chatRoomRefCore.getRef((long)_m_roomType);

#if UNITY_EDITOR
            if (null == _m_baseRefObj)
            {
                Debug.LogError($"取不到对应聊天房间配置：{_m_roomType.ToString()}");
            }
#endif
        }

        /// <summary>
        /// 聊天室类型
        /// </summary>
        public ENPChatRoomType type { get { return _m_roomType; } }
        /// <summary>
        /// 附加id
        /// </summary>
        public long extId { get { return _m_extId; } }
        /// <summary>
        /// 聊天室配置
        /// </summary>
        public NPChatRoomRefObj baseRefObj { get { return _m_baseRefObj; } }
        
        /// <summary>
        /// 未读数量
        /// </summary>
        public bool isUnread { get => _getIsUnread(); }
        public long lastReadTimeTag { get => _m_lastReadTimeTag; }

        public bool needShow { get => (_m_baseRefObj != null) && _m_baseRefObj.unlock_condition.IsEnable(null); }
        
        protected override bool _checkCanSendMsg(bool _needPopTip)
        {
            // 判断是否在cd中
            long timeToLastSend = FpsAndPingMgr.instance.serverTimeTag - lastSendTime;
            long cdMilliseconds = getSendCD() - timeToLastSend;
            if (cdMilliseconds > 0)
            {
                if(_needPopTip)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_sendInCd_seconds, cdMilliseconds / 1000));
                return false;
            }
            if(null != baseRefObj && !baseRefObj.input_unlock_condition.IsEnable(null))
            {
                if(_needPopTip)
                    //条件不够不能发送
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(baseRefObj.input_unlock_condition_desc,baseRefObj.input_unlock_condition_desc_args));
                return false;
            }

            // 判断是否被禁言
            if (NPPlayer.instance.chatComp.isInForbidChat(_m_roomType, _needPopTip))
                return false;

            return true;
        }

        protected override void _sendMsg(_AMsgDetailInfo _msgInfo)
        {
            NPGSClientListener.sendMsgByLog(GSWriter_022_ChatOp.make_006_ReqPlayerSendRoomMsgV2(_m_roomType, _m_extId, _msgInfo));
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
            if(null == _m_baseRefObj)
                return String.Empty;

            return TextTranslate.instance.getLanguage(_m_baseRefObj.name);
        }

        public NPGTextureIndex getChatIcon()
        {
            return _m_baseRefObj?.icon;
        }

        public string getChatContent()
        {
            MsgInfo info = _m_listMsgInfo.GetLast();
            if (null != info)
            {
                _INPChatMiniShowInfo showInfo = info.detailInfo as _INPChatMiniShowInfo;
                return showInfo.getMiniContent();
            }
            return string.Empty;
        }

        //消息时间戳
        public long getChatTimeMS()
        {
            MsgInfo info = _m_listMsgInfo.GetLast();
            if (null != info)
            {
                return info.timeMs;
            }

            return 0;
        }

        public long getSendCD() { return _m_baseRefObj == null ? 0 : _m_baseRefObj.send_cd; }

        public int getSortId()
        {
            return 2;
        }
        
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
    }
}