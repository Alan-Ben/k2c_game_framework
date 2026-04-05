using Common.ChildObj;
using JetBrains.Annotations;

namespace GOE
{
    public class SeatInfo
    {
        [NotNull] private readonly ChildSeatRefObj _m_baseRef;
        [NotNull] private readonly LazyCd _m_lazyCd;
        private long _m_fullNextRemainMs;
        private ChildInfo _m_childInfo;


        public SeatInfo([NotNull] Child_SeatInfo _serverSeatInfo)
        {
            _m_baseRef = GRefdataCoreMgr.instance.childSeatCore.getRef(_serverSeatInfo.getSeatId());
            _m_lazyCd = new LazyCd(_serverSeatInfo.getEnergy(), _serverSeatInfo.getMaxEnergy(), _serverSeatInfo.getLastCalMs());
            _m_fullNextRemainMs = _serverSeatInfo.getFullGetNextRemainMs();
        }
        
        
        public long id { get { return _m_baseRef.seat_id; } }
        [NotNull] public ChildSeatRefObj baseRef { get { return _m_baseRef; } }
        public int energy { get { return _m_lazyCd.getEnergy(); } }
        public int maxEnergy { get { return _m_lazyCd.maxEnergy; } }
        public long remainMs { get { return _m_lazyCd.getRemainMs(); } }
        public long lastCalTime { get { return _m_lazyCd.lastCalTime; } }
        public long fullNextRemainMs { get { return _m_fullNextRemainMs; } }
        public ChildInfo childInfo { get { return _m_childInfo; } }

        
        [Pure]
        public bool isUnlock()
        {
            return baseRef.seat_unlock_condition.IsEnable(null);
        }
        public string getClassroomVideoName()
        {
            if (_m_childInfo == null)
                return GRefdataCoreMgr.instance.npGeneral.child_default_classroom_video_name;
            
            return _m_childInfo.getClassroomVideoName();
        }
        public bool needShowRedTip()
        {
            if (!isUnlock())
                return false;
            
            return childInfo == null || // 席位上没有子嗣时显示红点 
                   string.IsNullOrEmpty(childInfo.name) || // 席位上的子嗣没有命名时显示红点
                   energy > GRefdataCoreMgr.instance.npGeneral.child_seat_energy_red_tip_threshold; // 席位的脑力值达到指定数值时显示红点
        }
        

        internal void _setChildInfo(ChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
        }
        internal void _updateData(Child_SeatInfo _seatInfo)
        {
            if (_seatInfo == null || _seatInfo.getSeatId() != id)
                return;
            
            _m_lazyCd.updateInfo(_seatInfo.getEnergy(), _seatInfo.getMaxEnergy(), _seatInfo.getLastCalMs());
            _m_fullNextRemainMs = _seatInfo.getFullGetNextRemainMs();
        }


        private class LazyCd 
        {
            private int _m_energy;
            private int _m_maxEnergy;
            private long _m_lastCalMs;
            
            
            public LazyCd(int _energy, int _maxEnergy, long _lastCalMs)
            {
                updateInfo(_energy, _maxEnergy, _lastCalMs);
            }
            

            public int maxEnergy { get { return _m_maxEnergy; } }
            public long lastCalTime { get { return _m_lastCalMs; } }
            

            public void updateInfo(int _energy, int _maxEnergy, long _lastCalMs)
            {
                _m_energy = _energy;
                _m_maxEnergy = _maxEnergy;
                _m_lastCalMs = _lastCalMs;
            }
            

            private void _calcCheck()
            {
                // 已经达到上限不需要处理
                if (_m_energy >= _m_maxEnergy)
                    return;

                //经过时间
                long serverTimeNow = FpsAndPingMgr.instance.serverTimeTag;
                long recoverTime = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_S * 1000;
                long spanMs = serverTimeNow - _m_lastCalMs;
                if (spanMs > recoverTime)
                {
                    int times = (int)(spanMs / recoverTime);
                    int finalCount = times * 1 + _m_energy;
                    //超过上限，只能取上限值
                    if (finalCount >= _m_maxEnergy)
                    {
                        finalCount = _m_maxEnergy;
                        _m_lastCalMs = serverTimeNow;
                    }
                    else
                    {
                        //之前标记时间加上计算需要的时间
                        _m_lastCalMs += (times * recoverTime);
                    }

                    _m_energy = finalCount;
                }
            }

            public int getEnergy()
            {
                //结算一次
                _calcCheck();

                return _m_energy;
            }

            public long getRemainMs()
            {
                //结算一次
                _calcCheck();

                // count已经满了，直接返回0
                if (_m_energy >= _m_maxEnergy)
                    return 0;

                long recoverTime = GRefdataCoreMgr.instance.npGeneral.child_seat_recover_S * 1000;
                long elapsedMs = FpsAndPingMgr.instance.serverTimeTag - _m_lastCalMs;
                long remainMs = recoverTime - elapsedMs;
                if (remainMs < 0)
                    remainMs = 0;

                return remainMs;
            }
        }
    }
}