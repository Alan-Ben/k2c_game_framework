package NPUSServer.NPUSUserMgr.UserComp.ShopComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import USDB.Bo.PlayerForeverShopBuyRecordBO;

public class PlayerForeverShopBuyRecord
{
    private long _m_dbId;
    private long _m_shopItemRefId;
    private int _m_buyCount;

    public PlayerForeverShopBuyRecord(PlayerForeverShopBuyRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_shopItemRefId = _bo.getShopItemRefId();
        _m_buyCount = _bo.getBuyCount();
    }

    public long getShopItemRefId()
    {
        return _m_shopItemRefId;
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
        _bm.getBM(PlayerForeverShopBuyRecordBO.class).update("id", _m_dbId, updateValue);
    }
}