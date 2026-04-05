package NPGameRes.Refs.Activity;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * 活动组配置
 */
@RefTable(tableName = "activity_group")
public class RefActivityGroup extends RefBase
{
    private static RefActivityGroupMgr _g_mgr = new RefActivityGroupMgr();

    public static RefActivityGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityGroupMgr) _mgr;
    }


    public static class RefActivityGroupMgr extends RefTableContainer<RefActivityGroup>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityGroup newRef = (RefActivityGroup) _newRef;
        group_id = newRef.group_id;
        activity_id_list = newRef.activity_id_list;
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
        return group_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long group_id;
    public ArrayList<Long> activity_id_list = new ArrayList<>();

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
