package NPGameRes.Refs.Guild;

import Common.GuildEnum.EGuildBoxType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "guild_box")
public class RefGuildBox extends RefBase
{
    private static RefGuildBoxMgr _g_mgr = new RefGuildBoxMgr();

    public static RefGuildBoxMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildBoxMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildBoxMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildBox newRef = (RefGuildBox) _newRef;
        id = newRef.id;
        type = newRef.type;
        reward_id = newRef.reward_id;
        gain_guild_active_point = newRef.gain_guild_active_point;
    }

    public static class RefGuildBoxMgr extends RefTableContainer<RefGuildBox>
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

    public long id;//唯一id
    public EGuildBoxType type = EGuildBoxType.NONE;//宝箱类型（枚举：EGuildBoxType）
    public long reward_id;//奖励
    public int gain_guild_active_point;//获得的联盟活跃点
}