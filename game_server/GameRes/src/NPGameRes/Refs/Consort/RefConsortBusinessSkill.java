package NPGameRes.Refs.Consort;

import CommonEnum.ESpecAttrType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "consort_business_skill")
public class RefConsortBusinessSkill extends RefBase
{
    private static RefConsortBusinessSkillMgr _g_mgr = new RefConsortBusinessSkillMgr();
    public static RefConsortBusinessSkillMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortBusinessSkill> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortBusinessSkillMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortBusinessSkill newRef = (RefConsortBusinessSkill) _newRef;
        id = newRef.id;
        property = newRef.property;
        unlock_need_intimacy = newRef.unlock_need_intimacy;
        normal_cost_group_id = newRef.normal_cost_group_id;
        advance_cost_group_id = newRef.advance_cost_group_id;
        normal_add_pro_group_id = newRef.normal_add_pro_group_id;
        advance_add_pro_group_id = newRef.advance_add_pro_group_id;
    }
    
    public static class RefConsortBusinessSkillMgr extends RefTableContainer<RefConsortBusinessSkill>
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
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public ESpecAttrType property = ESpecAttrType.NONE;//对应相性
    public int unlock_need_intimacy;//解锁所需亲密度
    public int normal_cost_group_id;//普通领悟消耗组id
    public int advance_cost_group_id;//高级领悟消耗组id
    public int normal_add_pro_group_id;//普通领悟加成概率组ID
    public int advance_add_pro_group_id;//高级领悟加成概率组ID
}
