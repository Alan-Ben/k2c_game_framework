package NPGameRes.Refs.StageGoal;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "stage_goal")
public class RefStageGoal extends RefBase
{
    private static RefStageGoalMgr _g_mgr = new RefStageGoalMgr();

    public static RefStageGoalMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefStageGoalMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefStageGoalMgr) _mgr;
    }

    public static class RefStageGoalMgr extends RefTableContainer<RefStageGoal>
    {
        @Override
        public void _onTableLoaded()
        {
        	
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefStageGoal newRef = (RefStageGoal) _newRef;
        step = newRef.step;
        next_step_need_server_start_day = newRef.next_step_need_server_start_day;
        next_step_simple_unlock_id = newRef.next_step_simple_unlock_id;
        task_list = newRef.task_list;
        reward_item_list = newRef.reward_item_list;
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
        return step;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long step; //主键 id
    public int next_step_need_server_start_day; //进入下一阶段要求的服务器天数
    public int next_step_simple_unlock_id; //进入下一阶段解锁条件id
    public List<Long> task_list = new ArrayList<>(); //任务id列表
    public List<NPCommonCostItem> reward_item_list = new ArrayList<>();

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
