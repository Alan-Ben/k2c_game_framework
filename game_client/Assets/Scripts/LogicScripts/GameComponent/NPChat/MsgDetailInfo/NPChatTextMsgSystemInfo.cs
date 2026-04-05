
using ALPackage;
using NPEnum;
using ChatPackage;
using Common.NpChatObj;

namespace GOE
{
    /// <summary>
    /// 聊天系统消息文字信息
    /// </summary>
    public class NPChatTextMsgSystemInfo : _AMsgDetailInfo<NPCommon_ChatContent_System, NPCommon_ChatSystemPlayerContent>, _INPChatMiniShowInfo
    {
        private NPChatNPCRefObj _m_chatNpcRef;//聊天Npc配置
        private bool _m_isInited;//是否初始化过
        
        public NPChatTextMsgSystemInfo() : base((int) ENPChatMsgType.SYSTEM)
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
        public NPChatNPCRefObj chatNpcRef { get { _initRef(); return _m_chatNpcRef; } }
        
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
            return TextTranslate.instance.getLanguage(content.getContent());
        }
        
        /// <summary>
        /// 初始化获取配置信息
        /// </summary>
        private void _initRef()
        {
            if (_m_isInited)
            {
                return;
            }
            
            _m_chatNpcRef = GRefdataCoreMgr.instance.chatNPCRefCore.getRef(sender.getSystemPlayerId());
            if (null == _m_chatNpcRef)
            {
                ALLog.Error($"can no find chatNPCRef by id {sender.getSystemPlayerId()}");
            }

            _m_isInited = true;
        }
    }
}