package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Guarantee;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import NPCommon.Game.WeightQualityValueList;
import NPGameRes.Refs.RefGeneral;
import USDB.Bo.PlayerTreasureHuntGuaranteeBO;

/**
 * 太空寻宝区域保底信息
 * 数据对象封装，包含BO对象进行数据持久化
 */
public class TreasureHuntGuaranteeAreaInfo 
{
    private TreasureHuntGuaranteeMgr _m_mgr; // 所属管理器
    private long _m_dbId;
    private final long _m_areaId;
    private int _m_treasureGuaranteeCount; // 该区域奇物保底计数，缓存值
    private int _m_consecutiveHighQualityCount; // 连续获得高品质矿石次数，缓存值
    private int _m_consecutiveNoHighQualityCount; // 连续未获得高品质矿石次数，缓存值
    
    /**
     * 创建新的区域保底信息（无BO对象）
     * @param mgr 管理器
     * @param _areaId 区域ID
     */
    public TreasureHuntGuaranteeAreaInfo(TreasureHuntGuaranteeMgr mgr, long _areaId)
    {
        _m_mgr = mgr;
        _m_dbId = 0;
        _m_areaId = _areaId;
        _m_treasureGuaranteeCount = 0;
        _m_consecutiveHighQualityCount = 0;
        _m_consecutiveNoHighQualityCount = 0;
    }
    
    /**
     * 从现有BO对象创建区域保底信息
     * @param mgr 管理器
     * @param _bo BO对象
     */
    public TreasureHuntGuaranteeAreaInfo(TreasureHuntGuaranteeMgr mgr, PlayerTreasureHuntGuaranteeBO _bo)
    {
        _m_mgr = mgr;
        _m_dbId = _bo.getId();
        _m_areaId = _bo.getAreaId();
        _m_treasureGuaranteeCount = _bo.getTreasureGuaranteeCount();
        _m_consecutiveHighQualityCount = _bo.getConsecutiveHighQualityCount();
        _m_consecutiveNoHighQualityCount = _bo.getConsecutiveNoHighQualityCount();
    }
    
    /**
     * 获取区域ID
     * @return 区域ID
     */
    public long getAreaId()
    {
        return _m_areaId;
    }

    public BM getBM()
    {
        return _m_mgr.getComp().getUSServer().getBM();
    }
    
    /**
     * 获取奇物保底计数
     * @return 保底计数
     */
    public int getTreasureGuaranteeCount()
    {
        return _m_treasureGuaranteeCount;
    }
    
    /**
     * 获取连续获得高品质矿石次数
     * @return 连续获得高品质矿石次数
     */
    public int getConsecutiveHighQualityCount()
    {
        return _m_consecutiveHighQualityCount;
    }
    
    /**
     * 获取连续未获得高品质矿石次数
     * @return 连续未获得高品质矿石次数
     */
    public int getConsecutiveNoHighQualityCount()
    {
        return _m_consecutiveNoHighQualityCount;
    }
    
