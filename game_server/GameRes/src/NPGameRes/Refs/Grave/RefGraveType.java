package NPGameRes.Refs.Grave;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

@RefTable(tableName = "grave_type")
public class RefGraveType extends RefBase
{
    private static RefGraveTypeMgr _g_mgr = new RefGraveTypeMgr();

    public static RefGraveTypeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return _g_mgr;
    }

    public static class RefGraveTypeMgr extends RefTableContainer<RefGraveType>
    {
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGraveTypeMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGraveType newRef = (RefGraveType) _newRef;
        id = newRef.id;
        player_title_id_list = newRef.player_title_id_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public ArrayList<Long> player_title_id_list = new ArrayList<>();
}
