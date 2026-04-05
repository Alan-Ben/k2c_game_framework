using System.Collections.Generic;

namespace GOE
{
    public class TowerChallengeResult
    {
        private bool _m_isSuc; 
        private List<NPCommonCostItem> _m_costItems;
        private long _m_oldChapterId; // 当前章节
        private int _m_oldLevel; // 当前Stage中的索引 1-10
        
        private long _m_targetChapterId; // 当前章节
        private int _m_targetLevel; // 当前Stage中的索引 1-10
        private int _m_upLevel;
        private long _m_targetPlayerCid; // 当前关卡占领的玩家Cid

        public List<NPCommonCostItem> costItems => _m_costItems;
        public bool isSuc=> _m_isSuc;
        public bool hasItem => _m_costItems.Count > 0;

        public long oldChapterId => _m_oldChapterId;
        public int oldLevel => _m_oldLevel;
        
        public long targetChapterId => _m_targetChapterId;
        public int targetLevel => _m_targetLevel;
        public long targetPlayerCid => _m_targetPlayerCid;

        
        public TowerChallengeResult(bool _isDefeat, long _playerCid, List<NPCommonCostItem> _costItems, long _oldChapterId, int _oldLevel, long _targetChapterId, int _targetLevel)
        {
            _m_isSuc = _isDefeat;
            _m_costItems = _costItems;
            _m_oldChapterId = _oldChapterId;
            _m_oldLevel = _oldLevel;
            _m_targetChapterId = _targetChapterId;
            _m_targetLevel = _targetLevel;
            _m_targetPlayerCid = _playerCid;
        }
    }
}