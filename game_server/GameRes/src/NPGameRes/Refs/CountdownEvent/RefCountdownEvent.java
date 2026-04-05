package NPGameRes.Refs.CountdownEvent;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "countdown_event")
public class RefCountdownEvent extends RefBase
{
    private static RefCountdownEventMgr _g_mgr = new RefCountdownEventMgr();

    public static RefCountdownEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefCountdownEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefCountdownEventMgr) _mgr;
    }

    public static class RefCountdownEventMgr extends RefTableContainer<RefCountdownEvent>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCountdownEvent newRef = (RefCountdownEvent) _newRef;
        id = newRef.id;
        can_trigger_num = newRef.can_trigger_num;
        undone_need_reset = newRef.undone_need_reset;
        quest_id = newRef.quest_id;
        duration = newRef.duration;
        done_cost_list = newRef.done_cost_list;
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
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //主键 id
    public int can_trigger_num; //可以触发次数
    public boolean undone_need_reset; //可以触发次数
    public long quest_id; //任务id
    public int duration; //CD时长(秒)

    //GOB-9362【优化-0】倒计时事件支持完成事件时扣除物品 https://www.teambition.com/task/69aa4585adb4944ed7c43a3d
    public ArrayList<NPCommonCostItem> done_cost_list = new ArrayList<>();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
