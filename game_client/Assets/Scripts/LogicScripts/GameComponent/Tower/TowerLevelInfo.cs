using Common.TowerObj;

namespace GOE
{
    public class TowerLevelInfo
    {
        private long _m_chapter;
        /// <summary>
        /// 数值是Stage里索引值1-10，不是章节里的关卡索引值
        /// </summary>
        private int _m_level;
        private long _m_power;
        private long _m_towerCoinCount;
        private long _m_playerCid;
        private TowerChapterRefObj _m_chapterRefObj;

        public TowerLevelInfo(long _chapter, int _level)
        {
            _m_chapter = _chapter;
            _m_level = _level;
            _m_chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(_chapter);
            if (_m_chapterRefObj != null)
            {
                _m_power = _m_chapterRefObj.getBossPower(_m_level);
                _m_towerCoinCount = _m_chapterRefObj.getTowerCoinCount(_m_level);
            }
        }
        public TowerLevelInfo(TowerChapterRefObj _chapter, int _level)
        {
            if (_chapter == null) 
                return;
            _m_chapter = _chapter.id;
            _m_level = _level;
            _m_chapterRefObj = _chapter;
            _m_power = _m_chapterRefObj.getBossPower(_m_level);
            _m_towerCoinCount = _m_chapterRefObj.getTowerCoinCount(_m_level);

        }
        
        public TowerLevelInfo(string _bossName, long _power, long _chapter, int _level)
        {
            _m_chapter = _chapter;
            _m_level = _level;
            _m_chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(_chapter);
            _m_power = _power;
            if (_m_chapterRefObj != null)
            {
                _m_towerCoinCount = _m_chapterRefObj.getTowerCoinCount(_m_level);
            }
        }

        public void updatePlayerInfo(long playerCid)
        {
            _m_playerCid = playerCid;
        }

        /// <summary>
        /// 当前章节
        /// </summary>
        public long chapter => _m_chapter;
        
        /// <summary>
        /// 章节信息
        /// </summary>
        public TowerChapterRefObj chapterRefObj => _m_chapterRefObj;
        /// <summary>
        /// 当前阶段索引
        /// </summary>
        public int level => _m_level;
        /// <summary>
        /// 关卡守卫名字
        /// </summary>
        public string bossName => TextTranslate.instance.getLanguage(_m_chapterRefObj.boss_name);
        /// <summary>
        /// 关卡战力
        /// </summary>
        public long power => _m_power;
        
        /// <summary>
        /// 每日金币产出
        /// </summary>
        public long towerCoinCount => _m_towerCoinCount;
        /// <summary>
        /// 关卡名 区分了top层和普通层
        /// </summary>
        public string levelName => _m_chapterRefObj?.getLevelName( _m_level);
        
        /// <summary>
        /// 这个关卡所在的总关卡数
        /// </summary>
        public long totalLevel => _m_chapterRefObj?.getTotalLevel(_m_level) ?? 0;
        
        /// <summary>
        /// 这个关卡所在的章节关卡数
        /// </summary>
        public long chapterLevel => _m_level;

        /// <summary>
        /// banner背景
        /// </summary>
        public NPGTextureIndex banner_tex => _m_chapterRefObj?.banner_tex;
        /// <summary>
        /// boss形象	
        /// </summary>
        public NPGTextureIndex boss_icon => _m_chapterRefObj?.boss_icon;

        /// <summary>
        /// 是否是玩家当前所在关卡
        /// </summary>
        public bool isMyCurTowerLevel => NPPlayer.instance.towerComp.curChapterId == _m_chapter && NPPlayer.instance.towerComp.curLevel == _m_level;
        
        public TowerResearchRefObj researchRefObj
        {
            get
            {
                if (_m_chapterRefObj == null || _m_chapterRefObj.research_list == null)
                    return null;

                foreach (TowerResearchRefObj researchRef in _m_chapterRefObj.research_list)
                {
                    if (researchRef != null && researchRef.level == _m_level)
                        return researchRef;
                }
                return null;
            }
        }
        
        /// <summary>
        /// 驻扎玩家cid，如果为0则表示没有驻扎玩家
        /// </summary>
        public long playerCid => _m_playerCid;

        public bool isPVE => _m_chapterRefObj == null ? true : _m_chapterRefObj.if_pve_chapter;
        
        /// <summary>
        /// 按照章节和关卡从从小到大排序
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Sort(TowerLevelInfo a, TowerLevelInfo b)
        {
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            //按照章节和关卡从从小到大排序
            if (a._m_chapter == b._m_chapter)
                return a._m_level.CompareTo(b._m_level);
            return a._m_chapter.CompareTo(b._m_chapter);
        }
    }
}