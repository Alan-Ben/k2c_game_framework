package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "simple_unlock")
public class RefSimpleUnlock extends RefBase
{
    private static RefTableContainer<RefSimpleUnlock> _g_mgr = new RefTableContainer<RefSimpleUnlock>();

    public static RefTableContainer<RefSimpleUnlock> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefSimpleUnlock> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefSimpleUnlock>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSimpleUnlock newRef = (RefSimpleUnlock) _newRef;
        id = newRef.id;
        condition_info = newRef.condition_info;
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

    //////////////////////////////
    public long id;                 //关联ID
    public NPPlayerConditionGroupObj condition_info;        //条件集合

}
