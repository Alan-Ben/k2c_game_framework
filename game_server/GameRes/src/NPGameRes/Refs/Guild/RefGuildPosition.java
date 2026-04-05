package NPGameRes.Refs.Guild;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "guild_position")
public class RefGuildPosition extends RefBase
{
    private static RefGuildPositionMgr _g_mgr = new RefGuildPositionMgr();

    public static RefGuildPositionMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildPositionMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildPositionMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildPosition newRef = (RefGuildPosition) _newRef;
        type = newRef.type;
        name = newRef.name;
        pre_position = newRef.pre_position;
        permission_list = newRef.permission_list;
        can_appoint_position_list = newRef.can_appoint_position_list;
        leader_passive_transfer_priority = newRef.leader_passive_transfer_priority;
        trans_need_historical_contributions = newRef.trans_need_historical_contributions;
    }

    /**
     * 检查是否有某个权限
     * @param _needPermission
     * @return
     */
    public boolean checkPermission(EGuildPermissionType _needPermission)
    {
        return permission_list.contains(_needPermission);
    }

    /**
     * 是否可以任命某个职位
     * @param _positionType
     * @return
     */
    public boolean checkCanAppoint(EGuildPositionType _positionType)
    {
        return can_appoint_position_list.contains(_positionType);
    }

    public static class RefGuildPositionMgr extends RefTableContainer<RefGuildPosition>
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

    public EGuildPositionType type;
    public String name;
    public EGuildPositionType pre_position;
    public List<EGuildPermissionType> permission_list;
    public List<EGuildPositionType> can_appoint_position_list;
    public int leader_passive_transfer_priority;
    public int trans_need_historical_contributions;
}