    /**
     * 增加保底计数并保存到数据库
     */
    public void incrementTreasureGuaranteeCount()
    {
        _m_treasureGuaranteeCount++;

        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("treasure_guarantee_count", _m_treasureGuaranteeCount);
            getBM().getBM(PlayerTreasureHuntGuaranteeBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 重置保底计数并保存到数据库
     */
    public void resetTreasureGuaranteeCount()
    {
        if (_m_treasureGuaranteeCount == 0)
            return;

        _m_treasureGuaranteeCount = 0;

        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("treasure_guarantee_count", _m_treasureGuaranteeCount);
            getBM().getBM(PlayerTreasureHuntGuaranteeBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 检查是否达到矿石保底条件
     * @return
     */
    public WeightQualityValueList checkActiveOreGuarantee()
    {
        if (RefGeneral.Ref().treasure_hunt_ore_guarantee_min_time == 0 || RefGeneral.Ref().treasure_hunt_ore_guarantee_max_time == 0)
            return null; // 没有设置矿石保底次数

        if (_m_consecutiveHighQualityCount >= RefGeneral.Ref().treasure_hunt_ore_guarantee_max_time)
            return RefGeneral.Ref().treasure_hunt_ore_guarantee_max_weight;

        if (_m_consecutiveNoHighQualityCount >= RefGeneral.Ref().treasure_hunt_ore_guarantee_min_time)
            return RefGeneral.Ref().treasure_hunt_ore_guarantee_min_weight;

        return null;
    }
    
    /**
     * 增加连续获得高品质矿石次数并保存到数据库
     */
    public void incrementConsecutiveHighQualityCount()
    {
        _m_consecutiveHighQualityCount++;
        _m_treasureGuaranteeCount++;
        _m_consecutiveNoHighQualityCount = 0; // 重置连续未获得计数

        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("treasure_guarantee_count", _m_treasureGuaranteeCount);
            updateValue.addValueObj("consecutive_high_quality_count", _m_consecutiveHighQualityCount);
            updateValue.addValueObj("consecutive_no_high_quality_count", _m_consecutiveNoHighQualityCount);
            getBM().getBM(PlayerTreasureHuntGuaranteeBO.class).update("id", _m_dbId, updateValue);
        }
    }
    
    /**
     * 增加连续未获得高品质矿石次数并保存到数据库
     */
    public void incrementConsecutiveNoHighQualityCount()
    {
        _m_consecutiveNoHighQualityCount++;
        _m_treasureGuaranteeCount++;
        _m_consecutiveHighQualityCount = 0; // 重置连续获得计数

        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("treasure_guarantee_count", _m_treasureGuaranteeCount);
            updateValue.addValueObj("consecutive_high_quality_count", _m_consecutiveHighQualityCount);
            updateValue.addValueObj("consecutive_no_high_quality_count", _m_consecutiveNoHighQualityCount);
            getBM().getBM(PlayerTreasureHuntGuaranteeBO.class).update("id", _m_dbId, updateValue);
        }
    }
    
    /**
     * 重置所有连续计数并保存到数据库
     */
    public void resetConsecutiveCounts()
    {
        if (_m_consecutiveHighQualityCount == 0 && _m_consecutiveNoHighQualityCount == 0)
            return;

        _m_consecutiveHighQualityCount = 0;
        _m_consecutiveNoHighQualityCount = 0;

        if (!tryCreateInDB())
        {
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("consecutive_high_quality_count", _m_consecutiveHighQualityCount);
            updateValue.addValueObj("consecutive_no_high_quality_count", _m_consecutiveNoHighQualityCount);
            getBM().getBM(PlayerTreasureHuntGuaranteeBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 是否可以触发奇物保底
     * @return
     */
    public boolean canTriggerTreasureGuarantee()
    {
        if (RefGeneral.Ref().treasure_hunt_treasure_guarantee_time == 0)
            return false;

        return _m_treasureGuaranteeCount >= RefGeneral.Ref().treasure_hunt_treasure_guarantee_time;
    }

    /**
     * 尝试在数据库中创建信息
     * @return
     */
    public boolean tryCreateInDB()
    {
        if(_m_dbId != 0)
            return false;

        PlayerTreasureHuntGuaranteeBO bo = new PlayerTreasureHuntGuaranteeBO();
        bo.setCid(getBM(), _m_mgr.getComp().getUserData().getCid());
        bo.setAreaId(getBM(), _m_areaId);
        bo.setTreasureGuaranteeCount(getBM(), _m_treasureGuaranteeCount);
        bo.setConsecutiveHighQualityCount(getBM(), _m_consecutiveHighQualityCount);
        bo.setConsecutiveNoHighQualityCount(getBM(), _m_consecutiveNoHighQualityCount);
        bo.insert(getBM());
        _m_dbId = bo.getId();

        return true;
    }
}