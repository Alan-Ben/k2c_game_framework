package NPGameRes.Refs.Child;

import CommonEnum.EChildSexType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "child_init_res")
public class RefChildInitRes extends RefBase
{
    private static RefChildInitResMgr _g_mgr = new RefChildInitResMgr();
    public static RefChildInitResMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChildInitResMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChildInitResMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChildInitRes newRef = (RefChildInitRes) _newRef;
        id = newRef.id;
        sex = newRef.sex;
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
    
    public static class RefChildInitResMgr extends RefTableContainer<RefChildInitRes>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public EChildSexType sex = EChildSexType.NONE;
}
