
using ALPackage;
using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志
    /// </summary>
    public class ChatGuildLogMsgInfo : _AMsgDetailInfo<Common_ChatContent_GuildLog, NPCommon_ChatSystemPlayerContent>, _INPChatMiniShowInfo
    {
        private NPChatNPCRefObj _m_chatNpcRef;//聊天Npc配置
        private _IGuildLogMsgDetailInfo _m_iLogDetailInfo;//log的详细信息
        private bool _m_isInited;//是否初始化过
        
        public ChatGuildLogMsgInfo() : base((int) ENPChatMsgType.GUILD_LOG)
        {
            _m_isInited = false;
        }
        
        /// <summary>
        /// 是否是我自己的消息
        /// </summary>
        public override bool isMyMsg { get { return false; } }
        
        /// <summary>
        /// 聊天Npc配置 
        /// </summary>
        public NPChatNPCRefObj chatNpcRef { get { _initData(); return _m_chatNpcRef; } }
        
        public _IGuildLogMsgDetailInfo guildLogDetailInfo { get { _initData(); return _m_iLogDetailInfo; } }
        
        /// <inheritdoc/>
        public string getMiniSender()
        {
            if (null != chatNpcRef)
            {
                return TextTranslate.instance.getLanguage(chatNpcRef.name);
            }
            // return sender.getCName();
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_Npc_name_System);
        }

        /// <inheritdoc/>
        public string getMiniContent()
        {
            if (guildLogDetailInfo != null)
            {
                return guildLogDetailInfo.content;
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
            
            _m_chatNpcRef = GRefdataCoreMgr.instance.chatNPCRefCore.getRef(sender.getSystemPlayerId());
            if (null == _m_chatNpcRef)
            {
                ALLog.Error($"can no find chatNPCRef by id {sender.getSystemPlayerId()}");
            }

            _m_iLogDetailInfo = GuildLogMsgDetailFactory.getGuildLogMsgDetailInfo(content.getLogType(), content.getData());
            
            _m_isInited = true;
        }
    }
}