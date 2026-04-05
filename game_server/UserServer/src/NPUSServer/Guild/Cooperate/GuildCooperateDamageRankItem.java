package NPUSServer.Guild.Cooperate;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RankObj.Rank_BaseItem;
import NPCommon.DB.BM.BM;
import USDB.Bo.GuildCooperateDamageRankBO;

/**
 * 联盟协作伤害排行榜数据项
 * 
 * 主要功能：
 * 1. 存储单个玩家的排行榜信息（CID、总伤害、排名等）
 * 2. 提供伤害数据更新功能
 * 3. 构造协议对象用于客户端显示
 * 
 * 设计特点：
 * - 使用现有的Rank_BaseItem协议结构
 * - 支持伤害累计和排名更新
 * - 记录最后更新时间用于数据同步
 */
public class GuildCooperateDamageRankItem
{
    private long _m_dbId;
    // 玩家CID
    private final long _m_cid;
    // 累计总伤害
    private long _m_totalDamage;
    // 当前排名
    private int _m_rank;
    // 最后更新时间
    private long _m_lastUpdateTimeMs;

    public GuildCooperateDamageRankItem(GuildCooperateDamageRankBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_totalDamage = _bo.getTotalDamage();
        _m_lastUpdateTimeMs = _bo.getLastUpdateTimeMs();
        _m_rank = 0; // 初始排名为0，后续会被设置
    }

    public GuildCooperateDamageRankItem(Long _cid)
    {
        _m_dbId = 0;
        _m_cid = _cid;
        _m_totalDamage = 0;
        _m_lastUpdateTimeMs = 0;
        _m_rank = 0; // 初始排名为0，后续会被设置
    }

    /**
     * 获取玩家CID
     * @return 玩家CID
     */
    public long getCid()
    {
        return _m_cid;
    }

    /**
     * 设置排名
     * @param _rank 排名
     */
    public void setRank(int _rank)
    {
        _m_rank = _rank;
    }

    /**
     * 获取排名
     * @return 当前排名
     */
    public int getRank()
    {
        return _m_rank;
    }

    /**
     * 获取累计总伤害
     * @return 累计总伤害
     */
    public long getTotalDamage()
    {
        return _m_totalDamage;
    }

    /**
     * 获取最后更新时间
     * @return 最后更新时间毫秒
     */
    public long getLastUpdateTimeMs()
    {
        return _m_lastUpdateTimeMs;
    }

    /**
     * 增加伤害值
     * 
     * @param _additionalDamage 额外伤害值
     * @param _timeMs 更新时间
     */
    public void addDamage(BM _bmObj, long _additionalDamage, long _timeMs)
    {
        if (_additionalDamage <= 0)
            return;

        _m_totalDamage += _additionalDamage;
        _m_lastUpdateTimeMs = _timeMs;

        if (_m_dbId == 0)
        {
            // 插入新记录
            GuildCooperateDamageRankBO newBo = new GuildCooperateDamageRankBO();
            newBo.setCid(_bmObj,_m_cid);
            newBo.setTotalDamage(_bmObj,_m_totalDamage);
            newBo.setLastUpdateTimeMs(_bmObj, _m_lastUpdateTimeMs);
            newBo.insert(_bmObj);
            _m_dbId = newBo.getId();
        }else
        {
            // 更新现有记录
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("total_damage", _m_totalDamage);
            updateValue.addValueObj("last_update_time_ms", _m_lastUpdateTimeMs);
            _bmObj.getBM(GuildCooperateDamageRankBO.class).update("id", _m_dbId, updateValue);
        }
    }

    /**
     * 构造协议对象
     * 
     * 字段映射：
     * - key → 玩家CID
     * - sourceId → 未使用，设为0
     * - score → 累计总伤害
     * - rank → 当前排名
     * 
     * @return 排行榜基础协议对象
     */
    public Rank_BaseItem makeProto()
    {
        return new Rank_BaseItem(_m_cid, 0L, _m_totalDamage, _m_rank);
    }

    /**
     * 销毁数据
     * @param _bm
     */
    public void discard(BM _bm)
    {
        if (_m_dbId != 0)
        {
            _bm.getBM(GuildCooperateDamageRankBO.class).delAll("id", _m_dbId);
            _m_dbId = 0;
        }
    }

    public void clearData()
    {
        _m_totalDamage = 0;
        _m_lastUpdateTimeMs = 0;
    }
}