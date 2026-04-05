package NPGameRes.Refs.ActivityFund;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

/**
 * 活动基金配置表
 */
@RefTable(tableName = "activity_fund")
public class RefActivityFund extends RefBase
{
    private static RefActivityFundMgr _g_mgr = new RefActivityFundMgr();

    public static RefActivityFundMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityFundMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityFundMgr) _mgr;
    }

    public static class RefActivityFundMgr extends RefTableContainer<RefActivityFund>
    {
        @Override
        public void _onTableLoaded()
        {
        }

        public RefActivityFund getRefByActivityId(long _activityId)
        {
            for(RefActivityFund ref : this.getList())
            {
                if(ref.activity_id == _activityId)
                    return ref;
            }
            return null;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityFund newRef = (RefActivityFund) _newRef;
        activity_fund_id = newRef.activity_fund_id;
        activity_id = newRef.activity_id;
        task_group_id = newRef.task_group_id;
        task_refresh_time = newRef.task_refresh_time;
        process_cur_count = newRef.process_cur_count;
        refresh_process_cur_count_type = newRef.refresh_process_cur_count_type;
        mail_id = newRef.mail_id;
    }

    @Override
    public long Id()
    {
        return activity_fund_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 基金ID（主键）
    public long activity_fund_id;

    // 活动ID（0表示常驻基金）
    public long activity_id;

    // 任务组ID
    public long task_group_id;

    // 任务刷新时间 秒
    public int task_refresh_time;

    // 计数器的高级公式（ENPPlayerVariableType）
    public NPPlayerVariableGroupObj process_cur_count;

    // 刷新高级公式的事件枚举
    public String refresh_process_cur_count_type;

    // 补发邮件id
    public long mail_id;
}
