package NPGameRes.Refs.Activity;

import Common.CrossTeamEnum.ENPCrossTeamJoinCond;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "activity_team")
public class RefActivityTeam extends RefBase
{
    private static RefActivityTeamMgr _g_mgr = new RefActivityTeamMgr();

    public static RefActivityTeamMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityTeamMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityTeamMgr) _mgr;
    }

    public static class RefActivityTeamMgr extends RefTableContainer<RefActivityTeam>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityTeam newRef = (RefActivityTeam) _newRef;
        activity_id = newRef.activity_id;
        member_limit = newRef.member_limit;
        apply_cond_type = newRef.apply_cond_type;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return activity_id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long activity_id;//关联活动id
    public int member_limit;//队伍人数上限
    public ENPCrossTeamJoinCond apply_cond_type = ENPCrossTeamJoinCond.NONE;//申请条件类型
}
