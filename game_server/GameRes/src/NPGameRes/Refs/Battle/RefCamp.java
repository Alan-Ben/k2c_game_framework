package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

import java.util.ArrayList;

/**
 * @author scott
 */
@RefTable(tableName = "map_camp")
public class RefCamp extends RefBase
{
    private static RefListContainer<RefCamp> _g_mgr = new RefListContainer<RefCamp>();

    public static RefListContainer<RefCamp> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefCamp> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefCamp>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCamp newRef = (RefCamp) _newRef;
        map_id = newRef.map_id;
        camp_id = newRef.camp_id;
        group_list = newRef.group_list;
        share_group = newRef.share_group;
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
        return map_id * 100 + camp_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int map_id;
    public int camp_id; //势力ID
    public ArrayList<Integer> group_list;  //本势力下的Group集合
    public int share_group;   //共享组(目前是单一组状态)

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////


}
