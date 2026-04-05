package NPGameRes.Refs.Marquee;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "marquee")
public class RefMarquee extends RefBase
{
    private static RefMarqueeMgr _g_mgr = new RefMarqueeMgr();
    public static RefMarqueeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefMarqueeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarqueeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarquee newRef = (RefMarquee) _newRef;
        id = newRef.id;
        show_pos_id = newRef.show_pos_id;
        priority_id = newRef.priority_id;
        duration_sec = newRef.duration_sec;
        duration_count = newRef.duration_count;
        life_sec = newRef.life_sec;
        ui_res_id = newRef.ui_res_id;
        offline_need_show = newRef.offline_need_show;
    }
    
    public static class RefMarqueeMgr extends RefTableContainer<RefMarquee>
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

    public long id; //唯一id
    public int show_pos_id;//窗口展示队列
    public int priority_id;//优先级（越大越优先）
    public int duration_sec;//循环播放时长秒（优先于次数）
    public int duration_count;//循环播放次数
    public int life_sec = -1;//生存时间秒，-1表示即时过期
    public long ui_res_id;//预制体ID
    public boolean offline_need_show;//玩家离线期间是否需要展示
}
