package NPUSServer.Tower;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.TowerObj.Tower_OpponentInfo;
import Common.TowerObj.Tower_PosInfo;
import NPCommon.DB.BM.BM;
import USDB.Bo.TowerFloorBO;

public class TowerFloorInfo
{
    private long _m_chapterId;
    private int _m_chapterLevel;

    private long _m_dbId;
    private long _m_cid;

    private boolean _m_isDiscarded = false;

    public TowerFloorInfo(TowerFloorBO _bo)
    {
        _m_chapterId = _bo.getChapterId();
        _m_chapterLevel = _bo.getChapterLevel();

        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
    }

    public long getChapterId()
    {
        return _m_chapterId;
    }

    public int getChapterLevel()
    {
        return _m_chapterLevel;
    }

    public long getCid()
    {
        return _m_cid;
    }

    /**
     * 修改位置
     * @param _bmObj
     * @param _chapterId
     * @param _chapterLevel
     */
    public void chgPos(BM _bmObj, long _chapterId, int _chapterLevel)
    {
        _m_chapterId = _chapterId;
        _m_chapterLevel = _chapterLevel;

        ALMySqlUpdateValue update = new ALMySqlUpdateValue();
        update.addValueObj("chapter_id", _m_chapterId);
        update.addValueObj("chapter_level", _m_chapterLevel);
        _bmObj.getBM(TowerFloorBO.class).update("id", _m_dbId, update);
    }

    /**
     * 销毁数据
     * @param _bmObj
     */
    public void discard(BM _bmObj)
    {
        if (_m_isDiscarded)
            return;
        _m_isDiscarded = true;
        _bmObj.getBM(TowerFloorBO.class).delAll("id", _m_dbId);
    }

    public Tower_OpponentInfo makeOpponentInfo()
    {
        Tower_OpponentInfo opponentInfo = new Tower_OpponentInfo();
        opponentInfo.setPosInfo(new Tower_PosInfo(_m_chapterId, _m_chapterLevel));
        opponentInfo.setPlayerId(_m_cid);
        return opponentInfo;
    }
}
