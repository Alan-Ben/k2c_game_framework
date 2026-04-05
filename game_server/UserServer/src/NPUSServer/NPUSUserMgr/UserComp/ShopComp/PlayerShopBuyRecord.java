package NPUSServer.NPUSUserMgr.UserComp.ShopComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import USDB.Bo.PlayerShopBuyRecordBO;

public class PlayerShopBuyRecord
{
    private long _m_dbId;
    private int _m_buyCount;

    public PlayerShopBuyRecord(PlayerShopBuyRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_buyCount = _bo.getBuyCount();
    }

    public int getBuyCount()
    {
        return _m_buyCount;
    }

    public void recordBuyCount(BM _bm, int _hasBuyNum)
    {
        _m_buyCount = _hasBuyNum;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("buy_count", _m_buyCount);
        _bm.getBM(PlayerShopBuyRecordBO.class).update("id", _m_dbId, updateValue);
    }
}
