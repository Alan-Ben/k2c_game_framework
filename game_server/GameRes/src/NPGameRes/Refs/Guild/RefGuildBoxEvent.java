package NPGameRes.Refs.Guild;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "guild_box_event")
public class RefGuildBoxEvent extends RefBase
{
    private static RefGuildBoxEventMgr _g_mgr = new RefGuildBoxEventMgr();

    public static RefGuildBoxEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildBoxEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildBoxEventMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildBoxEvent newRef = (RefGuildBoxEvent) _newRef;
        box_id = newRef.box_id;
        logic_event = newRef.logic_event;
        trigger_per = newRef.trigger_per;
        trigger_count_rate = newRef.trigger_count_rate;
    }

    public static class RefGuildBoxEventMgr extends RefTableContainer<RefGuildBoxEvent>
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
        return box_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long box_id;//宝箱 id
    public String logic_event;//监控事件枚举列表
    public int trigger_per;//触发概率(万分比)
    public NPCountRate trigger_count_rate;//触发数量（数量 或 事件参数*倍数）
}