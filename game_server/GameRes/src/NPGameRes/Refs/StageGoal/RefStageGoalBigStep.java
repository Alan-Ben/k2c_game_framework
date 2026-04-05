package NPGameRes.Refs.StageGoal;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "stage_goal_big_step")
public class RefStageGoalBigStep extends RefBase
{
    private static RefStageGoalBigStepMgr _g_mgr = new RefStageGoalBigStepMgr();

    public static RefStageGoalBigStepMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefStageGoalBigStepMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefStageGoalBigStepMgr) _mgr;
    }

    public static class RefStageGoalBigStepMgr extends RefTableContainer<RefStageGoalBigStep>
    {
        @Override
        public void _onTableLoaded()
        {

        }

        /**
         * 获取对应的大阶段目标配表
         * @param _step
         */
        public RefStageGoalBigStep getBigStep(long _step)
        {
            RefStageGoalBigStep bigStepRef = null;

            for (RefStageGoalBigStep ref : getList())
            {
                if (ref == null)
                    continue;

                if (ref.begins_from_small_step > _step)
                    break;

                bigStepRef = ref;
            }

            return bigStepRef;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefStageGoalBigStep newRef = (RefStageGoalBigStep) _newRef;
        big_step = newRef.big_step;
        begins_from_small_step = newRef.begins_from_small_step;
        reward_item_list = newRef.reward_item_list;
        done_need_draw_all_step_reward = newRef.done_need_draw_all_step_reward;
        first_reach_reward_item_list = newRef.first_reach_reward_item_list;
        title = newRef.title;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return big_step;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long big_step; //主键 id
    public long begins_from_small_step; //对应开始小阶段
    public List<NPCommonCostItem> reward_item_list = new ArrayList<>();
    public boolean done_need_draw_all_step_reward;//是否需要领取所有小阶段奖励才可以领奖
    public List<NPCommonCostItem> first_reach_reward_item_list;//大阶段首达奖励列表
    public String title;//标题

    @RefField(isIgnore = true)
    public List<RefStageGoal> stage_goal_list = new ArrayList<>(); //阶段目标列表
    public void setStageGoals(List<RefStageGoal> stageGoals)
    {
        stage_goal_list = stageGoals;
    }


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
