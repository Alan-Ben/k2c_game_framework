package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "map_group")
public class RefMapGroup extends RefBase
{
    private static RefListContainer<RefMapGroup> _g_mgr = new RefListContainer<RefMapGroup>();

    public static RefListContainer<RefMapGroup> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMapGroup> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMapGroup>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapGroup newRef = (RefMapGroup) _newRef;
        map_id = newRef.map_id;
        group_id = newRef.group_id;
        friend_group = newRef.friend_group;
        share_res_group = newRef.share_res_group;
        default_race = newRef.default_race;
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
        return map_id * 100 + group_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long map_id;
    public int group_id;
    public ArrayList<Integer> friend_group;
    public ArrayList<Integer> share_res_group;
    public long default_race;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
