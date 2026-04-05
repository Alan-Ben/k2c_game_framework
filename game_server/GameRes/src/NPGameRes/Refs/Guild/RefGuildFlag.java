package NPGameRes.Refs.Guild;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "guild_flag")
public class RefGuildFlag extends RefBase
{
    private static RefGuildFlagMgr _g_mgr = new RefGuildFlagMgr();

    public static RefGuildFlagMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildFlagMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildFlagMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildFlag newRef = (RefGuildFlag) _newRef;
        id = newRef.id;
    }

    public static class RefGuildFlagMgr extends RefTableContainer<RefGuildFlag>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
}