package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "map_area")
public class RefMapArea extends RefBase
{
    private static RefListContainer<RefMapArea> _g_mgr = new RefListContainer<RefMapArea>();

    public static RefListContainer<RefMapArea> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMapArea> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMapArea>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapArea newRef = (RefMapArea) _newRef;
        map_id = newRef.map_id;
        area_id = newRef.area_id;
        group_id = newRef.group_id;
        pos_a_x = newRef.pos_a_x;
        pos_a_y = newRef.pos_a_y;
        pos_b_x = newRef.pos_b_x;
        pos_b_y = newRef.pos_b_y;
        point_list = newRef.point_list;
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
        return map_id * 100 + area_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public long map_id;
    public int area_id;//建造 区域id
    public int group_id;//所属组别
    public int pos_a_x;//对角顶点a坐标x
    public int pos_a_y;//对角顶点a坐标y
    public int pos_b_x;//对角顶点b坐标x
    public int pos_b_y;//对角顶点b坐标y
    public String point_list;//多边形顶点

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
