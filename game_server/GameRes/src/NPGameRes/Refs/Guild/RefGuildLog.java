package NPGameRes.Refs.Guild;

import Common.GuildEnum.EGuildLogShowType;
import Common.GuildEnum.EGuildLogType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "guild_log")
public class RefGuildLog extends RefBase
{
    private static RefGuildLogMgr _g_mgr = new RefGuildLogMgr();

    public static RefGuildLogMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildLogMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildLogMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildLog newRef = (RefGuildLog) _newRef;
        type = newRef.type;
        show_type_list = newRef.show_type_list;
    }

    public static class RefGuildLogMgr extends RefTableContainer<RefGuildLog>
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
        return type.ordinal();
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public EGuildLogType type; //唯一id
    public List<EGuildLogShowType> show_type_list; //显示类型列表
}