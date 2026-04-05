package NPGameRes.Refs.Tower;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "tower_chapter_stage")
public class RefTowerChapterStage extends RefBase
{
    private static RefTowerChapterMgr _g_mgr = new RefTowerChapterMgr();

    public static RefTowerChapterMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTowerChapterMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTowerChapterMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTowerChapterStage newRef = (RefTowerChapterStage) _newRef;
        id = newRef.id;
        next_stage_id = newRef.next_stage_id;
        levels_in_range_count = newRef.levels_in_range_count;
        diamond_reward_per_level = newRef.diamond_reward_per_level;
        base_power_per_level = newRef.base_power_per_level;
        power_growth_ratio = newRef.power_growth_ratio;
        daily_tower_coin_output_base = newRef.daily_tower_coin_output_base;
        tower_coin_growth_value = newRef.tower_coin_growth_value;
        tower_coin_reward_ratio_per_level = newRef.tower_coin_reward_ratio_per_level;
    }

    public static class RefTowerChapterMgr extends RefTableContainer<RefTowerChapterStage>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//stage_id
    public long next_stage_id;//下一个stage_id
    public int levels_in_range_count;//区间内关卡数量

    public int diamond_reward_per_level;//到达每关的钻石奖励

    public long base_power_per_level;//关卡实力基础值
    public int power_growth_ratio;//实力增长万分比

    public long daily_tower_coin_output_base;//每日迷宫币产出基础值
    public int tower_coin_growth_value;//迷宫币增长值
    public int tower_coin_reward_ratio_per_level;//到达每关的迷宫币奖励万分比系数
}
