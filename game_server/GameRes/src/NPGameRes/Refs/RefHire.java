package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

@RefTable(tableName = "hire")
public class RefHire extends RefBase
{
    private static RefListContainer<RefHire> _g_mgr = new RefListContainer<>();

    public static RefListContainer<RefHire> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefListContainer<RefHire> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefListContainer<RefHire>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefHire newRef = (RefHire) _newRef;
        id = newRef.id;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }

    public long id;

}
