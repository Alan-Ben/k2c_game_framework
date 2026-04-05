package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "map_path")
public class RefMapPath extends RefBase
{
    private static RefListContainer<RefMapPath> _g_mgr = new RefListContainer<RefMapPath>();

    public static RefListContainer<RefMapPath> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMapPath> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMapPath>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapPath newRef = (RefMapPath) _newRef;
        map_id = newRef.map_id;
        group_id = newRef.group_id;
        point_id = newRef.point_id;
        next_point_id = newRef.next_point_id;
        research_distance = newRef.research_distance;
        area_id = newRef.area_id;
        is_research_enable = newRef.is_research_enable;
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
        return map_id * 10000000 + (group_id * 100000) + point_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long map_id;//地图ID    
    public int group_id;//组别ID    
    public int point_id;//点Id
    public int next_point_id; //下一个点的Id
    public int research_distance; //在本线段内移动需要重算的半径
    public int area_id;
    public boolean is_research_enable;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
