using System;
using Common.ChildEnum;
using Common.ChildObj;
using GS2GC.p014_ChildOp;
using JetBrains.Annotations;

namespace GOE
{
    public class UnmarriedInfo
    {
        private readonly long _m_adultId;
        [NotNull] private readonly AdultInfo _m_adultInfo;
        
        private int _m_expiredTs;
        private EAdultStatus _m_status;
        private long _m_minAllowBonus;
        
        
        public UnmarriedInfo(Adult_UnmarriedInfo _serverInfo)
        {
            _m_adultId = _serverInfo.getAdult().getId();
            _m_adultInfo = new AdultInfo(_serverInfo.getAdult(), NPPlayer.instance.playerInfo.CID);
            _m_status = _serverInfo.getStatus();
            _m_expiredTs = _serverInfo.getExpiredTs();
            _m_minAllowBonus = _serverInfo.getMinAllowBonus();
        }
        

        public long adultId { get { return _m_adultId; } } 
        [NotNull] public AdultInfo adultInfo { get { return _m_adultInfo; } }
        public EAdultStatus status { get { return _m_status; } }
        public int expiredTs { get { return _m_expiredTs; } }
        public long minAllowBonus { get { return _m_minAllowBonus; } }


        public void chgStatus(EAdultStatus _status, int _expiredTs, long _minAllowBonus)
        {
            _m_status = _status;
            _m_expiredTs = _expiredTs;
            _m_minAllowBonus = _minAllowBonus;

            if (FpsAndPingMgr.instance.serverTimeTagS >= _m_expiredTs)
            {
                _m_status = EAdultStatus.NONE;
                _m_minAllowBonus = 0;
            }
        }
    }
}