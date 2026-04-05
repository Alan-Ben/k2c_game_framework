package NPUSServer.NPUSUserMgr.UserComp.GachaComp.Record;

import Common.GachaObj.Gacha_RecordInfo;
import NPCommon.DB.BM.BM;
import USDB.Bo.PlayerGachaRecordBO;

public class GachaPoolRecordInfo
{
    private long _m_dbId;
    private long _m_itemId;
    private int _m_rollTimeSec;

    public GachaPoolRecordInfo(PlayerGachaRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_itemId = _bo.getItemId();
        _m_rollTimeSec = _bo.getRollTimeSec();
    }

    public void discard(BM _bm)
    {
        _bm.getBM(PlayerGachaRecordBO.class).delAll("id", _m_dbId);
    }

    /**
     * 构造协议信息
     * @return
     */
    public Gacha_RecordInfo toProto()
    {
        Gacha_RecordInfo recordInfo = new Gacha_RecordInfo();
        recordInfo.setItemId(_m_itemId);
        recordInfo.setRollTimeSec(_m_rollTimeSec);
        recordInfo.setDbId(_m_dbId);
        return recordInfo;
    }
}
