package NPGameRes.Refs.Activity;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

@RefTable(tableName = "activity_plan")
public class RefActivityPlan extends RefBase
{
    private static RefActivityScheduleMgr _g_mgr = new RefActivityScheduleMgr();

    public static RefActivityScheduleMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityScheduleMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityScheduleMgr) _mgr;
    }

    public static class RefActivityScheduleMgr extends RefTableContainer<RefActivityPlan>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityPlan newRef = (RefActivityPlan) _newRef;
        id = newRef.id;
        start_day = newRef.start_day;
        duration_hour = newRef.duration_hour;
        rewarding_duration_sec = newRef.rewarding_duration_sec;
        activity_list = newRef.activity_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public int start_day; //第几天开始活动
    public int duration_hour; //活动持续小时
    public int rewarding_duration_sec; //领奖期持续时间 秒
    public ArrayList<Long> activity_list = new ArrayList<>(); //需要开启的活动列表
}