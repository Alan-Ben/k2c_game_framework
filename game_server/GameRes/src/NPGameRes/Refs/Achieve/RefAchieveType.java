package NPGameRes.Refs.Achieve;

import CommonEnum.EAchieveType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "achieve_type")
public class RefAchieveType extends RefBase
{
    private static RefTableContainer<RefAchieveType> _g_mgr = new RefTableContainer<RefAchieveType>();

    public static RefTableContainer<RefAchieveType> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefAchieveType> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefAchieveType>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAchieveType newRef = (RefAchieveType) _newRef;
        type = newRef.type;
    }

    @Override
    public long Id()
    {
        return type.ordinal();
    }

    public EAchieveType type;
}
