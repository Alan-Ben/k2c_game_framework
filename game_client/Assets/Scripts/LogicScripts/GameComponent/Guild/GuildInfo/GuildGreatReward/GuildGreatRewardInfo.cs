using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟大礼信息
    /// </summary>
    public class GuildGreatRewardInfo
    {
        /// <summary>
        /// 数据id
        /// </summary>
        private long _m_lDbId;
        
        /// <summary>
        /// 发放时间
        /// </summary>
        private long _m_lSendTimeMs;

        /// <summary>
        /// 过期时间
        /// </summary>
        private long _m_lExpiredTimeMs;

        public long dbId { get { return _m_lDbId; } }
        public long sendTimeMs { get { return _m_lSendTimeMs; } }
        public long expiredTimeMs { get { return _m_lExpiredTimeMs; } }
        
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isValid { get { return _m_lExpiredTimeMs > FpsAndPingMgr.instance.serverTimeTag; } }

        public GuildGreatRewardInfo(Guild_GreatRewardInfo _info)
        {
            updateInfo(_info);
        }
        
        public void updateInfo(Guild_GreatRewardInfo _info)
        {
            if(_info == null)
                return;
            
            _m_lDbId = _info.getDbId();
            _m_lSendTimeMs = _info.getSendTimeMs();
            _m_lExpiredTimeMs = _m_lSendTimeMs + GRefdataCoreMgr.instance.npGeneral.guild_great_reward_valid_time_sec * 1000;
        }
    }
}