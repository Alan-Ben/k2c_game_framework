using Common.DungeonObj;

namespace GOE
{
    public class MiddayDungeonBossInfo
    {
        /// <summary>
        /// 波数
        /// </summary>
        private int _m_wave;
        /// <summary>
        /// 扣除血量
        /// </summary>
        private long _m_deductedHp;
        
        public int wave => _m_wave;
        public long deductedHp => _m_deductedHp;
        
        public void update(MiddayDungeon_BossInfo _bossInfo)
        {
            if(_bossInfo == null)
                return;
            if (_bossInfo.getWave() <= 0)
            {
                _m_wave = 1;
                _m_deductedHp = 0;
                return;
            }
            
            _m_wave = _bossInfo.getWave();
            _m_deductedHp = _bossInfo.getDeductedHp();
        }

        public void reset()
        {
            _m_wave = 1;
            _m_deductedHp = 0;
        }
        
        
    }
}