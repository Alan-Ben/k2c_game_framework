package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function;

import NPGameRes.Refs.Building.RefBusinessBuildingDevelop;

public class BuildingBusinessDevelop
{
    private RefBusinessBuildingDevelop _m_ref;

    public BuildingBusinessDevelop(RefBusinessBuildingDevelop _ref)
    {
        _m_ref = _ref;
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }
}
