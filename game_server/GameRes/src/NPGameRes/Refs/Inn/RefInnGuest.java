package NPGameRes.Refs.Inn;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "inn_guest")
public class RefInnGuest extends RefBase
{
    // 1. 静态管理器实例
    private static RefInnGuestMgr _g_mgr = new RefInnGuestMgr();

    // 2. 静态方法
    public static RefInnGuestMgr getMgr()
    {
        return _g_mgr;
    }

    // 3. 基类方法实现
    @Override
    public RefInnGuestMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefInnGuestMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefInnGuest newRef = (RefInnGuest) _newRef;
        id = newRef.id;
        unlock_condition_list = newRef.unlock_condition_list;
        finesse_add = newRef.finesse_add;
        handbook_reward = newRef.handbook_reward;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // 4. 管理器内部类
    public static class RefInnGuestMgr extends RefTableContainer<RefInnGuest>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 配表加载完成后的处理逻辑
        }
    }

    // 5. 配表字段定义
    public long id; // 客人ID（主键）
    public List<NPPlayerConditionGroupObj> unlock_condition_list; // 解锁条件
    public long finesse_add; // 熟练度加成
    public List<NPCommonCostItem> handbook_reward; // 图鉴奖励
}