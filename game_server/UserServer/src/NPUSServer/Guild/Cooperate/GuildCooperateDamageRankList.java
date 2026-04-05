package NPUSServer.Guild.Cooperate;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RankObj.Rank_BaseItem;
import USDB.Bo.GuildCooperateDamageRankBO;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/**
 * 联盟协作伤害排行榜管理类
 * 
 * 主要功能：
 * 1. 管理单个公会内的伤害排行榜数据
 * 2. 提供排行榜的查询、更新、排序功能
 * 3. 支持实时排名重定位和数据同步
 * 4. 线程安全的并发访问控制
 * 
 * 设计特点：
 * - 基于List+Map双重索引提高查询效率
 * - 使用冒泡算法实现实时排名调整
 * - 支持完整排行榜和个人排名查询
 * - 线程安全的锁机制
 */
public class GuildCooperateDamageRankList
{
    // 所属公会ID
    private final GuildCooperateInfo _m_cooperateInfo;

    // 排行榜数据列表（按排名排序）
    private List<GuildCooperateDamageRankItem> _m_itemList;

    /**
     * 构造函数
     * @param _cooperateInfo 公会协作信息对象
     */
    public GuildCooperateDamageRankList(GuildCooperateInfo _cooperateInfo)
    {
        _m_cooperateInfo = _cooperateInfo;
        _m_itemList = new ArrayList<>();
    }

    /**
     * 加锁
     */
    private void _lock()
    {
        _m_cooperateInfo._lock();
    }

    /**
     * 解锁
     */
    private void _unlock()
    {
        _m_cooperateInfo._unlock();
    }

    /**
     * 初始化排行榜数据
     * @param _rankBo
     */
    public void initFromDB(GuildCooperateDamageRankBO _rankBo)
    {
        _m_itemList.add(new GuildCooperateDamageRankItem(_rankBo));
    }

    /**
     * 初始化完成处理
     */
    public void onInited(List<Long> _memberCidList)
    {
        Set<Long> cidSet = new HashSet<>();
        for (GuildCooperateDamageRankItem rankItem : _m_itemList)
        {
            cidSet.add(rankItem.getCid());
        }
        for (Long cid : _memberCidList)
        {
            if (!cidSet.contains(cid))
            {
                GuildCooperateDamageRankItem newItem = new GuildCooperateDamageRankItem(cid);
                _m_itemList.add(newItem);
            }
        }

        _m_itemList.sort((o1, o2) ->
        {
            int damageCompare = Long.compare(o1.getTotalDamage(), o2.getTotalDamage());
            if (damageCompare == 0)
                return Long.compare(o1.getLastUpdateTimeMs(),o2.getLastUpdateTimeMs());

            return -damageCompare;
        });

        for (int i = 0; i < _m_itemList.size(); i++)
        {
            _m_itemList.get(i).setRank(i + 1);
        }
    }

