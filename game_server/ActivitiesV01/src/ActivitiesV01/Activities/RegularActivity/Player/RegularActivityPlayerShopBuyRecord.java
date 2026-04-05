package ActivitiesV01.Activities.RegularActivity.Player;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import ActivitiesV01.Bo.RegularActivityPlayerShopBuyRecordBO;
import Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord;
import NPCommon.DB.BM.BM;

/**
 * 活动玩家商店购买记录
 */
public class RegularActivityPlayerShopBuyRecord
{
    // 数据库ID
    private long _m_dbId;
    // 商品id
    private long _m_itemId;
    // 购买数量
    private long _m_hadBuyCount;

    /**
     * 构造函数
     * @param _bo 数据库对象
     */
    public RegularActivityPlayerShopBuyRecord(RegularActivityPlayerShopBuyRecordBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_itemId = _bo.getItemId();
        _m_hadBuyCount = _bo.getHadBuyCount();
    }

    public long getItemId()
    {
        return _m_itemId;
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
     * @param _bmObj
     * @param _addCount 增加数量
     * @return 是否成功增加
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
        _bmObj.getBM(RegularActivityPlayerShopBuyRecordBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 构造协议
     * @return

     */
    public RegularActivity_ShopBuyRecord makeProto()
    {
        RegularActivity_ShopBuyRecord proto = new RegularActivity_ShopBuyRecord();
        proto.setItemId(_m_itemId);
        proto.setBuyCount(_m_hadBuyCount);
        return proto;
    }
}
