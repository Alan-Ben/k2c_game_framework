package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "consort_cg")
public class RefConsortCg extends RefBase
{
    private static RefConsortCgMgr _g_mgr = new RefConsortCgMgr();
    public static RefConsortCgMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortCg> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortCgMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortCg newRef = (RefConsortCg) _newRef;
        cg_id = newRef.cg_id;
        consort_id = newRef.consort_id;
        add = newRef.add;
        cg_unlock_item_list = newRef.cg_unlock_item_list;
    }
    
    public static class RefConsortCgMgr extends RefTableContainer<RefConsortCg>
    {
    	@Override
        public void _onTableLoaded()
        {
        }
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
        return cg_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long cg_id;
    public long consort_id;//对应妃子id
    public int add;//加护点加成万分比
    public ArrayList<NPCommonCostItem> cg_unlock_item_list = new ArrayList<>();//cg解锁奖励列表
}
