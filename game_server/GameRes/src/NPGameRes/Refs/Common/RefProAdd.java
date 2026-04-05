package NPGameRes.Refs.Common;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "pro_add")
public class RefProAdd extends RefBase
{
    private static RefProAddMgr _g_mgr = new RefProAddMgr();

    public static RefProAddMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefProAddMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefProAddMgr) _mgr;
    }

    public static class RefProAddMgr extends RefTableContainer<RefProAdd>
    {
    	@Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefProAdd newRef = (RefProAdd) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        add = newRef.add;
        random_weight = newRef.random_weight;
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
    public int group_id;//加成组id
    public int add; //加成万分比
    public int random_weight;//成功概率万分比
}
