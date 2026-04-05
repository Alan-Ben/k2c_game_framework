package NPUSServer.GachaPublicRecord;

import Common.GachaObj.Gacha_PublicRecordInfo;
import NPCommon.DB.BM.BM;
import USDB.Bo.GachaPublicRecordBO;

public class GachaPublicRecord
{
    private long _m_dbId;
    private long _m_itemId;
    private String _m_playerName;

    public GachaPublicRecord(GachaPublicRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_itemId = _bo.getItemId();
        _m_playerName = _bo.getPlayerName();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    /**
     * 销毁
     * @param _bm
     */
    public void discard(BM _bm)
    {
        _bm.getBM(GachaPublicRecordBO.class).delAll("id", _m_dbId);
    }

    public Gacha_PublicRecordInfo toProto()
    {
        Gacha_PublicRecordInfo recordInfo = new Gacha_PublicRecordInfo();
        recordInfo.setItemId(_m_itemId);
        recordInfo.setPlayerName(_m_playerName);
        recordInfo.setDbId(_m_dbId);
        return recordInfo;
    }
}
