package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "dungeon_mob_instance")
public class RefMapMob extends RefBase
{
    private static RefListContainer<RefMapMob> _g_mgr = new RefListContainer<RefMapMob>();

    public static RefListContainer<RefMapMob> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMapMob> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMapMob>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapMob newRef = (RefMapMob) _newRef;
        dungeon_id = newRef.dungeon_id;
        mob_instance_id = newRef.mob_instance_id;
        mob_id = newRef.mob_id;
        x = newRef.x;
        y = newRef.y;
        group_id = newRef.group_id;
        patrol_pos_s = newRef.patrol_pos_s;
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
        return dungeon_id * 10000 + mob_instance_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long dungeon_id;//地图ID
    public long mob_instance_id;
    public long mob_id;
    public int x;//出生点x
    public int y;//出生点y
    public int group_id;//所属组别
    public String patrol_pos_s;//巡逻点


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
