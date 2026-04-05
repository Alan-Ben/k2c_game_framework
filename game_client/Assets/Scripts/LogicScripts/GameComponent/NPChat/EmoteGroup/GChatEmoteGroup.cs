using Common.NpPlayerInfoObj;

namespace GOE
{
    public class GChatEmoteGroup
    {
        /// <summary>
        /// 表情包唯一id
        /// </summary>
        private long _m_id;
        /// <summary>
        /// 超时时间标记，单位秒。-1表示永久
        /// </summary>
        private int _m_expireTimeTagS;
        /// <summary>
        /// 是否查看过
        /// </summary>
        private bool _m_viewed;
        /// <summary>
        /// 表情包配置
        /// </summary>
        private GChatEmoteGroupRefObj _m_groupRef;
        
        
        public GChatEmoteGroup(PlayerInfo_ChatEmoteGroup _serverInfo)
        {
            updateInfo(_serverInfo);
        }

        public long id { get => _m_id; }
        public GChatEmoteGroupRefObj groupRef { get => _m_groupRef; }
        public bool isExpire { get => _m_expireTimeTagS > 0 && FpsAndPingMgr.instance.serverTimeTagS > _m_expireTimeTagS; }

        public void updateInfo(PlayerInfo_ChatEmoteGroup _info)
        {
            _m_id = _info.getId();
            _m_expireTimeTagS = _info.getExpireTimeTagS();
            _m_viewed = _info.getViewed();
            _m_groupRef = GRefdataCoreMgr.instance.chatEmoteGroupRefCore.getRef(_m_id);
        }
    }
}