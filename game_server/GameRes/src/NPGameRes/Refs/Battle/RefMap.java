package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author scott 角 色 信 息
 */
@RefTable(tableName = "map_ref")
public class RefMap extends RefBase
{
    private static RefTableContainer<RefMap> _g_mgr = new RefTableContainer<RefMap>();

    public static RefTableContainer<RefMap> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefMap> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefMap>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMap newRef = (RefMap) _newRef;
        map_id = newRef.map_id;
        Name = newRef.Name;
        desc = newRef.desc;
        player_group = newRef.player_group;
        scene_id = newRef.scene_id;
        camp_list = newRef.camp_list;
        groupList = newRef.groupList;
        surrender_time = newRef.surrender_time;
        surrender_cd = newRef.surrender_cd;
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
        return map_id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long map_id; // 地图ID
    public String Name;// 地图名字
    public String desc;// 地图介绍
    public ArrayList<Integer> player_group;// 玩家组别列表
    public long scene_id;// 地图资源id

    public String camp_list;
    public String groupList;

    public long surrender_time;//战局开始多久后可以发起投降.单位(毫秒) 
    public long surrender_cd;//发起投降后多久可以再次投降.单位(毫秒)


    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
