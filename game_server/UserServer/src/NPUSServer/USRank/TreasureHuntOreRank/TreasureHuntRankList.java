package NPUSServer.USRank.TreasureHuntOreRank;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.TreasureHuntObj.TreasureHunt_OreRankItem;
import USDB.Bo.PlayerTreasureHuntOreBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class TreasureHuntRankList
{
    // 矿石id
    private final long _m_oreId;

    private List<TreasureHuntRankItem> _m_itemList;
    private Map<Long, TreasureHuntRankItem> _m_itemMap;

    // 排行锁
    private MutexAtom _m_mutex;

    public TreasureHuntRankList(long _oreId)
    {
        _m_oreId = _oreId;
        _m_itemList = new ArrayList<>();
        _m_itemMap = new HashMap<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 初始化排行榜数据
     * @param _oreBoList
     */
    public void initFromDB(List<PlayerTreasureHuntOreBO> _oreBoList)
    {
        // 先排序
        _oreBoList.sort((item1, item2) ->
        {
            // 先比较最高记录
            int recordCompareResult = Integer.compare(item2.getMaxRecord(), item1.getMaxRecord());

            // 如果最高记录相同，则比较达成时间
            if (recordCompareResult == 0)
                recordCompareResult = Long.compare(item1.getReachMaxRecordTimeMs(), item2.getReachMaxRecordTimeMs());

            return recordCompareResult;
        });

        for (int i = 0; i < _oreBoList.size(); i++)
        {
            if (i >= 1000)
                break; // 只保留前1000名

            PlayerTreasureHuntOreBO oreBo = _oreBoList.get(i);
            if (oreBo == null)
                continue;

            // 检查配表是否存在
            TreasureHuntRankItem oreRankItem = new TreasureHuntRankItem(oreBo.getCid(), oreBo.getMaxRecord(), oreBo.getReachMaxRecordTimeMs());
            oreRankItem.setRank(i + 1);
            _m_itemList.add(oreRankItem);
            _m_itemMap.put(oreBo.getCid(), oreRankItem);
        }
    }

    /**
     * 查询玩家的最高记录
     * @param _cid
     * @return
     */
    public TreasureHuntRankItem lookupItem(long _cid)
    {
        _lock();
        try
        {
            return _m_itemMap.get(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新玩家的最高记录
     * @param _cid
     * @param _maxRecord
     */
    public void updatePlayerRecord(long _cid, int _maxRecord, long _timeMs)
    {
        _lock();
        try
        {
            TreasureHuntRankItem oreRankItem = lookupItem(_cid);
            if (oreRankItem != null)
            {
                int oriRecord = oreRankItem.getMaxRecord();
                if (oriRecord >= _maxRecord)
                    return;

                oreRankItem.updateMaxRecord(_maxRecord, _timeMs);
            } else
            {
                oreRankItem = new TreasureHuntRankItem(_cid, _maxRecord, _timeMs);
                _m_itemList.add(oreRankItem);
                // 设置初始排名
                oreRankItem.setRank(_m_itemList.size());
                _m_itemMap.put(_cid, oreRankItem);
            }

            // 重新定位玩家的排行位置
            _relocateItem(oreRankItem);

            // 如果排行超过1000名，则移除最后一名
            if (_m_itemList.size() > 1000)
            {
                TreasureHuntRankItem lastItem = _m_itemList.remove(_m_itemList.size() - 1);
                _m_itemMap.remove(lastItem.getCid());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重新定位玩家的排行位置
     * @param _oreRankItem
     */
    private void _relocateItem(TreasureHuntRankItem _oreRankItem)
    {
        _lock();
        try
        {
            int oriRank = _oreRankItem.getRank();
            // 使用冒泡算法，从当前位置向前冒泡
            int i = oriRank - 1;
            // 特殊处理可能排在第一位的情况
            for (; i > 0; i--)
            {
                TreasureHuntRankItem item = _m_itemList.get(i - 1);
                // 如果当前项的最高记录超过了前一项的最高记录，则需要向前冒泡
                boolean shouldPromote = _oreRankItem.getMaxRecord() > item.getMaxRecord();
                if (shouldPromote)
                {
                    // 向后移动被超过的项
                    _m_itemList.set(i, item);
                    item.setRank(i + 1);
                } else
                {
                    // 找到正确位置，停止冒泡
                    break;
                }
            }

            // 设置项目到最终位置
            _m_itemList.set(i, _oreRankItem);
            _oreRankItem.setRank(i + 1);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取排行榜前三数据
     * @return
     */
    public List<TreasureHunt_OreRankItem> makeTop3ProtoList()
    {
        _lock();
        try
        {
            if (_m_itemList.isEmpty())
                return null;

            List<TreasureHunt_OreRankItem> top3List = new ArrayList<>();
            for (int i = 0; i < Math.min(3, _m_itemList.size()); i++)
            {
                TreasureHuntRankItem item = _m_itemList.get(i);
                if (item != null)
                {
                    top3List.add(item.makeProto());
                }
            }
            return top3List;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过排名获取排行榜数据
     * @param _rank
     * @return
     */
    public TreasureHunt_OreRankItem lookupItemByRank(int _rank)
    {
        _lock();
        try
        {
            if (_rank < 1 || _rank > _m_itemList.size())
                return null;

            TreasureHuntRankItem item = _m_itemList.get(_rank - 1);
            if (item != null)
            {
                return item.makeProto();
            }
            return null;
        } finally
        {
            _unlock();
        }
    }
}
