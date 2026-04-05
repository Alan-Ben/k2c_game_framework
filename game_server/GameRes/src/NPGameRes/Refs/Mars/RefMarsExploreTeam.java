package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "mars_explore_team")
public class RefMarsExploreTeam extends RefBase
{
    private static RefMarsExploreTeamMgr _g_mgr = new RefMarsExploreTeamMgr();

    public static RefMarsExploreTeamMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreTeamMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreTeamMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreTeam newRef = (RefMarsExploreTeam) _newRef;
        team_id = newRef.team_id;
        unlock_cond = newRef.unlock_cond;
    }

    @Override
    public long Id()
    {
        return team_id;
    }

    public static class RefMarsExploreTeamMgr extends RefTableContainer<RefMarsExploreTeam>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long team_id;//队伍ID
    public NPPlayerConditionGroupObj unlock_cond;//解锁条件
}