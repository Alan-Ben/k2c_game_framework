package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "player_permissions")
public class RefPlayerPermissions extends RefBase
{
    private static RefTableContainer<RefPlayerPermissions> _g_mgr = new RefTableContainer<RefPlayerPermissions>();

    public static RefTableContainer<RefPlayerPermissions> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerPermissions> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerPermissions>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerPermissions newRef = (RefPlayerPermissions) _newRef;
        id = newRef.id;
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

    public long id;
}
