
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志
    /// </summary>
    public class ChatSystemLogMsgInfo : _ANPPlayerChatMsgDetailInfo<ChatObj_SystemLog>, _INPChatMiniShowInfo
    {
        private bool _m_isInited;//是否初始化过
        private ChatSystemLogRefObj _m_chatSystemLogRef;
        
        public ChatSystemLogMsgInfo() : base((int) ENPChatMsgType.SYSTEM_LOG)
        {
            _m_isInited = false;
        }
        
        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return false; } }

        public ChatSystemLogRefObj chatSystemLogRef
        {
            get
            {
                _initData();
                return _m_chatSystemLogRef;
            }
        }

        /// <inheritdoc/>
        public string getMiniSender()
        {
            if (null != chatSystemLogRef && !string.IsNullOrEmpty(chatSystemLogRef.sender_name))
                return TextTranslate.instance.getLanguage(chatSystemLogRef.sender_name);
            
            if (!string.IsNullOrEmpty(sender.getCName()))
                return sender.getCName();
            
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_Npc_name_System);
        }

        /// <inheritdoc/>
        public string getMiniContent()
        {
            if (null != chatSystemLogRef)
            {
                if (chatSystemLogRef.content_has_sender_name)
                {
                    List<object> args = new List<object> { sender.getCName() };
                    if (chatSystemLogRef.content_args != null)
                        args.AddRange(chatSystemLogRef.content_args);
                    return TextTranslate.instance.getLanguage(chatSystemLogRef.content, args.ToArray());
                }
                return TextTranslate.instance.getLanguage(chatSystemLogRef.content, chatSystemLogRef.content_args);
            }
            
            return "";
        }
        
        /// <summary>
        /// 初始化获取配置信息
        /// </summary>
        private void _initData()
        {
            if (_m_isInited || content == null)
            {
                return;
            }
            
            _m_chatSystemLogRef = GRefdataCoreMgr.instance.chatSystemLogRefCore.getRef((long)content.getLogType());

            _m_isInited = true;
        }
    }
}