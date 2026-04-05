using System.Collections.Generic;
using ALPackage;
using Common.NpChatObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天分享宝箱消息
    /// </summary>
    public class NPChatMsgCommonBoxInfo : _ANPPlayerChatMsgDetailInfo<NPCommon_ChatContent_CommBox>, _INPChatMiniShowInfo, _IChatMsgItemCommonBoxShowInfo
    {
        private bool _m_isInited;//是否初始化过
        private NPSOCommonBoxRefObj _m_boxRefObj;//宝箱配置数据

        /// <summary>
        /// 宝箱配置数据
        /// </summary>
        public NPSOCommonBoxRefObj boxRefObj
        {
            get
            {
                _initData();
                return _m_boxRefObj;
            }
        }
        /// <summary>
        /// 宝箱实例ID
        /// </summary>
        public long boxInstanceId { get { return content != null ? content.getInstanceId() : 0; } }

        public NPChatMsgCommonBoxInfo() : base((int)ENPChatMsgType.COMM_BOX)
        {
            _m_isInited = false;
        }

        public string getMiniSender()
        {
            if (null != boxRefObj && !string.IsNullOrEmpty(boxRefObj.sender_name))
                return TextTranslate.instance.getLanguage(boxRefObj.sender_name);
            
            if (!string.IsNullOrEmpty(sender.getCName()))
                return sender.getCName();
            
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_Npc_name_System);
        }

        public string getMiniContent()
        {
            if (content == null)
                return null;

            if (boxRefObj == null)
            {
                ALLog.Error($"未找到对应的宝箱数据,refId:{content.getRefId()}");
                return null;
            }

            if (boxRefObj.content_has_sender_name)
            {
                List<object> args = new List<object> { sender.getCName() };
                if (boxRefObj.mini_chat_desc_args != null)
                    args.AddRange(boxRefObj.mini_chat_desc_args);
                return TextTranslate.instance.getLanguage(boxRefObj.mini_chat_desc, args.ToArray());
            }
            return TextTranslate.instance.getLanguage(boxRefObj.mini_chat_desc, boxRefObj.mini_chat_desc_args);
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
            
            _m_boxRefObj = GRefdataCoreMgr.instance.commonBoxMap.getRef((long)content.getRefId());

            _m_isInited = true;
        }
    }      
}
