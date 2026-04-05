package NPGameRes.Refs.Child;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "child_career")
public class RefChildCareer extends RefBase
{
    private static RefChildCareerMgr _g_mgr = new RefChildCareerMgr();
    public static RefChildCareerMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChildCareerMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChildCareerMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChildCareer newRef = (RefChildCareer) _newRef;
        career_id = newRef.career_id;
        career_group_id = newRef.career_group_id;
        career_add = newRef.career_add;
        career_rand_wei = newRef.career_rand_wei;
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
        return career_id;
    }
    
    public static class RefChildCareerMgr extends RefTableContainer<RefChildCareer>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long career_id;//子嗣相性id
    public int career_group_id;//职业随机组id
    public int career_add;//职业加成万分比
    public int career_rand_wei;//职业随机权重
}
