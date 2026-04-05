package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "map_point")
public class RefMapPoint extends RefBase
{
    private static RefListContainer<RefMapPoint> _g_mgr = new RefListContainer<RefMapPoint>();

    public static RefListContainer<RefMapPoint> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefMapPoint> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefMapPoint>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMapPoint newRef = (RefMapPoint) _newRef;
        map_id = newRef.map_id;
        point_id = newRef.point_id;
        x = newRef.x;
        y = newRef.y;
        radius = newRef.radius;
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
        return map_id * 10000 + point_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long map_id;//地图ID

    public int point_id;//组别ID

    public int x;//路径的节点X

    public int y; //路径的节点Y

    public int radius; //移动到该点的最小允许范围


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
