package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function;

import NPGameRes.Refs.Building.RefBusinessBuildingProduct;
import USDB.Bo.PlayerBuildingBusinessProductBO;

public class BuildingBusinessProduct
{
    private RefBusinessBuildingProduct _m_ref;
    private long _m_dbId;

    public BuildingBusinessProduct(RefBusinessBuildingProduct _ref, PlayerBuildingBusinessProductBO _bo)
    {
        _m_ref = _ref;
        _m_dbId = _bo.getId();
    }

    public long getRefId()
    {
        return _m_ref.Id();
    }
}
