using System;
using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;

namespace GOE
{
    public class GuildDungeonLog
    {
        private EGuildDungeon_LogType _m_logType;
        private int _m_timeS = 0; // 日志时间戳
        
        private long _m_cid;
        private long _m_monsterId;// 怪物ID
        private long _m_damage;// 伤害数值
        private bool _m_isKill;
        private long _m_dungeonId;
        private EGuildDungeon_StartType _m_startType;
        private long _m_startCost;
        
        public EGuildDungeon_LogType logType => _m_logType;
        public long cid => _m_cid;
        public int timeS => _m_timeS;
        public long monsterId => _m_monsterId;
        public long damage => _m_damage;
        public bool isKill => _m_isKill;
        public long dungeonId => _m_dungeonId;
        public EGuildDungeon_StartType startType => _m_startType;
        public long startCost => _m_startCost;

      

        public GuildDungeonLog(EGuildDungeon_LogType _logType, int _timeS)
        {
            _m_logType = _logType;
            _m_timeS = _timeS;
          
        }

        public void initAttackInfo(long _cid, long _monsterId, long _damage)
        {
            _m_cid = _cid;
            _m_monsterId = _monsterId;
            _m_damage = _damage;
            _m_isKill = false;
        }

        public void initKillInfo(long _cid, long _monsterId)
        {
            _m_cid = _cid;
            _m_monsterId = _monsterId;
            _m_damage = 0;
            _m_isKill = true;
        }

        public void initStartInfo(long _dungeonId, long _cid, EGuildDungeon_StartType _startType, long _startCost)
        {
            _m_dungeonId = _dungeonId;
            _m_cid = _cid;
            _m_startType = _startType;
            _m_startCost = _startCost;
            _m_isKill = false;
        }
        
        public void combineAttackLog(long _damage, int _timeMs)
        {
            _m_damage += _damage;
            if (_m_timeS < _timeMs)
            {
                _m_timeS = _timeMs;
            }
        }
        
    }
}