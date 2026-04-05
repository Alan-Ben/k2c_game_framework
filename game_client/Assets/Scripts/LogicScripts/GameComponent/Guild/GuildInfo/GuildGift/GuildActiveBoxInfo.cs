using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟活跃宝箱信息
    /// </summary>
    public class GuildActiveBoxInfo
    {
        /// <summary>
        /// 数据id
        /// </summary>
        private long _m_lDbId;
        
        /// <summary>
        /// 发送者cid
        /// </summary>
        private long _m_lSenderCid;
        
        /// <summary>
        /// 发送者信息
        /// </summary>
        private GuildMemberInfo _m_iSenderInfo;
        
        /// <summary>
        /// 发放时间
        /// </summary>
        private long _m_lSendTimeMs;

        /// <summary>
        /// 过期时间
        /// </summary>
        private long _m_lExpiredTimeMs;

        /// <summary>
        /// 发送宝箱的印章数量
        /// </summary>
        private long _m_lStamp;

        public long dbId { get { return _m_lDbId; } }
        public long senderCid { get { return _m_lSenderCid; } }
        public GuildMemberInfo senderInfo { get { return _m_iSenderInfo; } }
        public long sendTimeMs { get { return _m_lSendTimeMs; } }
        public long expiredTimeMs { get { return _m_lExpiredTimeMs; } }
        public long stamp { get { return _m_lStamp; } }
        
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isValid { get { return _m_lExpiredTimeMs > FpsAndPingMgr.instance.serverTimeTag; } }

        public GuildActiveBoxInfo(Guild_ActiveBoxInfo _info)
        {
            updateInfo(_info);
        }
        
        public void updateInfo(Guild_ActiveBoxInfo _info)
        {
            if(_info == null)
                return;
            
            _m_lDbId = _info.getDbId();
            _m_lSenderCid = _info.getSenderCid();
            _m_iSenderInfo = NPPlayer.instance.guildComp.guildInfo?.getMemberInfo(_m_lSenderCid);
            _m_lSendTimeMs = _info.getSendTimeMs();
            _m_lExpiredTimeMs = _m_lSendTimeMs + GRefdataCoreMgr.instance.npGeneral.active_box_valid_time_sec * 1000;
            _m_lStamp = _info.getStamp();
        }
    }
}