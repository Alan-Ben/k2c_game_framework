package NPGameRes.Refs.Player;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "player_create_player_prefab")
public class RefPlayerCreatePlayerPrefab extends RefBase
{
    private static RefTableContainer<RefPlayerCreatePlayerPrefab> _g_mgr = new RefTableContainer<RefPlayerCreatePlayerPrefab>();

    public static RefTableContainer<RefPlayerCreatePlayerPrefab> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerCreatePlayerPrefab> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerCreatePlayerPrefab>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerCreatePlayerPrefab newRef = (RefPlayerCreatePlayerPrefab) _newRef;
        id = newRef.id;
        skin_id = newRef.skin_id;
        icon_id = newRef.icon_id;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }
    ////////////////////////

    public long id;
    public long skin_id;//皮肤id
    public long icon_id;//头像id
}
