package NPGameRes.Refs.Chapter;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 章节金币消耗倍率配表
 *
 * 根据战力比例区间配置金币消耗的倍率
 * 战力比例 = (玩家战力 - 关卡战力) / 关卡战力 * 10000
 */
@RefTable(tableName = "chapter_cost")
public class RefChapterCost extends RefBase
{
    private static RefChapterCostMgr _g_mgr = new RefChapterCostMgr();

    public static RefChapterCostMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterCostMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterCostMgr) _mgr;
    }

    public static class RefChapterCostMgr extends RefTableContainer<RefChapterCost>
    {
        @Override
        public void _onTableLoaded()
        {

        }

        /**
         * 根据战力比例获取对应的金币消耗倍率
         *
         * @param _powerRate 战力比例(万分比)，计算公式：(玩家战力 - 关卡战力) / 关卡战力 * 10000
         * @return 金币消耗倍率(万分比)，未找到返回10000(即1倍)
         */
        public int getCostMultipleRate(int _powerRate)
        {
            for (RefChapterCost ref : getList())
            {
                if (ref.power_rate_start <= _powerRate && _powerRate <= ref.power_rate_end)
                {
                    return ref.cost_multiple_rate;
                }
            }
            // 未找到匹配区间，返回默认倍率1倍
            return 10000;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapterCost newRef = (RefChapterCost) _newRef;
        id = newRef.id;
        power_rate_start = newRef.power_rate_start;
        power_rate_end = newRef.power_rate_end;
        cost_multiple_rate = newRef.cost_multiple_rate;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 配表字段
    public long id; // 配置ID
    public int power_rate_start; // 战力比例区间(万分比)开始
    public int power_rate_end; // 战力比例区间(万分比)结束
    public int cost_multiple_rate; // 金币消耗倍率(万分比)
}