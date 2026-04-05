package NPGameRes.Refs.CountdownEvent;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

import java.util.ArrayList;

/**
 * @author mark 通用任务配置
 */
@RefTable(tableName = "countdown_event_task")
public class RefCountdownEventTask extends RefBase
{
    private static RefCountdownEventTaskMgr _g_mgr = new RefCountdownEventTaskMgr();

    public static RefCountdownEventTaskMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefCountdownEventTaskMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefCountdownEventTaskMgr) _mgr;
    }

    public static class RefCountdownEventTaskMgr extends RefTableContainer<RefCountdownEventTask>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCountdownEventTask newRef = (RefCountdownEventTask) _newRef;
        id = newRef.id;
        process_cur_count = newRef.process_cur_count;
        process_count = newRef.process_count;
        trigger_count_rate = newRef.trigger_count_rate;
        trigger_event = newRef.trigger_event;
        trigger_condition = newRef.trigger_condition;
        is_set = newRef.is_set;
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
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //主键 id
    public NPPlayerVariableGroupObj process_cur_count; //进度当前值（高级公式）(计数器的话可以不用配置)(进度条计数目标达到时就算完成)
    public long process_count; //进度条计数目标值
    public NPCountRate trigger_count_rate; //触发器count累计到进度值的倍率
    public ArrayList<String> trigger_event = null; //触发器-事件
    public NPPlayerConditionGroupObj trigger_condition; //触发器-触发条件
    public boolean is_set;//是否设置值 (默认FALSE) TRUE 数值为覆盖模式 FALSE 数值为增量统计模式

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
