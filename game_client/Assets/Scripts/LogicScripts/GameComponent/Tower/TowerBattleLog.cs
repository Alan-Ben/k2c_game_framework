using Common.TowerObj;

namespace GOE
{
    public class TowerBattleLog
    {
        private long _m_chapterId;
        private int _m_level;
        private bool _m_isDefendSuc;// 是否防守成功
        private long _m_happenTs; // 发生时间戳
        private long _m_playerCid; // 玩家cid
        
        
        public long playerCid => _m_playerCid;
        public string desc;
        public bool isDefendSuc => _m_isDefendSuc;
        public long happenTs => _m_happenTs;
        public TowerBattleLog(Tower_ReportInfo reportInfo)
        {
            if (reportInfo == null)
                return;
            _m_playerCid = reportInfo.getAttackerCid();
            Tower_PosInfo posInfo = reportInfo.getPosInfo();
            _m_chapterId = posInfo.getChapterId();
            _m_level = posInfo.getLevel();
            _m_isDefendSuc = !reportInfo.getIsSucc();
            _m_happenTs = reportInfo.getTimestamp();
            TowerChapterRefObj chapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_m_chapterId);
            if(isDefendSuc)
                desc = TextTranslate.instance.getLanguage(TransKeyConst.tower_log_defend_suc_desc, chapterRefObj.getLevelName(_m_level));
            else
                desc = TextTranslate.instance.getLanguage(TransKeyConst.tower_log_defend_fail_desc, chapterRefObj.getLevelName(_m_level),
                    reportInfo.getDownLevel());
        }
    }
}