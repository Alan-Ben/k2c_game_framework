package NPUSServer.CommonActivityMgr.Core.CrystalGiftPack;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.CommonFuncObj.CrystalGiftPack_BuyRecord;
import NPCommon.DB.BM.BM;
import USDB.Bo.ActivityPlayerCrystalGiftPackBuyRecordBO;

/**
 * 活动玩家钻石礼包购买记录
 */
public class ActivityPlayerCrystalGiftPackBuyRecord
{
    // 数据库ID
    private long _m_dbId;
    // 礼包id
    private long _m_giftPackId;
    // 购买数量
    private long _m_hadBuyCount;

    /**
     * 构造函数
     * @param _bo 数据库对象
     */
    public ActivityPlayerCrystalGiftPackBuyRecord(ActivityPlayerCrystalGiftPackBuyRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_giftPackId = _bo.getItemId();
        _m_hadBuyCount = _bo.getHadBuyCount();
    }

    public long getGiftPackId()
    {
        return _m_giftPackId;
    }

    /**
     * 获取购买数量
     * @return 购买数量
     */
    public long getHadBuyCount()
    {
        return _m_hadBuyCount;
    }

    /**
     * 增加购买数量
     * @param _bmObj 数据库管理器
     * @param _addCount 增加数量
     */
    public void addBuyCount(BM _bmObj, int _addCount)
    {
        if (_addCount <= 0)
            return;

        // 更新内存数据
        _m_hadBuyCount += _addCount;

        // 更新数据库
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_buy_count", _m_hadBuyCount);
        _bmObj.getBM(ActivityPlayerCrystalGiftPackBuyRecordBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 构造协议
     * @return 购买记录协议
     */
    public CrystalGiftPack_BuyRecord makeProto()
    {
        CrystalGiftPack_BuyRecord proto = new CrystalGiftPack_BuyRecord();
        proto.setGiftPackId(_m_giftPackId);
        proto.setHadBuyCount(_m_hadBuyCount);
        return proto;
    }
}