    /**
     * 查询玩家的排行榜记录
     *
     * 线程安全：使用锁保护并发访问
     *
     * @param _cid 玩家CID
     * @return 排行榜项，如果不存在返回null
     */
    public GuildCooperateDamageRankItem lookupItem(long _cid)
    {
        _lock();
        try
        {
            for (GuildCooperateDamageRankItem item : _m_itemList)
            {
                if (item.getCid() == _cid)
                    return item;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新玩家的伤害记录
     * 
     * 执行流程：
     * 1. 查找或创建玩家排行榜记录
     * 2. 更新伤害数据
     * 3. 重新定位排名位置
     * 4. 维护双重索引的一致性
     * 
     * 线程安全：全程加锁保护
     * 
     * @param _cid 玩家CID
     * @param _additionalDamage 新增伤害值
     * @param _timeMs 更新时间
     */
    public void updatePlayerDamage(long _cid, long _additionalDamage, long _timeMs)
    {
        if (_additionalDamage <= 0)
            return;

        _lock();
        try
        {
            GuildCooperateDamageRankItem rankItem = lookupItem(_cid);
            if (rankItem != null)
            {
                // 更新现有记录
                rankItem.addDamage(_m_cooperateInfo.getUSServer().getBM(), _additionalDamage, _timeMs);
            } else
            {
                // 创建新记录
                GuildCooperateDamageRankBO newBo = new GuildCooperateDamageRankBO();
                newBo.setGuildId(_m_cooperateInfo.getUSServer().getBM(), _m_cooperateInfo.getGuildInfo().getGuildId());
                newBo.setCid(_m_cooperateInfo.getUSServer().getBM(), _cid);
                newBo.setTotalDamage(_m_cooperateInfo.getUSServer().getBM(), _additionalDamage);
                newBo.setLastUpdateTimeMs(_m_cooperateInfo.getUSServer().getBM(), _timeMs);
                newBo.insert(_m_cooperateInfo.getUSServer().getBM());

                // 创建新记录
                rankItem = new GuildCooperateDamageRankItem(newBo);
                _m_itemList.add(rankItem);
                // 设置初始排名
                rankItem.setRank(_m_itemList.size());
            }

            // 重新定位玩家的排行位置
            _relocateItem(rankItem);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重新定位排行榜项的位置
     * 
     * 算法说明：
     * 使用冒泡算法从当前位置向前冒泡，直到找到正确位置
     * 比较规则：总伤害降序，相同伤害时按更新时间升序
     * 
     * 线程安全：调用前已经加锁
     * 
     * @param _rankItem 需要重新定位的排行榜项
     */
    private void _relocateItem(GuildCooperateDamageRankItem _rankItem)
    {
        int oriRank = _rankItem.getRank();
        // 从当前位置向前冒泡
        int i = oriRank - 1;
        
        for (; i > 0; i--)
        {
            GuildCooperateDamageRankItem frontItem = _m_itemList.get(i - 1);
            
            // 判断是否需要向前冒泡
            boolean shouldPromote = false;
            
            if (_rankItem.getTotalDamage() > frontItem.getTotalDamage())
            {
                // 伤害更高，需要向前冒泡
                shouldPromote = true;
            } else if (_rankItem.getTotalDamage() == frontItem.getTotalDamage() 
                      && _rankItem.getLastUpdateTimeMs() < frontItem.getLastUpdateTimeMs())
            {
                // 伤害相同但更新时间更早，需要向前冒泡
                shouldPromote = true;
            }
            
            if (shouldPromote)
            {
                // 向后移动被超过的项
                _m_itemList.set(i, frontItem);
                frontItem.setRank(i + 1);
            } else
            {
                // 找到正确位置，停止冒泡
                break;
            }
        }

        // 设置项目到最终位置
        _m_itemList.set(i, _rankItem);
        _rankItem.setRank(i + 1);
    }

    /**
     * 获取完整排行榜数据
     * 
     * 返回公会内所有玩家的排行榜协议对象列表
     * 
     * 线程安全：使用锁保护数据读取
     * 
     * @return 完整排行榜的协议对象列表
     */
    public List<Rank_BaseItem> makeAllRankProtoList()
    {
        _lock();
        try
        {
            if (_m_itemList.isEmpty())
                return new ArrayList<>();

            List<Rank_BaseItem> allRankList = new ArrayList<>();
            for (GuildCooperateDamageRankItem item : _m_itemList)
            {
                if (item != null)
                {
                    allRankList.add(item.makeProto());
                }
            }
            return allRankList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清空排行榜数据（重置时调用）
     */
    protected void _clearDamageRankData()
    {
        _lock();
        try
        {
            // 清空内存数据
            _m_itemList.forEach(GuildCooperateDamageRankItem::clearData);

            // 清空数据
            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("total_damage", 0);
            updateValue.addValueObj("last_update_time_ms", 0);

            _m_cooperateInfo.getUSServer().getBM().getBM(GuildCooperateDamageRankBO.class)
                    .update("guild_id",_m_cooperateInfo.getGuildInfo().getGuildId(), updateValue);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 移除玩家的排行榜数据（玩家离开公会时调用）
     * @param _cid
     */
    public void removePlayerData(long _cid)
    {
        _lock();
        try
        {
            GuildCooperateDamageRankItem rankItem = lookupItem(_cid);
            if (rankItem != null)
            {
                // 从内存中移除
                _m_itemList.remove(rankItem);
                // 从数据库中删除
                rankItem.discard(_m_cooperateInfo.getUSServer().getBM());

                // 重新调整排名
                for (int i = rankItem.getRank() - 1; i < _m_itemList.size(); i++)
                {
                    _m_itemList.get(i).setRank(i + 1);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 新增玩家处理
     * @param _cid 玩家CID
     */
    public void onNewMemberAdded(long _cid)
    {
        _lock();
        try
        {
            GuildCooperateDamageRankItem rankItem = lookupItem(_cid);
            if (rankItem == null)
            {
                // 创建新记录
                rankItem = new GuildCooperateDamageRankItem(_cid);
                _m_itemList.add(rankItem);
                // 设置初始排名
                rankItem.setRank(_m_itemList.size());
            }
        } finally
        {
            _unlock();
        }
    }
}