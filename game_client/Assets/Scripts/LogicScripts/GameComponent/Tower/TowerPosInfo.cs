using Common.TowerObj;

namespace GOE
{
    public class TowerPosInfo
    {
        private long _m_chapterId; // 章节
        private int _m_level; // 章节关卡
        
        
        public long chapterId => _m_chapterId;
        public int level => _m_level;

        public TowerPosInfo(long chapterId, int level)
        {
            _m_chapterId = chapterId;
            _m_level = level;
        }
        public TowerPosInfo(Tower_PosInfo _posInfo)
        {
            updateInfo(_posInfo);
        }
        
        public void updateInfo(Tower_PosInfo _posInfo)
        {
            _m_level = _posInfo.getLevel();
            _m_chapterId = _posInfo.getChapterId();
        }
    }
}