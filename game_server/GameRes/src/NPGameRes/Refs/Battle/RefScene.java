package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Battle.WCGGTerrainIndex;

/**
 * @author scott 角 色 信 息
 */
@RefTable(tableName = "scene_info")
public class RefScene extends RefBase
{
    private static RefTableContainer<RefScene> _g_mgr = new RefTableContainer<RefScene>();

    public static RefTableContainer<RefScene> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefScene> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefScene>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefScene newRef = (RefScene) _newRef;
        id = newRef.id;
        name = newRef.name;
        terr_idx = newRef.terr_idx;
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
        return id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public String name;//场景名字

    public WCGGTerrainIndex terr_idx;    //对应地形数据的Id,如无数据则不填写，用于战斗场景

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
