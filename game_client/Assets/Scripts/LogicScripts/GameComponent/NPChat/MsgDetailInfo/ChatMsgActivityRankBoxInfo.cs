using ALPackage;
using Common.NpChatObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天分享冲榜宝箱消息
    /// </summary>
    public class ChatMsgActivityRankBoxInfo : _ANPPlayerChatMsgDetailInfo<NPCommon_ChatContent_ActivityRankBox>, _INPChatMiniShowInfo, _IChatMsgItemCommonBoxShowInfo
    {
        private bool _m_isInited;//是否初始化过
        private NPSOCommonBoxRefObj _m_boxRefObj;//宝箱配置数据
        private ActivityRankRushRefObj _m_activityRankRushRefObj;//活动冲榜配置数据
        private NPRankRefObj _m_rankRefObj;//排行榜配置数据
        private string _m_sGuildOrPlayerName;//公会或玩家名称

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
        /// 活动冲榜配置数据
        /// </summary>
        public ActivityRankRushRefObj activityRankRushRefObj
        {
            get
            {
                _initData();
                return _m_activityRankRushRefObj;
            }
        }
        /// <summary>
        /// 排行榜配置数据
        /// </summary>
        public NPRankRefObj rankRefObj
        {
            get
            {
                _initData();
                return _m_rankRefObj;
            }
        }
        /// <summary>
        /// 宝箱实例ID
        /// </summary>
        public long boxInstanceId { get { return content != null ? content.getInstanceId() : 0; } }
        /// <summary>
        /// 记录公会或玩家名称
        /// </summary>
        public string guildOrPlayerName { get => _m_sGuildOrPlayerName; set => _m_sGuildOrPlayerName = value; }

        public ChatMsgActivityRankBoxInfo() : base((int)ENPChatMsgType.ACTIVITY_RANK_BOX)
        {
            _m_isInited = false;
        }

        public string getMiniSender()
        {
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

            return TextTranslate.instance.getLanguage(boxRefObj.mini_chat_desc, boxRefObj.mini_chat_desc_args);
        }

        /// <summary>
        /// 初始化获取配置信息
        /// </summary>
        private void _initData()
        {
            if (_m_isInited || content == null)
                return;
            
            _m_boxRefObj = GRefdataCoreMgr.instance.commonBoxMap.getRef((long)content.getRefId());
            _m_activityRankRushRefObj = GRefdataCoreMgr.instance.activityRankRushRefCore.getRef((long)content.getActivityRankRushId());
            if (_m_activityRankRushRefObj != null)
                _m_rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_activityRankRushRefObj.rank_id);
            _m_isInited = true;
        }
    }      
}
