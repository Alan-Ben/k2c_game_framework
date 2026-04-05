package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "mars_people_letter")
public class RefMarsPeopleLetter extends RefBase
{
    private static RefMarsPeopleLetterMgr _g_mgr = new RefMarsPeopleLetterMgr();

    public static RefMarsPeopleLetterMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsPeopleLetterMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsPeopleLetterMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsPeopleLetter newRef = (RefMarsPeopleLetter) _newRef;
        id = newRef.id;
        refresh_cond = newRef.refresh_cond;
        create_wei = newRef.create_wei;
        need_deal = newRef.need_deal;
        resolve_cond = newRef.resolve_cond;
        created_satisfaction_change = newRef.created_satisfaction_change;
        deal_satisfaction_change = newRef.deal_satisfaction_change;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsPeopleLetterMgr extends RefTableContainer<RefMarsPeopleLetter>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;
    public NPPlayerConditionGroupObj refresh_cond;//刷新条件
    public int create_wei;//随机权重
    public boolean need_deal;//是否需要处理
    public NPPlayerConditionGroupObj resolve_cond;//处理完成条件
    public int created_satisfaction_change;//生成后的满意度变化
    public int deal_satisfaction_change;// 处理完成后的满意度变化
}