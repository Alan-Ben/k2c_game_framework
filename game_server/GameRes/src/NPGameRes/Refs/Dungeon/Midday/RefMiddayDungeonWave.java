package NPGameRes.Refs.Dungeon.Midday;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "midday_dungeon_wave")
public class RefMiddayDungeonWave extends RefBase
{
    private static RefMiddayDungeonWaveMgr _g_mgr = new RefMiddayDungeonWaveMgr();

    public static RefMiddayDungeonWaveMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefMiddayDungeonWaveMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMiddayDungeonWaveMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMiddayDungeonWave newRef = (RefMiddayDungeonWave) _newRef;
        wave = newRef.wave;
        boss_blood = newRef.boss_blood;
        dungeon_coin = newRef.dungeon_coin;
        hero_exp_reward_ratio = newRef.hero_exp_reward_ratio;
        item_list = newRef.item_list;
        box_drop_per = newRef.box_drop_per;
        box_id = newRef.box_id;
    }

    public static class RefMiddayDungeonWaveMgr extends RefTableContainer<RefMiddayDungeonWave>
    {
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return wave;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long wave; //唯一id
    public long boss_blood;//boss血量
    public long dungeon_coin;//获得副本积分
    public int hero_exp_reward_ratio;//大臣经验奖励倍数（万分比）
    public List<NPCommonCostItem> item_list;//奖励列表
    public int box_drop_per;//宝箱掉落概率（万分比）
    public long box_id;//宝箱id
}