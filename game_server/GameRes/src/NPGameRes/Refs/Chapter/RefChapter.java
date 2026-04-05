package NPGameRes.Refs.Chapter;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLongList;

import java.util.List;

@RefTable(tableName = "chapter")
public class RefChapter extends RefBase
{
    private static RefChapterMgr _g_mgr = new RefChapterMgr();

    public static RefChapterMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChapterMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChapterMgr) _mgr;
    }

    public static class RefChapterMgr extends RefTableContainer<RefChapter>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }


    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChapter newRef = (RefChapter) _newRef;
        chapter_id = newRef.chapter_id;
        initial_power = newRef.initial_power;
        initial_reward_exp = newRef.initial_reward_exp;
        initial_cost = newRef.initial_cost;
        power_add_rate = newRef.power_add_rate;
        reward_exp_add_rate = newRef.reward_exp_add_rate;
        cost_add_rate = newRef.cost_add_rate;
        point_count = newRef.point_count;
        point_event = newRef.point_event;
        boss_power = newRef.boss_power;
        reward_list = newRef.reward_list;
        gold_inspire_base_value = newRef.gold_inspire_base_value;
        crit_probability = newRef.crit_probability;
        reward_player_exp = newRef.reward_player_exp;
    }

    @Override
    public long Id()
    {
        return chapter_id;
    }

    public long chapter_id;//章节id
    public long initial_power;//战力需求
    public long initial_reward_exp;//关卡前进经验奖励(HERO_EXP)
    public long initial_cost;//金币前进消耗
    public List<Integer> power_add_rate;//战力需求递增比例列表（万分比）
    public List<Integer> reward_exp_add_rate;//关卡经验递增比例列表（万分比）
    public List<Integer> cost_add_rate;//金币前进消耗递增比例列表（万分比）
    public int point_count;//一共有几波(包括boss)
    public WCGPairLongList point_event;//对应触发事件
    public long boss_power;//boss战力
    public List<NPCommonCostItem> reward_list;//boss战奖励
    public long gold_inspire_base_value;//boss战金币鼓舞基础值
    public int crit_probability;//每一波暴击概率
    public long reward_player_exp;//每前进1次获得玩家经验奖励

    /**
     * 计算目标格子所需的战力
     * @return
     */
    public ResultOne<Long> calNeedPower(int _point)
    {
        //判断越界
        if(_point < 1 || _point > point_count)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        long power = (long) Math.ceil((double) initial_power * (10000 + power_add_rate.get(_point - 1)) / 10000);
        return ResultOne.succ(power);
    }

    /**
     * 计算目标格子所需的金币
     * @return
     */
    public ResultOne<Double> calNeedCost(int _point)
    {
        //判断越界
        if(_point < 1 || _point > point_count)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        double cost = (double) initial_cost * (10000 + cost_add_rate.get(_point - 1)) / 10000;
        return ResultOne.succ(cost);
    }

    /**
     * 计算目标格子奖励的经验
     * @return
     */
    public ResultOne<Long> calRewardExp(int _point)
    {
        //判断越界
        if(_point < 1 || _point > point_count)
            return ResultOne.failed(CommErr.PARAM_ERROR);

        long rewardExp = (long) Math.ceil((double)initial_reward_exp * (10000 + reward_exp_add_rate.get(_point - 1)) / 10000);
        return ResultOne.succ(rewardExp);
    }
}
