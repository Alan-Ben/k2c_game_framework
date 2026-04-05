package NPGameRes.Refs.CommonEvent;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "common_event")
public class RefCommonEvent extends RefBase
{
    private static RefTableContainer<RefCommonEvent> _g_mgr = new RefTableContainer<RefCommonEvent>();

    public static RefTableContainer<RefCommonEvent> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefCommonEvent> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefCommonEvent>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefCommonEvent newRef = (RefCommonEvent) _newRef;
        id = newRef.id;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;//事件id

    @RefField(isIgnore = true)
    public _ARefCommonEvent detailRef;

    /**
     * 设置事件详细数据
     */
    public void setDetailRef(_ARefCommonEvent _detailRef)
    {
        detailRef = _detailRef;
    }
}
