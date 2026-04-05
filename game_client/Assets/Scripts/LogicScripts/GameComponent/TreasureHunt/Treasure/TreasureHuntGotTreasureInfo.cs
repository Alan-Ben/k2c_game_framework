using Common.TreasureHuntObj;

namespace GOE
{
    public class TreasureHuntGotTreasureInfo : _ITreasureHuntTreasureInfo
    {
        private long _m_lTreasureId;
        private TreasureHuntTreasureRefObj _m_treasureRefObj;
        private TreasureHuntTreasureOutputRefObj _m_treasureOutputRefObj;
        private TreasureHuntTreasureSkillInfo _m_skillInfo;
        private long _m_lGainTimeMs; // 获得时间 ms
        
        public TreasureHuntGotTreasureInfo(TreasureHunt_TreasureInfo _treasureInfo)
        {
            updateTreasureInfo(_treasureInfo);
        }
        
        public long treasureId { get { return _m_lTreasureId; } }
        public TreasureHuntTreasureRefObj treasureRefObj {
            get
            {
                if (_m_treasureRefObj == null || _m_treasureRefObj.id != _m_lTreasureId)
                {
                    _m_treasureRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureRefCore.getRef(_m_lTreasureId);
                    _m_treasureOutputRefObj = GRefdataCoreMgr.instance.treasureHuntTreasureOutputRefCore.getRef(_m_lTreasureId);
                }
                return _m_treasureRefObj;
            }
        }

        public TreasureHuntTreasureOutputRefObj treasureOutputRefObj { get { return _m_treasureOutputRefObj; } }
        
        public ETreasureHuntTreasureState treasureState
        {
            get
            {
                // 使用这个类, 奇物一定是已获取了, 所以不用判断是否为未获取奇物
                
                // 若技能未激活, 则奇物状态为未激活奇物
                if(_m_skillInfo == null 
                   || (_m_skillInfo.skillState is ETreasureHuntSkillState.LOCK or ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE))
                    return ETreasureHuntTreasureState.GOT_NOT_ACTIVATE;

                // 技能已激活, 则奇物状态为已激活奇物
                return ETreasureHuntTreasureState.GOT_ACTIVATED;
            }
        }
        public long gainTimeMs { get { return _m_lGainTimeMs; } }
        public _ITreasureHuntSkillInfo skillInfo { get { return _m_skillInfo; } }

        public void updateTreasureInfo(TreasureHunt_TreasureInfo _treasureInfo)
        {
            if(_treasureInfo == null)
                return;
            
            _m_lTreasureId = _treasureInfo.getTreasureId();
            updateTreasureSkillLevel(_treasureInfo.getSkillLevel());
            _m_lGainTimeMs = _treasureInfo.getGainTimeMs();
        }

        /// <summary>
        /// 更新奇物等级
        /// </summary>
        public void updateTreasureSkillLevel(int _level)
        {
            if (_m_skillInfo == null)
            {
                _m_skillInfo = new TreasureHuntTreasureSkillInfo(treasureRefObj?.skill_id ?? 0, true, _level, treasureRefObj?.upgrade_cost);
            }
            else
            {
                _m_skillInfo.updateSkillLevel(_level);
            }
        }
    }
}